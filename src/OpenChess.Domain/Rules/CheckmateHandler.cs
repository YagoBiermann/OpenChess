namespace OpenChess.Domain
{
    internal class CheckmateHandler
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _legalMovesCalculator;
        private IMoveCalculator _protectedPiecesCalculator;
        public CheckmateHandler(IReadOnlyChessboard chessboard)
        {
            IMoveCalculatorStrategy allyPiecesStrategy = new IncludeAllyPieceStrategy();
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard, allyPiecesStrategy);

            IMoveCalculatorStrategy legalMovesStrategy = new IncludeEnemyPieceStrategy();
            IMoveCalculator legalMovesCalculator = new MovesCalculator(chessboard, legalMovesStrategy);
            _chessboard = chessboard;
            _protectedPiecesCalculator = moveCalculator;
            _legalMovesCalculator = legalMovesCalculator;
        }

        public bool IsInCheckmate(Color player, int checkAmount)
        {
            bool isNotInCheck = checkAmount == 0;
            bool isInDoubleCheck = checkAmount >= 2;
            if (isNotInCheck) return false;
            bool canSolveCheckByMovingTheKing = CanSolveCheckByMovingTheKing(player);
            if (isInDoubleCheck) return canSolveCheckByMovingTheKing;

            return CanSolveCheckByCoveringTheKing() || CanSolveCheckByMovingAPiece() || canSolveCheckByMovingTheKing;
        }

        private bool CanSolveCheckByMovingAPiece() { throw new Exception("Not implemented yet"); }

        private bool CanSolveCheckByCoveringTheKing() { throw new Exception("Not implemented yet"); }

        private bool CanSolveCheckByMovingTheKing(Color player)
        {
            IMoveCalculator legalMoves = new LegalMovesCalculator(_chessboard);
            IReadOnlyPiece king = _chessboard.FindPiece(player, typeof(King)).First();
            List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(ColorUtils.GetOppositeColor(player));
            List<Coordinate> protectedPiecesPosition = GetPositionOfProtectedPieces(piecesPosition);
            List<Coordinate> enemyMoves = CalculateAllLegalMoves(piecesPosition);
            enemyMoves.AddRange(protectedPiecesPosition);

            List<Coordinate> kingMoves = legalMoves.CalculateMoves(king).SelectMany(m => m.Coordinates).ToList();
            bool canBeSolved = !kingMoves.Intersect(enemyMoves).Any();

            return canBeSolved;
        }

        private List<Coordinate> CalculateAllLegalMoves(List<Coordinate> piecesPosition)
        {
            List<Coordinate> allMoves = new();
            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                List<MoveDirections> moves = _legalMovesCalculator.CalculateMoves(piece);
                foreach (MoveDirections move in moves)
                {
                    allMoves.AddRange(move.Coordinates);

                    IReadOnlySquare lastPosition = _chessboard.GetReadOnlySquare(move.Coordinates.Last());
                    bool isEnemyKing = lastPosition.ReadOnlyPiece is King && lastPosition.HasEnemyPiece(piece.Color);
                    if (isEnemyKing)
                    {
                        Coordinate? nextPosition = Coordinate.CalculateNextPosition(lastPosition.Coordinate, move.Direction);
                        if (nextPosition is null) { continue; }
                        allMoves.Add(nextPosition);
                    }
                }
            }

            return allMoves;
        }

        private List<Coordinate> GetPositionOfProtectedPieces(List<Coordinate> piecesPosition)
        {
            List<Coordinate> protectedPieces = new();

            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                List<MoveDirections> moves = _protectedPiecesCalculator.CalculateMoves(piece);
                foreach (MoveDirections move in moves)
                {
                    Coordinate lastPosition = move.Coordinates.Last();
                    if (_chessboard.GetReadOnlySquare(lastPosition).HasPiece) protectedPieces.Add(lastPosition);
                }
            }

            return protectedPieces;
        }
    }
}