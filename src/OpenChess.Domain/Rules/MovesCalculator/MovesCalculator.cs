
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
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
                if (piece is Pawn pawn)
                {
                    bool isEnPassantPosition = move.Coordinates.Contains(_chessboard.EnPassantAvailability.EnPassantPosition!);
                    bool isForwardMove = move.Direction.Equals(pawn.ForwardDirection);
                    if (isEnPassantPosition) { legalMoves.Add(move); continue; }
                    if (!pieces.Any() && !isForwardMove) { legalMoves.Add(new(move.Direction, new())); continue; }
                }
                if (!pieces.Any()) { legalMoves.Add(move); continue; }

                List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(piece.Origin, pieces);
                CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);
                List<Coordinate> rangeOfAttack = move.Coordinates.Take(nearestPiece.DistanceBetween).ToList();
                List<Coordinate> newRangeOfAttack = _strategy.Calculate(_chessboard, piece, new(move.Direction, rangeOfAttack));
                legalMoves.Add(new(currentDirection, newRangeOfAttack));
            }

            return legalMoves;
        }
    }
}