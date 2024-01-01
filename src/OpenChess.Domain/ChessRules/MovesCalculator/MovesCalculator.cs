
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
            List<PieceRangeOfAttack> legalMoves = CalculateLegalMoves(piece);
            return legalMoves.SelectMany(m => m.RangeOfAttack).Contains(destination);
        }

        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            return CalculateRangeOfAttack(piece).Where(m => m.IsHittingTheEnemyKing).ToList().Any();
        }

        public bool IsPinned(IReadOnlyPiece piece)
        {
            List<IReadOnlyPiece> enemyPieces = _chessboard.GetPieces(ColorUtils.GetOppositeColor(piece.Color)).FindAll(p => p.IsLongRange);
            if (!enemyPieces.Any()) return false;
            foreach (IReadOnlyPiece enemyPiece in enemyPieces)
            {
                var piecesInLineOfSightOfEnemyPiece = CalculateLineOfSight(enemyPiece).FindAll(l => l.AnyPieceInLineOfSight && l.PiecesInLineOfSight.Count >= 2).Select(m => m.PiecesInLineOfSight).ToList();
                if (!piecesInLineOfSightOfEnemyPiece.Any()) continue;

                bool pieceAtSecondPositionIsTheAllyKing(List<PieceDistances> pieceDistances) => pieceDistances.ElementAtOrDefault(1).Piece.Color == piece.Color && pieceDistances.ElementAtOrDefault(1).Piece is King;
                bool pieceAtFirstPositionIsTheSamePiece(List<PieceDistances> pieceDistances) => pieceDistances.ElementAtOrDefault(0).Piece.Equals(piece);
                bool isPinnedByEnemyPiece(List<PieceDistances> pieceDistances) => pieceAtFirstPositionIsTheSamePiece(pieceDistances) && pieceAtSecondPositionIsTheAllyKing(pieceDistances);

                if (piecesInLineOfSightOfEnemyPiece.Where(isPinnedByEnemyPiece).Any()) return true;
            }

            return false;
        }

        public void CalculateAndCacheAllMoves()
        {
            _preCalculatedRangeOfAttack.Clear();
            _preCalculatedLineOfSight.Clear();
            List<IReadOnlyPiece> pieces = _chessboard.GetAllPieces();
            List<PieceRangeOfAttack> allRangeOfAttack = new();
            List<PieceLineOfSight> allLineOfSight = new();

            foreach (var piece in pieces)
            {
                List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(piece);
                List<PieceRangeOfAttack> rangeOfAttack = CalculateRangeOfAttack(piece);
                allRangeOfAttack.AddRange(rangeOfAttack);
                allLineOfSight.AddRange(lineOfSight);
            }

            _preCalculatedRangeOfAttack.AddRange(allRangeOfAttack);
            _preCalculatedLineOfSight.AddRange(allLineOfSight);
        }

        public List<PieceRangeOfAttack> CalculateAllMoves()
        {
            if (!_preCalculatedRangeOfAttack.Any()) { CalculateAndCacheAllMoves(); }
            return new(_preCalculatedRangeOfAttack);
        }

        public List<PieceRangeOfAttack> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            if (piece is Pawn pawn) { return CalculatePawnMoves(pawn); }
            List<PieceRangeOfAttack> legalMoves = CalculateRangeOfAttack(piece);
            legalMoves.Where(m => m.NearestPiece?.Color == piece.Color).ToList().ForEach(m =>
            {
                m.RangeOfAttack.Remove(m.RangeOfAttack.Last()); // Remove ally piece position
            });

            return legalMoves;
        }

        public List<PieceRangeOfAttack> CalculatePawnMoves(Pawn pawn)
        {
            List<PieceRangeOfAttack> legalMoves = new();
            List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(pawn);
            foreach (PieceLineOfSight move in lineOfSight)
            {
                if (!move.LineOfSight.Any()) { legalMoves.Add(new(move.Piece, move.Direction, move.LineOfSight)); continue; }
                List<IReadOnlyPiece> piecesPosition = _chessboard.GetPieces(move.LineOfSight);
                List<Coordinate> rangeOfAttack = CalculatePositionsUntilTheNearestPiece(pawn, piecesPosition, move);
                bool lastPositionIsEmpty = !_chessboard.GetReadOnlySquare(rangeOfAttack.Last()).HasPiece;
                if (SpecialPawnRuleApplies(move, pawn, piecesPosition, lastPositionIsEmpty))
                {
                    rangeOfAttack.Remove(rangeOfAttack.Last());
                    PieceRangeOfAttack pawnMoves = new(pawn, move.Direction, rangeOfAttack);
                    legalMoves.Add(pawnMoves);
                    continue;
                }

                IReadOnlyPiece nearestPiece = _chessboard.GetReadOnlySquare(rangeOfAttack.Last()).ReadOnlyPiece!;
                legalMoves.Add(new(pawn, move.Direction, rangeOfAttack, nearestPiece));
            }

            return legalMoves;
        }

        public List<PieceRangeOfAttack> CalculateKingMoves(Color player)
        {
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(ColorUtils.GetOppositeColor(player));
            List<Coordinate> positionsNotAllowedForTheKing = CalculatePositionsNotAllowedForTheKing(pieces);

            IReadOnlyPiece king = _chessboard.GetPieces(player).Find(p => p is King)!;
            List<PieceRangeOfAttack> kingMoves = CalculateLegalMoves(king);
            bool kingMovesNotHittenByEnemyPiece(PieceRangeOfAttack k) => k.RangeOfAttack.Except(positionsNotAllowedForTheKing).Any();

            return kingMoves.FindAll(kingMovesNotHittenByEnemyPiece);
        }

        public List<PieceRangeOfAttack> CalculateRangeOfAttack(IReadOnlyPiece piece)
        {
            if (_preCalculatedRangeOfAttack.Any()) return GetRangeOfAttackFromCache(piece);

            List<PieceRangeOfAttack> legalMoves = new();
            List<PieceLineOfSight> lineOfSight = CalculateLineOfSight(piece);

            foreach (PieceLineOfSight move in lineOfSight)
            {
                if (piece is Pawn pawn && move.Direction.Equals(pawn.ForwardDirection)) { continue; }
                Direction currentDirection = move.Direction;
                if (!move.LineOfSight.Any()) { legalMoves.Add(new(move.Piece, move.Direction, new())); continue; }

                List<IReadOnlyPiece> piecesPosition = _chessboard.GetPieces(move.LineOfSight);
                List<Coordinate> rangeOfAttack = CalculatePositionsUntilTheNearestPiece(piece, piecesPosition, move);
                bool lastPositionIsEmpty = !_chessboard.GetReadOnlySquare(rangeOfAttack.Last()).HasPiece;
                if (lastPositionIsEmpty)
                {
                    legalMoves.Add(new(piece, currentDirection, move.LineOfSight));
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
                List<PieceDistances> piecesInLineOfSight = PieceDistances.CalculateDistance(piece, _chessboard.GetPieces(positions));

                lineOfSight.Add(new(piece, direction, positions, piecesInLineOfSight));
            }

            return lineOfSight;
        }

        private static List<Coordinate> CalculatePositionsUntilTheNearestPiece(IReadOnlyPiece piece, List<IReadOnlyPiece> piecesPosition, PieceLineOfSight move)
        {
            if (!piecesPosition.Any()) return new(move.LineOfSight);
            List<PieceDistances> distances = PieceDistances.CalculateDistance(piece, piecesPosition);
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
            List<PieceRangeOfAttack> allMoves = new();
            foreach (IReadOnlyPiece piece in piecesPosition)
            {
                List<PieceRangeOfAttack> moves = CalculateRangeOfAttack(piece);

                if (!piece.IsLongRange) { allMoves.AddRange(moves); continue; }
                allMoves.AddRange(moves.FindAll(m => !m.IsHittingTheEnemyKing));
                var movesHittingTheEnemyKing = moves.FindAll(m => m.IsHittingTheEnemyKing);

                foreach (var move in movesHittingTheEnemyKing)
                {
                    Coordinate? positionBehindTheKing = Coordinate.CalculateNextPosition(move.NearestPiece!.Origin, move.Direction);
                    if (positionBehindTheKing is null) continue;
                    move.RangeOfAttack.Add(positionBehindTheKing);
                }
                allMoves.AddRange(movesHittingTheEnemyKing);
            }
            List<Coordinate> positionsNotAllowedToMove = allMoves.SelectMany(m => m.RangeOfAttack).ToList();

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