
namespace OpenChess.Domain
{
    internal class IncludeAllyPieceStrategy : IMoveCalculatorStrategy
    {
        public bool ShouldIncludePiece(Color player, IReadOnlyPiece pieceAtLastPosition)
        {
            bool includeAllyPiece = pieceAtLastPosition.Color == player;
            return includeAllyPiece;
        }
    }
}