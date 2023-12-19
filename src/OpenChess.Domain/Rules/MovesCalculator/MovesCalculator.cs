
namespace OpenChess.Domain
{
    internal class MovesCalculator
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculatorStrategy _strategy;
        public MovesCalculator(IReadOnlyChessboard chessboard, IMoveCalculatorStrategy strategy)
        {
            _chessboard = chessboard;
            _strategy = strategy;
        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
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
                legalMoves.Add(new(currentDirection, rangeOfAttack));
            }

            return legalMoves;
        }
    }
}