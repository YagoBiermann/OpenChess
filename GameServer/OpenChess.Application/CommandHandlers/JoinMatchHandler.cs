using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class JoinMatchHandler(IMatchRepository matchRepository) : IRequestHandler<JoinMatchCommand, JoinMatchDTO>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<JoinMatchDTO> Handle(JoinMatchCommand request, CancellationToken cancellationToken)
        {
            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            match.Join(request.PlayerId);
            await _matchRepository.Update(match);
            char color = (char)match.GetPlayerById(request.PlayerId)!.Color;
            var matchDto = new JoinMatchDTO(match.Id.ToString(), match.Fen, color, match.Duration);

            return matchDto;
        }
    }
}