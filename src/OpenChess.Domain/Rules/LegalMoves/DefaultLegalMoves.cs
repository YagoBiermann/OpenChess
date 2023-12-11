
namespace OpenChess.Domain
{
    internal class DefaultLegalMoves : ILegalMoves
    {
        private IReadOnlyChessboard _chessboard;
        public DefaultLegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            List<MoveDirections> legalMoves = new();
            List<MoveDirections> moveRange = piece.CalculateMoveRange();

            foreach (MoveDirections move in moveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> pieces = _chessboard.GetPiecesPosition(move.Coordinates);
                if (!pieces.Any())
                {
                    legalMoves.Add(move);
                    continue;
                }

                List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(piece.Origin, pieces);
                CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);

                List<Coordinate> rangeOfAttack = move.Coordinates.Take(nearestPiece.DistanceBetween).ToList();
                IReadOnlySquare square = _chessboard.GetReadOnlySquare(nearestPiece.Position);
                bool isKing = square.HasTypeOfPiece(typeof(King));

                int lastPosition = rangeOfAttack.Count - 1;
                if (!square.HasEnemyPiece(piece.Color) || isKing) rangeOfAttack.RemoveAt(lastPosition);

                legalMoves.Add(new(currentDirection, rangeOfAttack));
            }

            return legalMoves;
        }
    }
}