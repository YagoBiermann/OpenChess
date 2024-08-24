using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    public record struct CreateMatchCommand(int Time) : IRequest<MatchInfo>;
}