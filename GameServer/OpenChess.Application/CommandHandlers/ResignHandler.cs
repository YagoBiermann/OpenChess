using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class ResignHandler(IMatchRepository matchRepository) : IRequestHandler<ResignCommand, MatchDTO>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<MatchDTO> Handle(ResignCommand request, CancellationToken cancellationToken)
        {
            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            match.FinishWithResign(request.PlayerId);
            await _matchRepository.Update(match);
            MatchDTO matchDTO = new(match.CurrentPlayer!.TimeRemaining.ToString(), match.Fen, match.Status.ToString(), (char?)match.Winner);
            return matchDTO;
        }
    }
}