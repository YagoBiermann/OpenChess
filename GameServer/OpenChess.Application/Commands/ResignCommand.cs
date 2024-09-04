using MediatR;

namespace OpenChess.Application
{
    public record struct ResignCommand(string MatchId, string PlayerId) : IRequest<EndGameDTO>;
}