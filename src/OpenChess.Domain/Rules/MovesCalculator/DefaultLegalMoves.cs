
namespace OpenChess.Domain
{
    internal class DefaultLegalMoves : IMoveCalculatorStrategy
    {
        public List<Coordinate> Calculate(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, List<Coordinate> rangeOfAttack)
        {
            List<Coordinate> legalMoves = new(rangeOfAttack);
            if (!legalMoves.Any()) return legalMoves;

            IReadOnlySquare square = chessboard.GetReadOnlySquare(legalMoves.Last());
            int lastPosition = legalMoves.IndexOf(legalMoves.Last());
            if (piece is not Pawn && !square.HasPiece) { return legalMoves; }
            Coordinate? enPassant = chessboard.EnPassantAvailability.EnPassantPosition;
            bool isKing = square.HasTypeOfPiece(typeof(King));
            bool hasAllyOrKing = !square.HasEnemyPiece(piece.Color) || isKing;
            bool isEnPassant = square.Coordinate.Equals(enPassant);
            bool doesntHavePieceOrIsNotEnPassant = !square.HasPiece || !isEnPassant;

            if (piece is Pawn && (hasAllyOrKing || doesntHavePieceOrIsNotEnPassant)) legalMoves.RemoveAt(lastPosition);
            if (piece is not Pawn && hasAllyOrKing) legalMoves.RemoveAt(lastPosition);

            return legalMoves;
        }
    }
}