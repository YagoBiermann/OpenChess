using MediatR;
using OpenChess.Domain;
namespace OpenChess.Application
{
    public record struct JoinMatchCommand(string MatchId, string PlayerId) : IRequest<MatchDTO>;
}