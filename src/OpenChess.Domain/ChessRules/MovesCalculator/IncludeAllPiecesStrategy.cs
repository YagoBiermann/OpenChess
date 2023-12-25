
namespace OpenChess.Domain
{
    internal class IncludeAllPiecesStrategy : IMoveCalculatorStrategy
    {
        public bool ShouldIncludePiece(Color player, IReadOnlyPiece pieceAtLastPosition)
        {
            return true;
        }
    }
}