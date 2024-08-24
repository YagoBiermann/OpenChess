using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class GetMatchHandler(IMatchRepository matchRepository) : IRequestHandler<GetMatchCommand, MatchInfo>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<MatchInfo> Handle(GetMatchCommand request, CancellationToken cancellationToken)
        {
            return await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
        }
    }
}