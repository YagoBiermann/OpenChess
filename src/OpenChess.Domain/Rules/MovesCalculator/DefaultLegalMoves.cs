
namespace OpenChess.Domain
{
    internal class DefaultLegalMoves : IMoveCalculatorStrategy
    {
        public List<Coordinate> Calculate(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, List<Coordinate> rangeOfAttack)
        {
            List<Coordinate> legalMoves = new(rangeOfAttack);
            if (!legalMoves.Any()) return legalMoves;

            IReadOnlySquare square = chessboard.GetReadOnlySquare(legalMoves.Last());
            if (!square.HasPiece) { return legalMoves; }
            bool isKing = square.HasTypeOfPiece(typeof(King));

            int lastPosition = legalMoves.IndexOf(legalMoves.Last());
            if (!square.HasEnemyPiece(piece.Color) || isKing) legalMoves.RemoveAt(lastPosition);

            return legalMoves;
        }
    }
}