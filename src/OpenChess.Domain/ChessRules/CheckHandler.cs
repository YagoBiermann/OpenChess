namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _movesCalculator;
        public CheckHandler(IReadOnlyChessboard chessboard, IMoveCalculator movesCalculator)
        {
            _chessboard = chessboard;
            _movesCalculator = movesCalculator;
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
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(player);
            foreach (IReadOnlyPiece piece in pieces)
            {
                if (piece is King) continue;
                if (_movesCalculator.PieceCanSolveTheCheck(piece)) return true;
            }

            return false;
        }

        private bool CanSolveCheckByMovingTheKing(Color player)
        {
            IReadOnlyPiece king = _chessboard.GetPieces(player).Find(p => p is King)!;
            bool canBeSolved = _movesCalculator.PieceCanSolveTheCheck(king);

            return canBeSolved;
        }

        private int CalculateCheckAmount(Color player)
        {
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(ColorUtils.GetOppositeColor(player));
            int checkAmount = 0;

            foreach (IReadOnlyPiece piece in pieces) { if (_movesCalculator.IsHittingTheEnemyKing(piece)) { checkAmount++; }; }

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
    }
}