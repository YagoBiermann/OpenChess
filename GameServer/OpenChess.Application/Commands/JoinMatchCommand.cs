using MediatR;
using OpenChess.Domain;
namespace OpenChess.Application
{
    public record struct JoinMatchCommand(string MatchId, string PlayerId, int PlayerColor = 0) : IRequest<MatchInfo>;
}