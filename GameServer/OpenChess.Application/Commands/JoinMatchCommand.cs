using MediatR;
namespace OpenChess.Application
{
    public record struct JoinMatchCommand(string MatchId, string PlayerId) : IRequest<JoinMatchDTO>;
}