namespace OpenChess.Domain
{
    internal class CheckmateHandler
    {
        private IReadOnlyChessboard _chessboard;
        private CheckHandler _checkHandler;
        public CheckmateHandler(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            _checkHandler = new(chessboard);
        }

        public bool IsCheckmate()
        {
            int checkAmount = _checkHandler.CalculateCheckAmount(_chessboard.Turn);
            bool isNotInCheck = checkAmount == 0;
            bool isInCheck = checkAmount == 1;
            bool isInDoubleCheck = checkAmount >= 2;
            if (isNotInCheck) return false;
            if (isInDoubleCheck) return CanSolveCheckByMovingTheKing();
            if (isInCheck) return CanSolveCheckByCoveringTheKing() || CanSolveCheckByMovingAPiece() || CanSolveCheckByMovingTheKing();
            return true;
        }

        private bool CanSolveCheckByMovingAPiece() { throw new Exception("Not implemented yet"); }
        private bool CanSolveCheckByCoveringTheKing() { throw new Exception("Not implemented yet"); }
        private bool CanSolveCheckByMovingTheKing() { throw new Exception("Not implemented yet"); }
    }
}