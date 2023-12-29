
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
    {
        private List<PieceRangeOfAttack> _preCalculatedMoves = new();
        private IReadOnlyChessboard _chessboard;

        public MovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            CalculateAndCacheAllMoves();
        }

        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination)
        {
            List<PieceRangeOfAttack> legalMoves = GetMoves(piece).Where(m => m.NearestPiece?.Color != piece.Color || m.NearestPiece is null).ToList();

            return legalMoves.Exists(m => m.RangeOfAttack!.Contains(destination));
        }

        public bool PieceCanSolveTheCheck(IReadOnlyPiece piece)
        {
            return CalculateMovesThatSolvesTheCheck(piece).Any();
        }

        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            return GetMoves(piece).Where(m => m.IsHittingTheEnemyKing).ToList().Any();
        }

        public List<PieceRangeOfAttack> CalculateMovesHittingTheEnemyKing(Color player)
        {
            return _preCalculatedMoves.Where(m => m.IsHittingTheEnemyKing && m.Piece.Color == player).ToList();
        }

        public List<PieceRangeOfAttack> CalculateMovesThatSolvesTheCheck(IReadOnlyPiece piece)
        {
            if (piece is King king) return CalculateKingMoves(king);
            return CalculateIntersectionWithEnemyMovesHittingTheKing(piece);
        }

        public void CalculateAndCacheAllMoves()
        {
            _preCalculatedMoves.Clear();
            List<IReadOnlyPiece> pieces = _chessboard.GetAllPieces();

            foreach (var piece in pieces)
            {
                List<PieceRangeOfAttack> moves = CalculateMoves(piece);
                _preCalculatedMoves.AddRange(moves);
            }
        }

        public List<PieceRangeOfAttack> CalculateMoves(IReadOnlyPiece piece)
        {
            if (_preCalculatedMoves.Any()) return GetMoves(piece);

            List<PieceRangeOfAttack> legalMoves = new();
            List<PieceRangeOfAttack> fullMoveRange = piece.CalculateMoveRange();

            foreach (PieceRangeOfAttack move in fullMoveRange)
            {
                Direction currentDirection = move.Direction;
                if (move.FullRange is null) continue;

                List<IReadOnlyPiece> piecesPosition = _chessboard.GetPieces(move.FullRange!);
                List<Coordinate> rangeOfAttack = RangeOfAttack(piece, piecesPosition, move);
                bool lastPositionIsEmpty = !_chessboard.GetReadOnlySquare(rangeOfAttack.Last()).HasPiece;
                if (piece is not Pawn && lastPositionIsEmpty)
                {
                    PieceRangeOfAttack pawnMoveRange = CreateMoveRange(piece, currentDirection, move.FullRange);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }

                if (piece is Pawn pawn && SpecialPawnRuleApplies(move, pawn, piecesPosition, lastPositionIsEmpty))
                {
                    rangeOfAttack.Remove(rangeOfAttack.Last());
                    PieceRangeOfAttack pawnMoveRange = CreateMoveRange(piece, currentDirection, move.FullRange, rangeOfAttack);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }

                PieceRangeOfAttack moveRange = CreateMoveRange(piece, currentDirection, move.FullRange, rangeOfAttack);

                legalMoves.Add(moveRange);
            }

            return legalMoves;
        }

        private PieceRangeOfAttack CreateMoveRange(IReadOnlyPiece piece, Direction currentDirection, List<Coordinate>? rangeOfAttack = null, List<Coordinate>? fullRange = null, bool isHittingTheEnemyKing = false)
        {
            if (fullRange is null) return new(piece, currentDirection);
            if (rangeOfAttack is null) return new(piece, currentDirection, null, fullRange);

            IReadOnlyPiece nearestPiece = _chessboard.GetReadOnlySquare(rangeOfAttack.Last()).ReadOnlyPiece!;
            List<IReadOnlyPiece> piecesInFullMoveRange = _chessboard.GetPieces(fullRange);
            List<PieceDistances> pieceDistances = PieceDistances.CalculateDistance(piece.Origin, piecesInFullMoveRange);
            isHittingTheEnemyKing = nearestPiece is King && nearestPiece.Color != piece.Color;
            PieceRangeOfAttack moveRange = new(piece, currentDirection, fullRange, rangeOfAttack, pieceDistances, nearestPiece, isHittingTheEnemyKing);

            return moveRange;
        }

        private static List<Coordinate> RangeOfAttack(IReadOnlyPiece piece, List<IReadOnlyPiece> piecesPosition, PieceRangeOfAttack move)
        {
            if (!piecesPosition.Any()) return new(move.FullRange!);
            List<PieceDistances> distances = PieceDistances.CalculateDistance(piece.Origin, piecesPosition);
            PieceDistances nearestPiece = PieceDistances.CalculateNearestDistance(distances);
            List<Coordinate> rangeOfAttack = move.FullRange!.Take(nearestPiece.DistanceFromOrigin).ToList();

            return rangeOfAttack;
        }

        private bool SpecialPawnRuleApplies(PieceRangeOfAttack move, Pawn pawn, List<IReadOnlyPiece> pieces, bool lastPositionIsEmpty)
        {
            bool isNotEnPassantPosition = !move.FullRange!.Contains(_chessboard.EnPassantAvailability.EnPassantPosition!);
            bool isForwardMove = move.Direction.Equals(pawn.ForwardDirection);
            bool isEmptyDiagonal = !pieces.Any() && !isForwardMove;
            bool isForwardMoveAndHasPiece = !lastPositionIsEmpty && isForwardMove;

            return isForwardMoveAndHasPiece || (isEmptyDiagonal && isNotEnPassantPosition);
        }

        private List<PieceRangeOfAttack> CalculateIntersectionWithEnemyMovesHittingTheKing(IReadOnlyPiece piece)
        {
            if (piece is King) throw new ChessboardException("This method cannot handle king moves");
            List<Coordinate> moveTowardsTheKing = CalculateMoveTowardsTheKing(piece);
            List<PieceRangeOfAttack> legalMoves = GetMoves(piece);

            return legalMoves.FindAll(m => m.RangeOfAttack!.Intersect(moveTowardsTheKing).Any());
        }

        private List<PieceRangeOfAttack> CalculateKingMoves(King king)
        {
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(ColorUtils.GetOppositeColor(king.Color));
            List<Coordinate> positionsNotAllowedForTheKing = CalculatePositionsNotAllowedForTheKing(pieces);

            List<PieceRangeOfAttack> kingMoves = GetMoves(king);
            kingMoves.RemoveAll(m => m.RangeOfAttack is null);
            bool kingMovesHittenByEnemyPiece(PieceRangeOfAttack k) => positionsNotAllowedForTheKing.Intersect(k.RangeOfAttack!).Any();
            kingMoves.RemoveAll(kingMovesHittenByEnemyPiece);

            return kingMoves;
        }

        private List<Coordinate> CalculatePositionsNotAllowedForTheKing(List<IReadOnlyPiece> piecesPosition)
        {
            List<List<PieceRangeOfAttack>> allMoves = new();
            foreach (IReadOnlyPiece piece in piecesPosition)
            {
                List<PieceRangeOfAttack> moves = GetMoves(piece);
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
            List<Coordinate> positionsNotAllowedToMove = new();
            List<Coordinate> pawnDiagonalPositions = allMoves.SelectMany(m => m.FindAll(p => p.Piece is Pawn).SelectMany(m => m.FullRange!)).ToList();
            List<Coordinate> rangeOfAttackOfPiecesExceptPawn = allMoves.SelectMany(m => m.FindAll(p => p.Piece is not Pawn).SelectMany(p => p.RangeOfAttack!)).ToList();
            positionsNotAllowedToMove.AddRange(pawnDiagonalPositions);
            positionsNotAllowedToMove.AddRange(rangeOfAttackOfPiecesExceptPawn);

            return positionsNotAllowedToMove;
        }

        private List<Coordinate> CalculateMoveTowardsTheKing(IReadOnlyPiece piece)
        {
            List<PieceRangeOfAttack> moves = GetMoves(piece);
            List<Coordinate> movesTowardsTheKing = new();

            moves.FindAll(m => m.RangeOfAttack is not null && m.IsHittingTheEnemyKing).ForEach(m =>
            {
                Coordinate kingPosition = m.NearestPiece!.Origin;
                movesTowardsTheKing.Add(piece.Origin);
                movesTowardsTheKing.AddRange(m.RangeOfAttack!);
                movesTowardsTheKing.Remove(kingPosition);
            });

            return movesTowardsTheKing;
        }

        private List<PieceRangeOfAttack> GetMoves(IReadOnlyPiece piece)
        {
            return _preCalculatedMoves.Where(m => m.Piece == piece).ToList();
        }
    }
}