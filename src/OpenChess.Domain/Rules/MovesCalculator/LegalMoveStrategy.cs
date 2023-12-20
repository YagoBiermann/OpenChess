
namespace OpenChess.Domain
{
    internal class LegalMoveStrategy : IMoveCalculatorStrategy
    {
        public List<Coordinate> Calculate(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, MoveDirections move)
        {
            List<Coordinate> legalMoves = new(move.Coordinates);
            if (!legalMoves.Any()) return legalMoves;

            IReadOnlySquare square = chessboard.GetReadOnlySquare(legalMoves.Last());
            int lastPosition = legalMoves.IndexOf(legalMoves.Last());
            if (piece is not Pawn && !square.HasPiece) { return legalMoves; }
            Coordinate? enPassant = chessboard.EnPassantAvailability.EnPassantPosition;
            bool squareHasAllyPiece = !square.HasEnemyPiece(piece.Color);
            bool isEnPassant = square.Coordinate.Equals(enPassant);
            bool squareIsEmptyAndIsNotEnPassant = !square.HasPiece && !isEnPassant;
            bool isForwardMove = piece is Pawn pawn && move.Direction.Equals(pawn.ForwardDirection);

            if (piece is Pawn && (squareHasAllyPiece || squareIsEmptyAndIsNotEnPassant || isForwardMove)) legalMoves.RemoveAt(lastPosition);
            if (piece is not Pawn && squareHasAllyPiece) legalMoves.RemoveAt(lastPosition);

            return legalMoves;
        }
    }
}