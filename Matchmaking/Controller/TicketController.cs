using System.Net.Mime;
using MatchMaking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Redis.OM;
using Redis.OM.Searching;

namespace MatchMaking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ILogger<TicketController> _logger;
        private readonly RedisCollection<Ticket> _tickets;

        public TicketController(ILogger<TicketController> logger, RedisConnectionProvider provider)
        {
            _logger = logger;
            _tickets = (RedisCollection<Ticket>)provider.RedisCollection<Ticket>();
        }

        [HttpPost]
        [EnableRateLimiting("Fixed")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Ticket))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTicketAsync([FromBody] UserDTO userDto)
        {
            bool isMissing = userDto.Score == 0 || userDto.MatchTime == 0;
            bool isInvalidScore = !Ticket.Scores().Contains(userDto.Score);
            bool isInvalidMatchTime = !Ticket.MatchAllowedTime().Contains(userDto.MatchTime);
            if (isMissing) return BadRequest("Request body is missing!");
            if (isInvalidScore) return BadRequest("Invalid guest skill level!");
            if (isInvalidMatchTime) return BadRequest("Invalid match time!");
            if (userDto.Score < 0 || userDto.Score > 3000) return BadRequest("Invalid player score!");

            Ticket ticket = new(Guid.NewGuid(), userDto.Score, userDto.MatchTime, DateTime.UtcNow.TimeOfDay);
            await _tickets.InsertAsync(ticket);

            CookieOptions cookieOptions = new()
            {
                Expires = new DateTimeOffset().AddSeconds(ticket.Timeout.TotalSeconds),
                HttpOnly = true,
                IsEssential = true,
            };
            Response.Cookies.Append("TicketId", ticket.Id.ToString(), cookieOptions);

            return Ok(ticket);
        }



        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteTicket()
        {
            ValidateCookies(Request);
            string? ticketId = TryGetTicketIdFromCookies(Request);
            Ticket? ticket = _tickets.FindById(ticketId!);
            if (ticket is null) return NotFound("Ticket not found!");

            _tickets.DeleteAsync(ticket);
            Response.Cookies.Delete(ticketId!);

            return Ok();
        }

        private IActionResult? ValidateCookies(HttpRequest request)
        {
            try
            {
                var cookies = request.Cookies;
                if (cookies?.Count == 0) return NotFound("Ticket not found");
                var ticketId = cookies?.Where(c => c.Key == "TicketId").First().Value;
                if (ticketId is null) return BadRequest("Id is missing!");
                bool isGuid = Guid.TryParse(ticketId, out var result);
                if (!isGuid) return BadRequest("Invalid id!");
                return null;
            }
            catch (System.Exception)
            {
                return BadRequest("Invalid id!");
            }
        }

        private string? TryGetTicketIdFromCookies(HttpRequest request)
        {
            var cookies = request.Cookies;
            string? ticketId = cookies?.Where(c => c.Key == "TicketId").First().Value;
            return ticketId;
        }
    }
};