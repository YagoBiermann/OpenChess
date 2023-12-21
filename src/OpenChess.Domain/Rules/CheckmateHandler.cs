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

    }
}