
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
    {
        private List<MoveDirections> _whitePlayerMoves = new();
        private List<MoveDirections> _blackPlayerMoves = new();
        private IReadOnlyChessboard _chessboard;

        public MovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            CalculateAllMoves();
        }

        public void CalculateAllMoves()
        {
            _whitePlayerMoves.Clear();
            _blackPlayerMoves.Clear();
            List<Coordinate> whitePieces = _chessboard.GetPiecesPosition(Color.White);
            List<Coordinate> blackPieces = _chessboard.GetPiecesPosition(Color.Black);

            foreach (var position in whitePieces)
            {
                var piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;

                List<MoveDirections> moves = CalculateMoves(piece);
                _whitePlayerMoves.AddRange(moves);
            }

            foreach (var position in blackPieces)
            {
                var piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;

                List<MoveDirections> moves = CalculateMoves(piece);
                _whitePlayerMoves.AddRange(moves);
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
                    MoveDirections pawnMoveRange = CreateMoveRange(piece, currentDirection);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }
                List<Coordinate> rangeOfAttack = RangeOfAttack(piece, piecesPosition, move);
                bool lastPositionIsEmpty = !_chessboard.GetReadOnlySquare(rangeOfAttack.Last()).HasPiece;
                if (piece is not Pawn && lastPositionIsEmpty)
                {
                    MoveDirections pawnMoveRange = CreateMoveRange(piece, currentDirection, move.FullRange);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }

                if (piece is Pawn pawn && SpecialPawnRuleApplies(move, pawn, piecesPosition, lastPositionIsEmpty))
                {
                    rangeOfAttack.Remove(rangeOfAttack.Last());
                    MoveDirections pawnMoveRange = CreateMoveRange(piece, currentDirection, move.FullRange, rangeOfAttack);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }

                MoveDirections moveRange = CreateMoveRange(piece, currentDirection, move.FullRange, rangeOfAttack);

                legalMoves.Add(moveRange);
            }

            return legalMoves;
        }

        private MoveDirections CreateMoveRange(IReadOnlyPiece piece, Direction currentDirection, List<Coordinate>? rangeOfAttack = null, List<Coordinate>? fullRange = null)
        {
            if (fullRange is null) return new(piece, currentDirection);
            if (rangeOfAttack is null) return new(piece, currentDirection, null, fullRange);

            IReadOnlyPiece nearestPiece = _chessboard.GetReadOnlySquare(rangeOfAttack.Last()).ReadOnlyPiece!;
            List<Coordinate> piecesInFullMoveRange = _chessboard.GetPiecesPosition(fullRange);
            List<CoordinateDistances> pieceDistances = CoordinateDistances.CalculateDistance(piece.Origin, piecesInFullMoveRange);
            MoveDirections moveRange = new(piece, currentDirection, fullRange, rangeOfAttack, pieceDistances, nearestPiece);

            return moveRange;
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