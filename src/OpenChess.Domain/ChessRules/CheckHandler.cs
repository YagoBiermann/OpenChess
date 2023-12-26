namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private Dictionary<IReadOnlyPiece, List<Coordinate>> _movesTowardsTheKing = new();
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _checkmateCalculator;
        private IMoveCalculator _protectedPiecesCalculator;
        private LegalMovesCalculator _legalMovesCalculator;
        public CheckHandler(IReadOnlyChessboard chessboard)
        {
            IMoveCalculatorStrategy allyPiecesStrategy = new IncludeAllyPieceStrategy();
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard, allyPiecesStrategy);

            _chessboard = chessboard;
            _protectedPiecesCalculator = moveCalculator;
            _checkmateCalculator = new CheckmateCalculator(chessboard);
            _legalMovesCalculator = new LegalMovesCalculator(_chessboard);
        }

        public bool IsInCheckmate(Color player, out CheckState checkState)
        {
            if (!IsInCheck(player, out checkState)) return false;
            if (checkState == CheckState.DoubleCheck) return !CanSolveCheckByMovingTheKing(player);

            return CanSolveCheckByCoveringTheKing() || CanSolveCheckByCapturingEnemyPiece() || CanSolveCheckByMovingTheKing(player);
        }

        public bool IsInCheck(Color player, out CheckState checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CheckState.NotInCheck;
        }

        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            List<Coordinate> movesTowardsTheKing = CalculateMoveTowardsTheKing(piece);
            return movesTowardsTheKing.Any();
        }

        private List<Coordinate> CalculateMoveTowardsTheKing(IReadOnlyPiece piece)
        {
            if (_movesTowardsTheKing.ContainsKey(piece)) { return _movesTowardsTheKing[piece]; };
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
                    _movesTowardsTheKing.Add(piece, movesTowardsTheKing);
                    break;
                }
            }

            return movesTowardsTheKing;
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

        private bool CanSolveCheckByCapturingEnemyPiece() { return true; }

        private bool CanSolveCheckByCoveringTheKing() { return true; }

        private bool CanSolveCheckByMovingTheKing(Color player)
        {
            IReadOnlyPiece king = _chessboard.FindPiece(player, typeof(King)).First();
            List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(ColorUtils.GetOppositeColor(player));
            List<Coordinate> protectedPiecesPosition = GetPositionOfProtectedPieces(piecesPosition);
            List<Coordinate> enemyMoves = CalculateCheckmateMoves(piecesPosition).SelectMany(m => m.SelectMany(c => c.Coordinates)).ToList();
            enemyMoves.AddRange(protectedPiecesPosition);

            List<Coordinate> kingMoves = _legalMovesCalculator.CalculateMoves(king).SelectMany(m => m.Coordinates).ToList();
            bool canBeSolved = kingMoves.Except(enemyMoves).ToList().Any();

            return canBeSolved;
        }

        private List<List<MoveDirections>> CalculateCheckmateMoves(List<Coordinate> piecesPosition)
        {
            List<List<MoveDirections>> allMoves = new();
            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                List<MoveDirections> move = _checkmateCalculator.CalculateMoves(piece);
                allMoves.Add(move);
            }

            return allMoves;
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
    }
}