using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using OpenChess.Domain;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.RateLimiting;
namespace OpenChess.Application
{
    [ApiController]
    class MatchController(IMatchRepository matchRepository, IConfiguration configuration, IMediator mediator, IHubContext<ChessHub> hubContext, ConnectionTrackingService connectionTrackingService, MatchTrackingService matchTrackingService) : ControllerBase
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly IConfiguration _configuration = configuration;
        private readonly IMediator _mediator = mediator;
        private readonly IHubContext<ChessHub> _hubContext = hubContext;
        private readonly ConnectionTrackingService _connectionTrackingService = connectionTrackingService;
        private readonly MatchTrackingService _matchTrackingService = matchTrackingService;

        // Endpoint used only by matchmaking service
        [HttpPost("internal/api/matches")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateMatch([FromBody] int time)
        {
            try
            {
                MatchInfo match = await _mediator.Send(new CreateMatchCommand(time));
                var locationUrl = Url.Action(
                    action: nameof(CreateMatch),
                    controller: "Matches",
                    values: new { id = match.MatchId },
                    protocol: Request.Scheme
                );
                return Created(locationUrl, new { matchId = match.MatchId });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("api/matches/{id}/actions/join")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(MatchInfo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> JoinMatch(Guid matchId)
        {
            try
            {
                await IsConnectedToMatch();
                var playerId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                MatchInfo match = await _mediator.Send(new JoinMatchCommand(matchId.ToString(), playerId, 0));
                List<string> connectionIds = await _connectionTrackingService.GetPlayerConnectionsAsync(playerId);
                string playerConnectionId = connectionIds.First();
                await _matchTrackingService.JoinMatchAsync(matchId.ToString(), playerId, playerConnectionId);
                await _hubContext.Groups.AddToGroupAsync(playerConnectionId, matchId.ToString());

                return Ok(new { match });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("api/matches/{id}/actions/play")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Play([FromRoute] string matchId, [FromBody] string origin, string destination, string? promoting = null)
        {
            try
            {
                await IsConnectedToMatch();
                var playerId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                await _mediator.Send(new PlayMoveCommand(matchId, playerId, origin, destination, promoting));
                MatchInfo match = await _mediator.Send(new GetMatchCommand(matchId));
                await _hubContext.Clients.Group(matchId).SendAsync("GameState", match);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("api/matches/{id}/actions/resign")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Resign([FromRoute] string matchId)
        {
            try
            {
                await IsConnectedToMatch();
                var playerId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                await _mediator.Send(new ResignCommand(matchId, playerId));
                var match = await _mediator.Send(new GetMatchCommand(matchId));
                await _hubContext.Clients.Group(matchId).SendAsync("GameState", match);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("api/matches/{id}/actions/timeout")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Timeout([FromRoute] string matchId)
        {
            try
            {
                await IsConnectedToMatch();
                var playerId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                await _mediator.Send(new TimeoutMatchCommand(matchId, playerId));
                var match = await _mediator.Send(new GetMatchCommand(matchId));
                await _hubContext.Clients.Group(matchId).SendAsync("GameState", match);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [EnableRateLimiting("Fixed")]
        [HttpPost("api/matches")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(MatchInfo))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCustomMatch([FromBody] int time, int playerColor)
        {
            try
            {
                ColorUtils.TryParseColor(playerColor);
                var playerId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(playerId)) { return BadRequest("Player ID claim not found."); }

                MatchInfo match = await _mediator.Send(new CreateMatchCommand(time));
                MatchInfo matchInfo = await _mediator.Send(new JoinMatchCommand(match.MatchId.ToString(), playerId, playerColor));

                return CreatedAtAction(nameof(CreateMatch), new { id = match.MatchId }, new { match = matchInfo });
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("api/players")]
        [EnableRateLimiting("Fixed")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreatePlayer()
        {
            var existingToken = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string playerId = Guid.NewGuid().ToString();

            if (!string.IsNullOrEmpty(existingToken))
            {
                var principal = ValidateToken(existingToken);
                if (principal is null) return BadRequest("Invalid token.");
                var existingPlayerId = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (!string.IsNullOrEmpty(existingPlayerId))
                {
                    playerId = existingPlayerId;
                }
            }
            string token = GenerateToken(playerId);
            Response.Headers.Append("Authorization", $"Bearer {token}");

            return Ok(playerId);
        }

        private string GenerateToken(string playerId)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, playerId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        private ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out var securityToken);
            return principal;
        }

        private TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
            };
        }

        private async Task<IActionResult?> IsConnectedToMatch()
        {
            var playerId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(playerId)) { return Unauthorized(); }
            var isConnected = await _connectionTrackingService.IsPlayerConnectedAsync(playerId);
            var isInMatch = await _matchTrackingService.IsPlayerInMatchAsync(playerId);
            if (!isConnected || !isInMatch) return Unauthorized();

            return null;
        }
    }
}