using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class TimeoutMatchHandler(IMatchRepository matchRepository) : IRequestHandler<TimeoutMatchCommand, EndGameDTO>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<EndGameDTO> Handle(TimeoutMatchCommand request, CancellationToken cancellationToken)
        {
            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            match.FinishWithTimeout(request.PlayerId);
            await _matchRepository.Update(match);

            return new((char)match.Winner!.Value, match.Status.ToString());
        }
    }
}