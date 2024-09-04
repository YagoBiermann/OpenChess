using System.Reflection.Metadata;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    public class ChessHub(IMediator mediator, ConnectionTrackingService connectionTrackingService, MatchTrackingService matchTrackingService) : Hub<IPlayers>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ConnectionTrackingService _connectionTrackingService = connectionTrackingService;
        private readonly MatchTrackingService _matchTrackingService = matchTrackingService;

        [Authorize]
        public async Task Join(string matchId)
        {
            try
            {
                var playerId = (Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new HubException("Player not authenticated.");
                JoinMatchDTO matchDto = await _mediator.Send(new JoinMatchCommand(matchId, playerId));
                List<string> connectionIds = await _connectionTrackingService.GetPlayerConnectionsAsync(playerId);
                string playerConnectionId = connectionIds.First();
                await _matchTrackingService.JoinMatchAsync(matchId.ToString(), playerId, playerConnectionId);
                await Groups.AddToGroupAsync(playerConnectionId, matchId.ToString());
                await Clients.Client(Context.ConnectionId).JoinMatch(matchDto);
            }
            catch (Exception e)
            {
                await Clients.Client(Context.ConnectionId).ErrorMessage(e.Message);
            }
        }

        [Authorize]
        public override async Task OnConnectedAsync()
        {
            var playerId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(playerId))
            {
                Context.Abort();
                return;
            }
            await _connectionTrackingService.AddConnectionAsync(playerId, Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var playerId = (Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new HubException("PlayerId not found!");
            await _connectionTrackingService.RemoveConnectionAsync(playerId, Context.ConnectionId);
            string? matchId = await _matchTrackingService.GetMatchIdFromPlayerAsync(playerId);
            if (matchId is null) return;
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