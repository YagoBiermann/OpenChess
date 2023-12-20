namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private IReadOnlyChessboard _chessboard;
        public CheckHandler(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }
        public int CalculateCheckAmount(Color player)
        {
            Color enemyPlayer = ColorUtils.GetOppositeColor(player);
            List<Coordinate> piecePositions = _chessboard.GetPiecesPosition(enemyPlayer);
            int checkAmount = 0;
            foreach (Coordinate origin in piecePositions)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece!;
                if (IsHittingTheEnemyKing(piece)) { checkAmount++; };
            }

            return checkAmount;
        }

        public bool IsInCheck(Color player)
        {
            return CalculateCheckAmount(player) > 0;
        }

        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            IMoveCalculatorStrategy strategy = new CheckMoveStrategy();
            List<MoveDirections> moveRange = new MovesCalculator(_chessboard, strategy).CalculateMoves(piece);
            bool isHitting = false;

            foreach (MoveDirections move in moveRange)
            {
                if (!move.Coordinates.Any()) continue;
                IReadOnlySquare square = _chessboard.GetReadOnlySquare(move.Coordinates.Last());
                if (square.HasPiece && square.ReadOnlyPiece is King) { isHitting = true; break; }
            }

            return isHitting;
        }
    }
}