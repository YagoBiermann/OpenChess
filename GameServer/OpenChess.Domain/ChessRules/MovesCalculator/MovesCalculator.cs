
namespace OpenChess.Domain
{
    internal class MovesCalculator : IMoveCalculator
    {
        private List<PieceAttackRange> _preCalculatedRangeOfAttack = new();
        private List<PieceLineOfSight> _preCalculatedLineOfSight = new();
        private List<PieceLineOfSight> _preCalculatedPinMoves = new();

        private IReadOnlyChessboard _chessboard;

        public MovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination)
        {
            List<PieceAttackRange> legalMoves = CalculateLegalMoves(piece);
            return legalMoves.SelectMany(m => m.AttackRange).Contains(destination);
        }

        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            return CalculateRangeOfAttack(piece).Where(m => m.IsHittingTheEnemyKing).ToList().Any();
        }

        public bool IsPinned(IReadOnlyPiece piece, out bool canMove)
        {
            canMove = false;
            if (piece is King) return false;
            var allEnemyMovesPinningAPiece = CalculatePinMovesFromPlayer(Color.GetOppositeColor(piece.Color));
            if (!allEnemyMovesPinningAPiece.Any()) return false;
            var enemyMovePinningCurrentPiece = allEnemyMovesPinningAPiece.Where(m => m.PiecesInLineOfSight.First().Piece.Equals(piece)).ToList();
            bool isPinned = enemyMovePinningCurrentPiece.Any();
            if (!isPinned) return false;
            var canMoveTowardsTheEnemyPiece = CanMoveWhenPinnedByEnemyPiece(piece, enemyMovePinningCurrentPiece.First());
            canMove = canMoveTowardsTheEnemyPiece;

            return isPinned;
        }

        public void CalculateAndCacheAllMoves()
        {
            ClearCache();
            List<IReadOnlyPiece> pieces = _chessboard.GetAllPieces();
            List<PieceAttackRange> allRangeOfAttack = new();
            List<PieceLineOfSight> allLineOfSight = new();
            List<PieceLineOfSight> allPinMoves = new();

            foreach (var piece in pieces)
            {
                List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(piece);
                List<PieceAttackRange> rangeOfAttack = CalculateRangeOfAttack(piece);
                allRangeOfAttack.AddRange(rangeOfAttack);
                allLineOfSight.AddRange(lineOfSight);
            }

            _preCalculatedRangeOfAttack.AddRange(allRangeOfAttack);
            _preCalculatedLineOfSight.AddRange(allLineOfSight);

            allPinMoves.AddRange(CalculatePinMovesFromPlayer(new(Color.White)));
            allPinMoves.AddRange(CalculatePinMovesFromPlayer(new(Color.Black)));
            _preCalculatedPinMoves.AddRange(allPinMoves);
        }

        public void ClearCache()
        {
            _preCalculatedLineOfSight.Clear();
            _preCalculatedRangeOfAttack.Clear();
            _preCalculatedPinMoves.Clear();
        }

        public List<PieceAttackRange> CalculateAllMoves()
        {
            if (!_preCalculatedRangeOfAttack.Any()) { CalculateAndCacheAllMoves(); }
            return new(_preCalculatedRangeOfAttack);
        }

        public List<PieceAttackRange> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            if (piece is Pawn pawn) { return CalculatePawnMoves(pawn); }
            List<PieceAttackRange> legalMoves = CalculateRangeOfAttack(piece);
            legalMoves.Where(m => m.NearestPiece?.Color == piece.Color).ToList().ForEach(m =>
            {
                m.AttackRange.Remove(m.AttackRange.Last()); // Remove ally piece position
            });

            return legalMoves;
        }

        public List<PieceAttackRange> CalculatePawnMoves(Pawn pawn)
        {
            List<PieceAttackRange> legalMoves = new();
            List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(pawn);
            foreach (PieceLineOfSight move in lineOfSight)
            {
                if (!move.LineOfSight.Any()) { legalMoves.Add(new(move.Piece, move.Direction, move.LineOfSight)); continue; }
                List<IReadOnlyPiece> piecesPosition = _chessboard.GetPieces(move.LineOfSight);
                List<Coordinate> rangeOfAttack = CalculatePositionsUntilTheNearestPiece(pawn, piecesPosition, move);
                bool lastPositionIsEmpty = _chessboard.GetPiece(rangeOfAttack.Last()) is null;
                if (SpecialPawnRuleApplies(move, pawn, piecesPosition, lastPositionIsEmpty))
                {
                    rangeOfAttack.Remove(rangeOfAttack.Last());
                    PieceAttackRange pawnMoves = new(pawn, move.Direction, rangeOfAttack);
                    legalMoves.Add(pawnMoves);
                    continue;
                }

                IReadOnlyPiece nearestPiece = _chessboard.GetPiece(rangeOfAttack.Last())!;
                legalMoves.Add(new(pawn, move.Direction, rangeOfAttack, nearestPiece));
            }

            return legalMoves;
        }

        public List<PieceAttackRange> CalculateKingMoves(Color player)
        {
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(Color.GetOppositeColor(player));
            List<Coordinate> positionsNotAllowedForTheKing = CalculatePositionsNotAllowedForTheKing(pieces);

            IReadOnlyPiece king = _chessboard.GetPieces(player).Find(p => p is King)!;
            List<PieceAttackRange> kingMoves = CalculateLegalMoves(king);
            bool kingMovesNotHittenByEnemyPiece(PieceAttackRange k) => k.AttackRange.Except(positionsNotAllowedForTheKing).Any();

            return kingMoves.FindAll(kingMovesNotHittenByEnemyPiece);
        }

        public List<PieceAttackRange> CalculateRangeOfAttack(IReadOnlyPiece piece)
        {
            if (_preCalculatedRangeOfAttack.Any()) return GetRangeOfAttackFromCache(piece);

            List<PieceAttackRange> legalMoves = new();
            List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(piece);

            foreach (PieceLineOfSight move in lineOfSight)
            {
                if (piece is Pawn pawn && move.Direction.Equals(pawn.ForwardDirection)) { continue; }
                Direction currentDirection = move.Direction;
                if (!move.LineOfSight.Any()) { legalMoves.Add(new(move.Piece, move.Direction, new())); continue; }

                List<IReadOnlyPiece> piecesPosition = _chessboard.GetPieces(move.LineOfSight);
                List<Coordinate> rangeOfAttack = CalculatePositionsUntilTheNearestPiece(piece, piecesPosition, move);
                bool lastPositionIsEmpty = _chessboard.GetPiece(rangeOfAttack.Last()) is null;
                if (lastPositionIsEmpty)
                {
                    legalMoves.Add(new(piece, currentDirection, move.LineOfSight));
                    continue;
                }

                IReadOnlyPiece nearestPiece = _chessboard.GetPiece(rangeOfAttack.Last())!;
                PieceAttackRange moveRange = new(piece, currentDirection, rangeOfAttack, nearestPiece);

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
                List<PieceDistances> piecesInLineOfSight = PieceDistances.CalculateDistanceBetweenPieces(piece, _chessboard.GetPieces(positions));

                lineOfSight.Add(new(piece, direction, positions, piecesInLineOfSight));
            }

            return lineOfSight;
        }

        private static List<Coordinate> CalculatePositionsUntilTheNearestPiece(IReadOnlyPiece piece, List<IReadOnlyPiece> piecesPosition, PieceLineOfSight move)
        {
            if (!piecesPosition.Any()) return new(move.LineOfSight);
            List<PieceDistances> distances = PieceDistances.CalculateDistanceBetweenPieces(piece, piecesPosition);
            PieceDistances nearestPiece = distances.FirstOrDefault();
            List<Coordinate> rangeOfAttack = move.LineOfSight.Take(nearestPiece.DistanceFromOrigin).ToList();

            return rangeOfAttack;
        }

        private bool SpecialPawnRuleApplies(PieceLineOfSight move, Pawn pawn, List<IReadOnlyPiece> pieces, bool lastPositionIsEmpty)
        {
            bool isNotEnPassantPosition = !move.LineOfSight.Contains(_chessboard.EnPassantAvailability.EnPassantPosition!);
            bool isForwardMove = move.Direction.Equals(pawn.ForwardDirection);
            bool isEmptyDiagonal = !pieces.Any() && !isForwardMove;
            bool isForwardMoveAndHasPiece = !lastPositionIsEmpty && isForwardMove;
            bool isDiagonalAndHasAllyPiece = !isForwardMove && pieces.FirstOrDefault()?.Color == pawn.Color;

            return isForwardMoveAndHasPiece || (isEmptyDiagonal && isNotEnPassantPosition) || isDiagonalAndHasAllyPiece;
        }

        private List<Coordinate> CalculatePositionsNotAllowedForTheKing(List<IReadOnlyPiece> piecesPosition)
        {
            List<PieceAttackRange> allMoves = new();
            foreach (IReadOnlyPiece piece in piecesPosition)
            {
                List<PieceAttackRange> moves = CalculateRangeOfAttack(piece);

                if (!piece.IsLongRange) { allMoves.AddRange(moves); continue; }
                allMoves.AddRange(moves.FindAll(m => !m.IsHittingTheEnemyKing));
                var movesHittingTheEnemyKing = moves.FindAll(m => m.IsHittingTheEnemyKing);

                foreach (var move in movesHittingTheEnemyKing)
                {
                    Coordinate? positionBehindTheKing = Coordinate.CalculateNextPosition(move.NearestPiece!.Origin, move.Direction);
                    if (positionBehindTheKing is null) continue;
                    move.AttackRange.Add(positionBehindTheKing);
                }
                allMoves.AddRange(movesHittingTheEnemyKing);
            }
            List<Coordinate> positionsNotAllowedToMove = allMoves.SelectMany(m => m.AttackRange).ToList();

            return positionsNotAllowedToMove;
        }

        private List<PieceAttackRange> GetRangeOfAttackFromCache(IReadOnlyPiece piece)
        {
            return _preCalculatedRangeOfAttack.Where(m => m.Piece == piece).ToList();
        }

        private List<PieceLineOfSight> GetLineOfSightFromCache(IReadOnlyPiece piece)
        {
            return _preCalculatedLineOfSight.Where(m => m.Piece == piece).ToList();
        }

        private List<PieceLineOfSight> CalculatePinMovesFromPlayer(Color player)
        {
            if (_preCalculatedPinMoves.Any()) return _preCalculatedPinMoves.Where(m => m.Piece.Color.Value == player.Value).ToList();

            List<PieceLineOfSight> movesPinningAPiece = new();
            List<IReadOnlyPiece> longRangePieces = _chessboard.GetPieces(player).FindAll(p => p.IsLongRange);
            if (!longRangePieces.Any()) return new();

            foreach (var piece in longRangePieces)
            {
                var lineOfSightOfAllyPiece = CalculateLineOfSight(piece).FindAll(l => l.AnyPieceInLineOfSight && l.PiecesInLineOfSight.Count >= 2);
                bool doesntHavePiecesInLineOfSight = !lineOfSightOfAllyPiece.Select(m => m.PiecesInLineOfSight).Any();
                if (doesntHavePiecesInLineOfSight) continue;

                bool pieceAtFirstPositionIsAnEnemyPiece(PieceLineOfSight lineOfSight) => lineOfSight.PiecesInLineOfSight.FirstOrDefault().Piece.Color != player && lineOfSight.PiecesInLineOfSight.FirstOrDefault().Piece is not King;
                bool pieceAtSecondPositionIsTheEnemyKing(PieceLineOfSight lineOfSight) => lineOfSight.PiecesInLineOfSight.ElementAtOrDefault(1).Piece.Color != player && lineOfSight.PiecesInLineOfSight.ElementAtOrDefault(1).Piece is King;
                bool isPinningTheEnemyPiece(PieceLineOfSight lineOfSight) => pieceAtFirstPositionIsAnEnemyPiece(lineOfSight) && pieceAtSecondPositionIsTheEnemyKing(lineOfSight);

                var pinnedPieces = lineOfSightOfAllyPiece.Where(isPinningTheEnemyPiece).ToList();
                if (!pinnedPieces.Any()) continue;

                movesPinningAPiece.Add(lineOfSightOfAllyPiece.First());
            }

            return movesPinningAPiece;
        }

        private bool CanMoveWhenPinnedByEnemyPiece(IReadOnlyPiece piece, PieceLineOfSight enemyMove)
        {
            List<PieceLineOfSight> pieceLineOfSight = CalculateLineOfSight(piece);
            if (!pieceLineOfSight.Any()) return false;
            List<Coordinate> enemyPositions = enemyMove.LineOfSight;
            enemyPositions.Add(enemyMove.Piece.Origin);
            List<PieceAttackRange> legalMoves = CalculateLegalMoves(piece);
            bool canMove = legalMoves.Exists(m => m.AttackRange.Intersect(enemyMove.LineOfSight).Any());
            return canMove;
        }
    }
}