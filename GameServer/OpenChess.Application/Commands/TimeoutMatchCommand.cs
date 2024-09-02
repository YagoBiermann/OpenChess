using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    public record struct TimeoutMatchCommand(string MatchId, string? PlayerId = null) : IRequest<MatchDTO>;
}