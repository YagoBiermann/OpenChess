using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class JoinMatchHandler(IMatchRepository matchRepository) : IRequestHandler<JoinMatchCommand, MatchDTO>
    {
        private readonly IMatchRepository _matchRepository = matchRepository;

        public async Task<MatchDTO> Handle(JoinMatchCommand request, CancellationToken cancellationToken)
        {
            IMatch match = await _matchRepository.GetById(request.MatchId) ?? throw new MatchException("Match not found!");
            match.Join(request.PlayerId);
            await _matchRepository.Update(match);

            return MatchDTOMapper.ToDTO(match);
        }
    }
}