using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
namespace OpenChess.Application
{
    class ChessHub(IMediator mediator, ConnectionTrackingService connectionTrackingService, MatchTrackingService matchTrackingService) : Hub<IPlayers>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ConnectionTrackingService _connectionTrackingService = connectionTrackingService;
        private readonly MatchTrackingService _matchTrackingService = matchTrackingService;

        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var playerId = (Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value) ?? throw new HubException("PlayerId not found!");
            await _connectionTrackingService.AddConnectionAsync(playerId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

    }

}