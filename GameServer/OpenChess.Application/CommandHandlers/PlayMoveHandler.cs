using MediatR;
using OpenChess.Domain;
namespace OpenChess.Application
{
    internal class PlayMoveHandler(IMatchRepository matchRepository, MoveTrackingService moveTrackingService) : IRequestHandler<PlayMoveCommand, MatchDTO?>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;
        private readonly MoveTrackingService _moveTrackingService = moveTrackingService;

        public async Task<MatchDTO?> Handle(PlayMoveCommand request, CancellationToken cancellationToken)
        {
            Guid moveId = GenerateMoveId(request);
            string? storedMoveId = await _moveTrackingService.GetMoveAsync(request.MatchId, moveId.ToString());
            if (storedMoveId is not null) { return; };

            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            _ = Guid.TryParse(request.PlayerId, out Guid playerId);
            Move move = new(playerId, Coordinate.GetInstance(request.Origin), Coordinate.GetInstance(request.Destination), request.Promoting);
            match.Play(move);
            await _matchRepository.Update(match);
            await _moveTrackingService.AddMoveAsync(request.MatchId, moveId.ToString());
        }

        private static Guid GenerateMoveId(PlayMoveCommand request)
        {
            var data = $"{request.MatchId}:{request.PlayerId}:{request.Origin}:{request.Destination}:{request.Promoting ?? string.Empty}";
            var hashBytes = System.Security.Cryptography.SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(data));
            var guidBytes = new byte[16];
            Array.Copy(hashBytes, guidBytes, 16);

            return new Guid(guidBytes);
        }
    }
}