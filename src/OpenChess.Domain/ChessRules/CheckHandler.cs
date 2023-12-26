namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _playerInCheckMovesCalculator;
        private LegalMovesCalculator _legalMovesCalculator;
        public CheckHandler(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            _playerInCheckMovesCalculator = new PlayerInCheckMovesCalculator(chessboard);
            _legalMovesCalculator = new LegalMovesCalculator(_chessboard);
        }

        public bool IsInCheckmate(Color player, out CheckState checkState)
        {
            if (!IsInCheck(player, out checkState)) return false;
            if (checkState == CheckState.DoubleCheck) return !CanSolveCheckByMovingTheKing(player);

            return !CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(player) && !CanSolveCheckByMovingTheKing(player);
        }

        public bool IsInCheck(Color player, out CheckState checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CheckState.NotInCheck;
        }

        private bool CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(Color player)
        {
            List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(player);
            List<MoveDirections> moves = new();
            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                if (piece is King) continue;
                moves.AddRange(_playerInCheckMovesCalculator.CalculateMoves(piece));
            }
            bool canBeSolved = moves.Any();

            return canBeSolved;
        }

        private bool CanSolveCheckByMovingTheKing(Color player)
        {
            IReadOnlyPiece king = _chessboard.FindPiece(player, typeof(King)).First();
            List<MoveDirections> kingMoves = _playerInCheckMovesCalculator.CalculateMoves(king);
            bool canBeSolved = kingMoves.SelectMany(m => m.Coordinates).ToList().Any();

            return canBeSolved;
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

        private bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            if (piece is King) return false;
            List<MoveDirections> moves = _legalMovesCalculator.CalculateMoves(piece);

            foreach (var move in moves)
            {
                if (!move.Coordinates.Any()) continue;
                var lastPosition = move.Coordinates.Last();
                var pieceAtLastPosition = _chessboard.GetReadOnlySquare(lastPosition).ReadOnlyPiece;

                if (pieceAtLastPosition is King && pieceAtLastPosition.Color != piece.Color) { return true; }
            }

            return false;
        }
    }
}