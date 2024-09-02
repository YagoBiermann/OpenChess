using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class JoinMatchHandler(IMatchRepository matchRepository) : IRequestHandler<JoinMatchCommand, MatchInfo>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<MatchInfo> Handle(JoinMatchCommand request, CancellationToken cancellationToken)
        {
            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            match.Join(request.PlayerId, request.PlayerColor);
            await _matchRepository.Update(match);

            return updatedMatchInfo;
        }
    }
}