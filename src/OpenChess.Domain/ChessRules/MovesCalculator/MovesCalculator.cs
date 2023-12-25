
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculatorStrategy _strategy;
        public MovesCalculator(IReadOnlyChessboard chessboard, IMoveCalculatorStrategy? strategy = null)
        {
            _chessboard = chessboard;
            if (strategy is null) { _strategy = new IncludeAllPiecesStrategy(); }
            _strategy = strategy!;
        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
        {
            List<MoveDirections> legalMoves = new();
            List<MoveDirections> moveRange = piece.CalculateMoveRange();

            foreach (MoveDirections move in moveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(move.Coordinates);
                if (!move.Coordinates.Any()) { legalMoves.Add(new(move.Direction, new(), piece)); continue; }
                List<Coordinate> rangeOfAttack = RangeOfAttack(piece, piecesPosition, move);
                bool lastPositionIsEmpty = !_chessboard.GetReadOnlySquare(rangeOfAttack.Last()).HasPiece;
                if (piece is not Pawn && lastPositionIsEmpty) { legalMoves.Add(new(move.Direction, rangeOfAttack, piece)); continue; }

                if (piece is Pawn pawn && SpecialPawnRuleApplies(move, pawn, piecesPosition, lastPositionIsEmpty))
                {
                    rangeOfAttack.Remove(rangeOfAttack.Last());
                    legalMoves.Add(new(currentDirection, rangeOfAttack, piece));
                    continue;
                }
                List<Coordinate> newRangeOfAttack = IncludeOrRemovePieceAtLastPosition(rangeOfAttack, piece);
                legalMoves.Add(new(currentDirection, newRangeOfAttack, piece));
            }

            return legalMoves;
        }

        private List<Coordinate> IncludeOrRemovePieceAtLastPosition(List<Coordinate> rangeOfAttack, IReadOnlyPiece piece)
        {
            List<Coordinate> newRangeOfAttack = new(rangeOfAttack);
            IReadOnlyPiece? pieceAtLastPosition = _chessboard.GetReadOnlySquare(rangeOfAttack.Last()).ReadOnlyPiece;
            if (pieceAtLastPosition is null) return newRangeOfAttack;
            bool shouldIncludePiece = _strategy.ShouldIncludePiece(piece.Color, pieceAtLastPosition);
            if (!shouldIncludePiece)
            {
                newRangeOfAttack.Remove(newRangeOfAttack.Last());
            }

            return newRangeOfAttack;
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