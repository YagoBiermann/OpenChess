using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class ResignHandler(IMatchRepository matchRepository) : IRequestHandler<ResignCommand>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task Handle(ResignCommand request, CancellationToken cancellationToken)
        {
            MatchInfo matchInfo = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            Match match = new(matchInfo);
            match.FinishWithResign(request.PlayerId);
            MatchInfo updatedMatchInfo = match.ToInfo();
            await _matchRepository.Update(updatedMatchInfo);
        }
    }
}