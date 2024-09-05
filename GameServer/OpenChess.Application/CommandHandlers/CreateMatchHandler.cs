using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    internal class CreateMatchHandler : IRequestHandler<CreateMatchCommand, string>
    {
        private readonly IMatchRepository _matchRepository;
        public CreateMatchHandler(IMatchRepository matchRepository) { _matchRepository = matchRepository; }
        public async Task<string> Handle(CreateMatchCommand request, CancellationToken cancellationToken)
        {
            Match match = new(request.Time);
            await _matchRepository.Create(match);

            return match.Id.ToString();
        }
    }
}