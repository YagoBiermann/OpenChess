
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
    {
        private List<MoveDirections> _preCalculatedMoves = new();
        private IReadOnlyChessboard _chessboard;

        public MovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            CalculateAllMoves();
        }

        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination)
        {
            List<MoveDirections> legalMoves = GetMoves(piece).Where(m => m.NearestPiece?.Color != piece.Color).ToList();

            return legalMoves.Exists(m => m.RangeOfAttack!.Contains(destination));
        }

        public bool PieceCanSolveTheCheck(IReadOnlyPiece piece)
        {
            return CalculateMovesThatSolvesTheCheck(piece).Any();
        }

        public List<MoveDirections> CalculateMovesHittingTheEnemyKing(Color player)
        {
            return _preCalculatedMoves.Where(m => m.IsHittingTheEnemyKing && m.Piece.Color == player).ToList();
        }

        public List<MoveDirections> CalculateMovesThatSolvesTheCheck(IReadOnlyPiece piece)
        {
            if (piece is King king) return CalculateKingMoves(king);
            return CalculateIntersectionWithEnemyMovesHittingTheKing(piece);
        }

        public void CalculateAllMoves()
        {
            _preCalculatedMoves.Clear();
            List<IReadOnlyPiece> pieces = _chessboard.GetAllPieces();

            foreach (var piece in pieces)
            {
                List<MoveDirections> moves = CalculateMoves(piece);
                _preCalculatedMoves.AddRange(moves);
            }
        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
        {
            if (_preCalculatedMoves.Any()) return GetMoves(piece);

            List<MoveDirections> legalMoves = new();
            List<MoveDirections> fullMoveRange = piece.CalculateMoveRange();

            foreach (MoveDirections move in fullMoveRange)
            {
                Direction currentDirection = move.Direction;
                if (move.FullRange is null) continue;

                List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(move.FullRange!);
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

        private MoveDirections CreateMoveRange(IReadOnlyPiece piece, Direction currentDirection, List<Coordinate>? rangeOfAttack = null, List<Coordinate>? fullRange = null, bool isHittingTheEnemyKing = false)
        {
            if (fullRange is null) return new(piece, currentDirection);
            if (rangeOfAttack is null) return new(piece, currentDirection, null, fullRange);

            IReadOnlyPiece nearestPiece = _chessboard.GetReadOnlySquare(rangeOfAttack.Last()).ReadOnlyPiece!;
            List<Coordinate> piecesInFullMoveRange = _chessboard.GetPiecesPosition(fullRange);
            List<CoordinateDistances> pieceDistances = CoordinateDistances.CalculateDistance(piece.Origin, piecesInFullMoveRange);
            isHittingTheEnemyKing = nearestPiece is King && nearestPiece.Color != piece.Color;
            MoveDirections moveRange = new(piece, currentDirection, fullRange, rangeOfAttack, pieceDistances, nearestPiece, isHittingTheEnemyKing);

            return moveRange;
        }

        private static List<Coordinate> RangeOfAttack(IReadOnlyPiece piece, List<Coordinate> piecesPosition, MoveDirections move)
        {
            if (!piecesPosition.Any()) return new(move.FullRange!);
            List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(piece.Origin, piecesPosition);
            CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);
            List<Coordinate> rangeOfAttack = move.FullRange!.Take(nearestPiece.DistanceBetween).ToList();

            return rangeOfAttack;
        }

        private bool SpecialPawnRuleApplies(MoveDirections move, Pawn pawn, List<Coordinate> piecesPosition, bool lastPositionIsEmpty)
        {
            bool isNotEnPassantPosition = !move.FullRange!.Contains(_chessboard.EnPassantAvailability.EnPassantPosition!);
            bool isForwardMove = move.Direction.Equals(pawn.ForwardDirection);
            bool isEmptyDiagonal = !piecesPosition.Any() && !isForwardMove;
            bool isForwardMoveAndHasPiece = !lastPositionIsEmpty && isForwardMove;

            return isForwardMoveAndHasPiece || (isEmptyDiagonal && isNotEnPassantPosition);
        }

        private List<MoveDirections> CalculateIntersectionWithEnemyMovesHittingTheKing(IReadOnlyPiece piece)
        {
            if (piece is King) throw new ChessboardException("This method cannot handle king moves");
            List<Coordinate> moveTowardsTheKing = CalculateMoveTowardsTheKing(piece);
            List<MoveDirections> legalMoves = GetMoves(piece);

            return legalMoves.FindAll(m => m.RangeOfAttack!.Intersect(moveTowardsTheKing).Any());
        }

        private List<MoveDirections> CalculateKingMoves(King king)
        {
            List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(ColorUtils.GetOppositeColor(king.Color));
            List<Coordinate> positionsNotAllowedForTheKing = CalculatePositionsNotAllowedForTheKing(piecesPosition);

            List<MoveDirections> kingMoves = GetMoves(king);
            kingMoves.RemoveAll(m => m.RangeOfAttack is null);
            bool kingMovesHittenByEnemyPiece(MoveDirections k) => positionsNotAllowedForTheKing.Intersect(k.RangeOfAttack!).Any();
            kingMoves.RemoveAll(kingMovesHittenByEnemyPiece);

            return kingMoves;
        }

        private List<Coordinate> CalculatePositionsNotAllowedForTheKing(List<Coordinate> piecesPosition)
        {
            List<List<MoveDirections>> allMoves = new();
            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                List<MoveDirections> moves = GetMoves(piece);
                if (piece is Pawn pawn)
                {
                    moves.RemoveAll(m => m.Direction.Equals(pawn.ForwardDirection));
                    allMoves.Add(moves);
                    continue;
                }

                var movesHittingTheEnemyKing = moves.Where(m => m.IsHittingTheEnemyKing && m.Piece.IsLongRange).ToList();
                if (!movesHittingTheEnemyKing.Any())
                {
                    allMoves.Add(moves);
                    continue;
                };

                moves.FindAll(m => m.IsHittingTheEnemyKing && m.Piece.IsLongRange).ForEach(m =>
                {
                    Coordinate? positionBehindTheKing = Coordinate.CalculateNextPosition(m.Piece.Origin, m.Direction);
                    if (positionBehindTheKing is null) return;
                    m.RangeOfAttack!.Add(positionBehindTheKing);
                });
            }
            List<Coordinate> positionsNotAllowedToMove = allMoves.SelectMany(m => m.SelectMany(c => c.RangeOfAttack!)).ToList();

            return positionsNotAllowedToMove;
        }

        public List<Coordinate> CalculateMoveTowardsTheKing(IReadOnlyPiece piece)
        {
            List<MoveDirections> moves = GetMoves(piece);
            List<Coordinate> movesTowardsTheKing = new();

            foreach (MoveDirections move in moves)
            {
                if (move.RangeOfAttack is null) continue;
                if (move.IsHittingTheEnemyKing)
                {
                    Coordinate kingPosition = move.NearestPiece!.Origin;
                    movesTowardsTheKing.Add(piece.Origin);
                    movesTowardsTheKing.AddRange(move.RangeOfAttack);
                    movesTowardsTheKing.Remove(kingPosition);
                    break;
                }
            }

            return movesTowardsTheKing;
        }

        private List<MoveDirections> GetMoves(IReadOnlyPiece piece)
        {
            return _preCalculatedMoves.Where(m => m.Piece == piece).ToList();
        }
    }
}