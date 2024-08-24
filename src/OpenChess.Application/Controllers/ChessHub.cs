using Microsoft.AspNetCore.SignalR;
namespace OpenChess.Application
{
    class ChessHub(IMediator mediator, ConnectionTrackingService connectionTrackingService, MatchTrackingService matchTrackingService) : Hub<IPlayers>
    {
        private readonly IMediator _mediator = mediator;
        private readonly ConnectionTrackingService _connectionTrackingService = connectionTrackingService;
        private readonly MatchTrackingService _matchTrackingService = matchTrackingService;
    }

}