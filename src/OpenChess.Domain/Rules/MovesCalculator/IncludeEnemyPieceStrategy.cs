
namespace OpenChess.Domain
{
    internal class IncludeEnemyPieceStrategy : IMoveCalculatorStrategy
    {
        public bool ShouldIncludePiece(Color player, IReadOnlyPiece pieceAtLastPosition)
        {
            bool dontIncludeAllyPiece = !(pieceAtLastPosition.Color == player);
            return dontIncludeAllyPiece;
        }
    }
}