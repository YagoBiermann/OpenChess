
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
    {
        private IReadOnlyChessboard _chessboard;

        public MovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
        {
            List<MoveDirections> legalMoves = new();
            List<MoveDirections> fullMoveRange = piece.CalculateMoveRange();

            foreach (MoveDirections move in fullMoveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(move.Coordinates);
                if (move.FullRange is null)
                {
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }
                List<Coordinate> rangeOfAttack = RangeOfAttack(piece, piecesPosition, move);
                bool lastPositionIsEmpty = !_chessboard.GetReadOnlySquare(rangeOfAttack.Last()).HasPiece;
                if (piece is not Pawn && lastPositionIsEmpty)
                {
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }

                if (piece is Pawn pawn && SpecialPawnRuleApplies(move, pawn, piecesPosition, lastPositionIsEmpty))
                {
                    rangeOfAttack.Remove(rangeOfAttack.Last());
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }


                legalMoves.Add(moveRange);
            }

            return legalMoves;
        }

        {

        }

        private static List<Coordinate> RangeOfAttack(IReadOnlyPiece piece, List<Coordinate> piecesPosition, MoveDirections move)
        {
            if (!piecesPosition.Any()) return new(move.Coordinates);
            List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(piece.Origin, piecesPosition);
            CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);
            List<Coordinate> rangeOfAttack = move.Coordinates.Take(nearestPiece.DistanceBetween).ToList();

            return rangeOfAttack;
        }

        private bool SpecialPawnRuleApplies(MoveDirections move, Pawn pawn, List<Coordinate> piecesPosition, bool lastPositionIsEmpty)
        {
            bool isNotEnPassantPosition = !move.Coordinates.Contains(_chessboard.EnPassantAvailability.EnPassantPosition!);
            bool isForwardMove = move.Direction.Equals(pawn.ForwardDirection);
            bool isEmptyDiagonal = !piecesPosition.Any() && !isForwardMove;
            bool isForwardMoveAndHasPiece = !lastPositionIsEmpty && isForwardMove;

            return isForwardMoveAndHasPiece || (isEmptyDiagonal && isNotEnPassantPosition);
        }
    }
}