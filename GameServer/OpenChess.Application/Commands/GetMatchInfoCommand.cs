using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    public record struct GetMatchCommand(string MatchId) : IRequest<MatchInfo>;
}