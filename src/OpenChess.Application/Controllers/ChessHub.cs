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

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var playerId = (Context.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value) ?? throw new HubException("PlayerId not found!");
            await _connectionTrackingService.RemoveConnectionAsync(playerId, Context.ConnectionId);
            string? matchId = await _matchTrackingService.GetMatchIdFromPlayerAsync(playerId);
            await _matchTrackingService.RemovePlayerFromMatchAsync(matchId!, playerId, Context.ConnectionId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, matchId!);
            _ = HandleTimeOutAsync(matchId, playerId);
            await base.OnDisconnectedAsync(exception);
        }

        private async Task HandleTimeOutAsync(string matchId, string playerId)
        {
            await Task.Delay(TimeSpan.FromSeconds(30));

            var isConnected = await _connectionTrackingService.IsPlayerConnectedAsync(playerId);

            if (!isConnected)
            {
                _ = _mediator.Send(new TimeoutMatchCommand(matchId, playerId));
            }
        }
    }

}