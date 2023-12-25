namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _legalMovesCalculator;
        private IMoveCalculator _protectedPiecesCalculator;
        public CheckHandler(IReadOnlyChessboard chessboard)
        {
            IMoveCalculatorStrategy allyPiecesStrategy = new IncludeAllyPieceStrategy();
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard, allyPiecesStrategy);

            IMoveCalculatorStrategy legalMovesStrategy = new IncludeEnemyPieceStrategy();
            IMoveCalculator legalMovesCalculator = new MovesCalculator(chessboard, legalMovesStrategy);
            _chessboard = chessboard;
            _protectedPiecesCalculator = moveCalculator;
            _legalMovesCalculator = legalMovesCalculator;
        }

        public bool IsInCheckmate(Color player, out CheckState checkState)
        {
            if (!IsInCheck(player, out checkState)) return false;
            if (checkState == CheckState.DoubleCheck) return CanSolveCheckByMovingTheKing(player);

            return CanSolveCheckByCoveringTheKing() || CanSolveCheckByMovingAPiece() || CanSolveCheckByMovingTheKing(player);
        }

        public bool IsInCheck(Color player, out CheckState checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CheckState.NotInCheck;
        }

        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            List<MoveDirections> moveRange = new LegalMovesCalculator(_chessboard).CalculateMoves(piece);
            bool isHitting = false;

            foreach (MoveDirections move in moveRange)
            {
                if (!move.Coordinates.Any()) continue;
                IReadOnlySquare square = _chessboard.GetReadOnlySquare(move.Coordinates.Last());
                if (square.HasPiece && square.ReadOnlyPiece is King && square.ReadOnlyPiece.Color != piece.Color) { isHitting = true; break; }
            }

            return isHitting;
        }

        private int CalculateCheckAmount(Color player)
        {
            List<Coordinate> piecePositions = _chessboard.GetPiecesPosition(ColorUtils.GetOppositeColor(player));
            int checkAmount = 0;
            foreach (Coordinate position in piecePositions)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                if (IsHittingTheEnemyKing(piece)) { checkAmount++; };
            }

            return checkAmount;
        }

        private static CheckState GetCheckState(int checkAmount)
        {
            return checkAmount switch
            {
                0 => CheckState.NotInCheck,
                1 => CheckState.Check,
                2 => CheckState.DoubleCheck,
                _ => throw new MatchException("The game could not compute the current check state")
            };
        }

        private bool CanSolveCheckByMovingAPiece() { return true; }

        private bool CanSolveCheckByCoveringTheKing() { return true; }

        private bool CanSolveCheckByMovingTheKing(Color player)
        {
            IMoveCalculator legalMoves = new LegalMovesCalculator(_chessboard);
            IReadOnlyPiece king = _chessboard.FindPiece(player, typeof(King)).First();
            List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(ColorUtils.GetOppositeColor(player));
            List<Coordinate> protectedPiecesPosition = GetPositionOfProtectedPieces(piecesPosition);
            List<Coordinate> enemyMoves = CalculateAllLegalMoves(piecesPosition);
            enemyMoves.AddRange(protectedPiecesPosition);

            List<Coordinate> kingMoves = legalMoves.CalculateMoves(king).SelectMany(m => m.Coordinates).ToList();
            bool canBeSolved = !kingMoves.Except(enemyMoves).Any();

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
                    if (!move.Coordinates.Any()) continue;
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
            if (!protectedPieces.Any()) return protectedPieces;

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