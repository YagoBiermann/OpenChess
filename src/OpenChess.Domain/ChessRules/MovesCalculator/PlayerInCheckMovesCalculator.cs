
namespace OpenChess.Domain
{
    internal class PlayerInCheckMovesCalculator : IMoveCalculator
    {
        private IReadOnlyChessboard _chessboard;
        private LegalMovesCalculator _legalMovesCalculator;
        private IMoveCalculator _protectedPiecesCalculator;
        public PlayerInCheckMovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            _legalMovesCalculator = new LegalMovesCalculator(_chessboard);
            IMoveCalculatorStrategy allyPiecesStrategy = new IncludeAllyPieceStrategy();
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard, allyPiecesStrategy);
            _protectedPiecesCalculator = moveCalculator;
        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
        {
            if (piece is King king) return CalculateKingMoves(king);
            return CalculatePieceMoves(piece);
        }

        private List<MoveDirections> CalculatePieceMoves(IReadOnlyPiece piece)
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