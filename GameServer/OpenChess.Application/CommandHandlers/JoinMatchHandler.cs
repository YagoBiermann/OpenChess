using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class JoinMatchHandler(IMatchRepository matchRepository) : IRequestHandler<JoinMatchCommand, MatchInfo>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<MatchInfo> Handle(JoinMatchCommand request, CancellationToken cancellationToken)
        {
            MatchInfo matchInfo = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            Match match = new(matchInfo);
            match.Join(request.PlayerId, request.PlayerColor);
            MatchInfo updatedMatchInfo = match.ToInfo();
            await _matchRepository.Update(updatedMatchInfo);

            return updatedMatchInfo;
        }
    }
}