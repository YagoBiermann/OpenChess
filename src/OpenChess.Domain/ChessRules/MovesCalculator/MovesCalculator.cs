
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
            List<MoveDirections> legalMoves = CalculateMoves(piece);

            return legalMoves.Exists(m => m.RangeOfAttack!.Contains(destination));
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
            if (_preCalculatedMoves.Any()) return GetPreCalculatedMoves(piece);

            List<MoveDirections> legalMoves = new();
            List<MoveDirections> fullMoveRange = piece.CalculateMoveRange();

            foreach (MoveDirections move in fullMoveRange)
            {
                Direction currentDirection = move.Direction;
                if (move.FullRange is null)
                {
                    MoveDirections pawnMoveRange = CreateMoveRange(piece, currentDirection);
                    legalMoves.Add(pawnMoveRange);
                    continue;
                }
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

        private List<MoveDirections> GetPreCalculatedMoves(IReadOnlyPiece piece)
        {
            var moves = _preCalculatedMoves.Where(p => p.Equals(piece)).ToList();
            return moves;
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


        public List<MoveDirections> CalculateInCheckMoves(IReadOnlyPiece piece)
        {
            if (piece is King king) return CalculateKingMoves(king);
            return CalculateIntersectionWithEnemyMovesTowardsTheKing(piece);
        }

        private List<MoveDirections> CalculateIntersectionWithEnemyMovesTowardsTheKing(IReadOnlyPiece piece)
        {
            if (piece is King) throw new ChessboardException("This method cannot handle king moves");
            List<Coordinate> enemyMoves = CalculateMoveTowardsTheKing(piece);
            List<MoveDirections> legalMoves = _legalMovesCalculator.CalculateMoves(piece);
            bool movesThatNotIntersectsTheEnemyMoves(MoveDirections moves) => moves.Coordinates.Except(enemyMoves).Any();
            legalMoves.RemoveAll(movesThatNotIntersectsTheEnemyMoves);

            return legalMoves;
        }

        private List<MoveDirections> CalculateKingMoves(King king)
        {
            List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(ColorUtils.GetOppositeColor(king.Color));
            List<Coordinate> protectedPiecesPosition = GetPositionOfProtectedPieces(piecesPosition);
            List<Coordinate> enemyMoves = CalculateAllEnemyMoves(piecesPosition).SelectMany(m => m.SelectMany(c => c.Coordinates)).ToList();
            enemyMoves.AddRange(protectedPiecesPosition);

            List<MoveDirections> kingMoves = _legalMovesCalculator.CalculateMoves(king);
            bool kingMovesHittenByEnemyPiece(MoveDirections p) => enemyMoves.Intersect(p.Coordinates).Any();
            kingMoves.RemoveAll(kingMovesHittenByEnemyPiece);

            return kingMoves;
        }

        private List<List<MoveDirections>> CalculateAllEnemyMoves(List<Coordinate> piecesPosition)
        {
            List<List<MoveDirections>> allMoves = new();
            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                List<MoveDirections> move = CalculateEnemyMoves(piece);
                allMoves.Add(move);
            }

            return allMoves;
        }

        private List<MoveDirections> CalculateEnemyMoves(IReadOnlyPiece piece)
        {
            if (piece is Pawn pawn)
            {
                List<MoveDirections> pawnMoves = pawn.CalculateMoveRange();
                pawnMoves.RemoveAll(m => m.Direction.Equals(pawn.ForwardDirection));
                return pawnMoves;
            }

            List<MoveDirections> moves = _legalMovesCalculator.CalculateMoves(piece);
            foreach (MoveDirections move in moves)
            {
                if (!move.Coordinates.Any()) continue;
                IReadOnlyPiece? pieceAtLastPosition = _chessboard.GetReadOnlySquare(move.Coordinates.Last()).ReadOnlyPiece;
                if (pieceAtLastPosition is null) continue;
                bool isEnemyKing = pieceAtLastPosition is King && pieceAtLastPosition.Color != piece.Color;
                if (!isEnemyKing) continue;

                Coordinate? nextPosition = Coordinate.CalculateNextPosition(pieceAtLastPosition.Origin, move.Direction);
                if (nextPosition is null) continue;
                move.Coordinates.Add(nextPosition);
            }

            return moves;
        }

        private List<Coordinate> GetPositionOfProtectedPieces(List<Coordinate> piecesPosition)
        {
            List<Coordinate> protectedPieces = new();
            if (!piecesPosition.Any()) return protectedPieces;

            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                List<MoveDirections> moves = _protectedPiecesCalculator.CalculateMoves(piece);
                foreach (MoveDirections move in moves)
                {
                    if (!move.Coordinates.Any()) continue;
                    Coordinate lastPosition = move.Coordinates.Last();
                    if (_chessboard.GetReadOnlySquare(lastPosition).HasPiece) protectedPieces.Add(lastPosition);
                }
            }

            return protectedPieces;
        }

        private List<Coordinate> CalculateMoveTowardsTheKing(IReadOnlyPiece piece)
        {
            List<MoveDirections> moves = _legalMovesCalculator.CalculateMoves(piece);
            List<Coordinate> movesTowardsTheKing = new();

            foreach (MoveDirections move in moves)
            {
                if (!move.Coordinates.Any()) continue;
                IReadOnlySquare square = _chessboard.GetReadOnlySquare(move.Coordinates.Last());
                if (square.HasPiece && square.ReadOnlyPiece is King && square.ReadOnlyPiece.Color != piece.Color)
                {
                    Coordinate kingPosition = move.Coordinates.Last();
                    movesTowardsTheKing.Add(piece.Origin);
                    movesTowardsTheKing.AddRange(move.Coordinates);
                    movesTowardsTheKing.Remove(kingPosition);
                    break;
                }
            }

            return movesTowardsTheKing;
        }
    }
}