using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class TimeoutMatchHandler(IMatchRepository matchRepository) : IRequestHandler<TimeoutMatchCommand, MatchDTO>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<MatchDTO> Handle(TimeoutMatchCommand request, CancellationToken cancellationToken)
        {
            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            match.FinishWithTimeout(request.PlayerId);
            await _matchRepository.Update(match);
            MatchDTO matchDTO = new(match.CurrentPlayer!.TimeRemaining.ToString(), match.Fen, match.Status.ToString(), match.Winner?.Value);
            return matchDTO;
        }
    }
}