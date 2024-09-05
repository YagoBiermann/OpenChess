using MediatR;

namespace OpenChess.Application
{
    public record struct PlayMoveCommand(string MatchId, string PlayerId, string Origin, string Destination, string? Promoting = null) : IRequest<MatchDTO>;
}