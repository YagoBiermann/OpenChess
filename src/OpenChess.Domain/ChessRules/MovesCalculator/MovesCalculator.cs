
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
    {
        private List<PieceRangeOfAttack> _preCalculatedRangeOfAttack = new();
        private List<PieceLineOfSight> _preCalculatedLineOfSight = new();
        private IReadOnlyChessboard _chessboard;

        public MovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination)
        {
            List<PieceRangeOfAttack> legalMoves = CalculateRangeOfAttack(piece).Where(m => m.NearestPiece?.Color != piece.Color || m.NearestPiece is null).ToList();

            return legalMoves.Exists(m => m.RangeOfAttack.Contains(destination));
        }

        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            return CalculateRangeOfAttack(piece).Where(m => m.IsHittingTheEnemyKing).ToList().Any();
        }

        public void CalculateAndCacheAllMoves()
        {
            _preCalculatedRangeOfAttack.Clear();
            _preCalculatedLineOfSight.Clear();
            List<IReadOnlyPiece> pieces = _chessboard.GetAllPieces();

            foreach (var piece in pieces)
            {
                List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(piece);
                List<PieceRangeOfAttack> rangeOfAttack = CalculateRangeOfAttack(piece);
                _preCalculatedRangeOfAttack.AddRange(rangeOfAttack);
                _preCalculatedLineOfSight.AddRange(lineOfSight);
            }
        }

        public List<PieceRangeOfAttack> CalculateAllMoves()
        {
            if (!_preCalculatedRangeOfAttack.Any()) { CalculateAndCacheAllMoves(); }
            return new(_preCalculatedRangeOfAttack);
        }

        public List<PieceRangeOfAttack> CalculateKingMoves(Color player)
        {
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(ColorUtils.GetOppositeColor(player));
            List<Coordinate> positionsNotAllowedForTheKing = CalculatePositionsNotAllowedForTheKing(pieces);

            List<PieceRangeOfAttack> kingMoves = _preCalculatedRangeOfAttack.Where(m => m.Piece is King && m.Piece.Color == player).ToList();
            kingMoves.RemoveAll(m => !m.RangeOfAttack.Any());
            bool kingMovesHittenByEnemyPiece(PieceRangeOfAttack k) => positionsNotAllowedForTheKing.Intersect(k.RangeOfAttack).Any();
            kingMoves.RemoveAll(kingMovesHittenByEnemyPiece);

            return kingMoves;
        }

        public List<PieceRangeOfAttack> CalculateRangeOfAttack(IReadOnlyPiece piece)
        {
            if (_preCalculatedRangeOfAttack.Any()) return GetRangeOfAttackFromCache(piece);

            List<PieceRangeOfAttack> legalMoves = new();
            List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(piece);

            foreach (PieceLineOfSight move in lineOfSight)
            {
                Direction currentDirection = move.Direction;
                if (!move.LineOfSight.Any()) { legalMoves.Add(new(move.Piece, move.Direction, new())); continue; }

                List<IReadOnlyPiece> piecesPosition = _chessboard.GetPieces(move.LineOfSight);
                List<Coordinate> rangeOfAttack = CalculatePositionsUntilTheNearestPiece(piece, piecesPosition, move);
                bool lastPositionIsEmpty = !_chessboard.GetReadOnlySquare(rangeOfAttack.Last()).HasPiece;
                if (piece is not Pawn && lastPositionIsEmpty)
                {
                    PieceRangeOfAttack pawnMoveRange = new(piece, currentDirection, move.LineOfSight);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }

                if (piece is Pawn pawn && SpecialPawnRuleApplies(move, pawn, piecesPosition, lastPositionIsEmpty))
                {
                    rangeOfAttack.Remove(rangeOfAttack.Last());
                    PieceRangeOfAttack pawnMoveRange = new(piece, currentDirection, rangeOfAttack);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }

                IReadOnlyPiece nearestPiece = _chessboard.GetReadOnlySquare(rangeOfAttack.Last()).ReadOnlyPiece!;
                PieceRangeOfAttack moveRange = new(piece, currentDirection, rangeOfAttack, nearestPiece);

                legalMoves.Add(moveRange);
            }

            return legalMoves;
        }

        public List<PieceLineOfSight> CalculateLineOfSight(IReadOnlyPiece piece)
        {
            if (_preCalculatedRangeOfAttack.Any()) return GetLineOfSightFromCache(piece);
            List<PieceLineOfSight> lineOfSight = new();

            foreach (Direction direction in piece.Directions)
            {
                int moveAmount = piece.MoveAmount;
                if (piece is Pawn pawn && direction.Equals(pawn.ForwardDirection))
                {
                    moveAmount = pawn.ForwardMoveAmount;
                }
                List<Coordinate> positions = Coordinate.CalculateSequence(piece.Origin, direction, moveAmount);
                List<IReadOnlyPiece> piecesInLineOfSight = _chessboard.GetPieces(positions);
                lineOfSight.Add(new(piece, direction, positions, piecesInLineOfSight));
            }

            return lineOfSight;
        }

        private static List<Coordinate> CalculatePositionsUntilTheNearestPiece(IReadOnlyPiece piece, List<IReadOnlyPiece> piecesPosition, PieceLineOfSight move)
        {
            if (!piecesPosition.Any()) return new(move.LineOfSight);
            List<PieceDistances> distances = PieceDistances.CalculateDistance(piece, piecesPosition);
            PieceDistances nearestPiece = PieceDistances.CalculateNearestDistance(distances);
            List<Coordinate> rangeOfAttack = move.LineOfSight.Take(nearestPiece.DistanceFromOrigin).ToList();

            return rangeOfAttack;
        }

        private bool SpecialPawnRuleApplies(PieceLineOfSight move, Pawn pawn, List<IReadOnlyPiece> pieces, bool lastPositionIsEmpty)
        {
            bool isNotEnPassantPosition = !move.LineOfSight.Contains(_chessboard.EnPassantAvailability.EnPassantPosition!);
            bool isForwardMove = move.Direction.Equals(pawn.ForwardDirection);
            bool isEmptyDiagonal = !pieces.Any() && !isForwardMove;
            bool isForwardMoveAndHasPiece = !lastPositionIsEmpty && isForwardMove;

            return isForwardMoveAndHasPiece || (isEmptyDiagonal && isNotEnPassantPosition);
        }

        private List<Coordinate> CalculatePositionsNotAllowedForTheKing(List<IReadOnlyPiece> piecesPosition)
        {
            List<List<PieceRangeOfAttack>> allMoves = new();
            foreach (IReadOnlyPiece piece in piecesPosition)
            {
                if (piece is Pawn pawn)
                {
                    var pawnLineOfSight = CalculateLineOfSight(pawn);
                    var pawnDiagonals = pawnLineOfSight.SkipWhile(m => m.Direction.Equals(pawn.ForwardDirection)).ToList();
                    var pawnRangeOfAttack = pawnDiagonals.Select(p => new PieceRangeOfAttack(p.Piece, p.Direction, p.LineOfSight)).ToList();

                    allMoves.Add(pawnRangeOfAttack);
                    continue;
                }

                List<PieceRangeOfAttack> moves = CalculateRangeOfAttack(piece);
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
                    m.RangeOfAttack.Add(positionBehindTheKing);
                });
            }
            List<Coordinate> positionsNotAllowedToMove = allMoves.SelectMany(m => m.SelectMany(m => m.RangeOfAttack)).ToList();

            return positionsNotAllowedToMove;
        }

        private List<PieceRangeOfAttack> GetRangeOfAttackFromCache(IReadOnlyPiece piece)
        {
            return _preCalculatedRangeOfAttack.Where(m => m.Piece == piece).ToList();
        }

        private List<PieceLineOfSight> GetLineOfSightFromCache(IReadOnlyPiece piece)
        {
            return _preCalculatedLineOfSight.Where(m => m.Piece == piece).ToList();
        }
    }
}