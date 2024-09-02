using MediatR;
using OpenChess.Domain;

namespace OpenChess.Application
{
    public record struct ResignCommand(string MatchId, string PlayerId) : IRequest<MatchDTO>;
}