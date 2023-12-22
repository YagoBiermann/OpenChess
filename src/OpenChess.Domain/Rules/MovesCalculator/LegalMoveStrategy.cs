
namespace OpenChess.Domain
{
    internal class LegalMoveStrategy : IMoveCalculatorStrategy
    {
        public bool ShouldIncludePiece(Color player, IReadOnlyPiece pieceAtLastPosition)
        {
            bool dontIncludeAllyPiece = !(pieceAtLastPosition.Color == player);

            return dontIncludeAllyPiece;
        }
    }
}