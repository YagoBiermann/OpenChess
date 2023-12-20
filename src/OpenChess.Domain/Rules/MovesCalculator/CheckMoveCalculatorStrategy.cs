
namespace OpenChess.Domain
{
    internal class CheckMoveCalculatorStrategy : IMoveCalculatorStrategy
    {
        public List<Coordinate> Calculate(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, MoveDirections move)
        {
            List<Coordinate> moves = new(move.Coordinates);
            if (!moves.Any()) return moves;
            bool isPawnForwardDirection = piece is Pawn pawn && move.Direction.Equals(pawn.ForwardDirection);

            IReadOnlySquare square = chessboard.GetReadOnlySquare(moves.Last());
            if (!square.HasPiece) { return moves; }

            int lastPosition = moves.IndexOf(moves.Last());
            if (!square.HasEnemyPiece(piece.Color) || isPawnForwardDirection) moves.RemoveAt(lastPosition);

            return moves;
        }
    }
}