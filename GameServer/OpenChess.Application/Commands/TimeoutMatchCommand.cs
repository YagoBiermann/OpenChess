using MediatR;

namespace OpenChess.Application
{
    public record struct TimeoutMatchCommand(string MatchId, string? PlayerId = null) : IRequest<EndGameDTO>;
}