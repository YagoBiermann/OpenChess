using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class CreateMatchHandler : IRequestHandler<CreateMatchCommand, MatchDTO>
    {
        private readonly IMatchRepository _matchRepository;
        public CreateMatchHandler(IMatchRepository matchRepository) { _matchRepository = matchRepository; }
        public async Task<MatchDTO> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            Match match = new(request.Time);
            MatchInfo matchInfo = match.ToInfo();
            await _matchRepository.Create(matchInfo);

            return matchInfo;
        }
    }
}