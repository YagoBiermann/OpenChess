using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class TimeoutMatchHandler(IMatchRepository matchRepository) : IRequestHandler<TimeoutMatchCommand>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task Handle(TimeoutMatchCommand request, CancellationToken cancellationToken)
        {
            MatchInfo matchInfo = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            Match match = new(matchInfo);
            match.FinishWithTimeout(request.PlayerId);
            MatchInfo updatedMatchInfo = match.ToInfo();
            await _matchRepository.Update(updatedMatchInfo);
        }
    }
}