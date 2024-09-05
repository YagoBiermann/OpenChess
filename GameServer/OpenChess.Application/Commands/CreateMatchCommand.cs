using MediatR;

namespace OpenChess.Application
{
    public record struct CreateMatchCommand(int Time) : IRequest<string>;
}