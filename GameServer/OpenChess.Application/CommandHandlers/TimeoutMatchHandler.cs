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
            MatchInfo updatedMatchInfo = match.ToInfo();
            await _matchRepository.Update(updatedMatchInfo);
        }
    }
}