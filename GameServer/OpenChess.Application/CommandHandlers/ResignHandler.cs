using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class ResignHandler(IMatchRepository matchRepository) : IRequestHandler<ResignCommand, ResignDTO>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<ResignDTO> Handle(ResignCommand request, CancellationToken cancellationToken)
        {
            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            match.FinishWithResign(request.PlayerId);
            await _matchRepository.Update(match);

            return new((char)match.Winner!.Value, match.Status.ToString());
        }
    }
}