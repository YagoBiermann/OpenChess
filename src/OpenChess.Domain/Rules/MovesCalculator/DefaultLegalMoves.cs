
namespace OpenChess.Domain
{
    internal class DefaultLegalMoves : IMoveCalculatorStrategy
    {
        public List<Coordinate> Calculate(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, MoveDirections move)
        {
            List<Coordinate> legalMoves = new(move.Coordinates);
            if (!legalMoves.Any()) return legalMoves;

            IReadOnlySquare square = chessboard.GetReadOnlySquare(legalMoves.Last());
            int lastPosition = legalMoves.IndexOf(legalMoves.Last());
            if (piece is not Pawn && !square.HasPiece) { return legalMoves; }
            Coordinate? enPassant = chessboard.EnPassantAvailability.EnPassantPosition;
            bool isKing = square.HasTypeOfPiece(typeof(King));
            bool hasAllyOrKing = !square.HasEnemyPiece(piece.Color) || isKing;
            bool isEnPassant = square.Coordinate.Equals(enPassant);
            bool isEmptyAndIsNotEnPassant = !square.HasPiece && !isEnPassant;
            bool isForwardMove = piece is Pawn pawn && move.Direction.Equals(pawn.ForwardDirection);

            if (piece is Pawn && (hasAllyOrKing || isEmptyAndIsNotEnPassant || isForwardMove)) legalMoves.RemoveAt(lastPosition);
            if (piece is not Pawn && hasAllyOrKing) legalMoves.RemoveAt(lastPosition);

            return legalMoves;
        }
    }
}