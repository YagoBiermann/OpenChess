namespace OpenChess.Domain
{
    internal static class Check
    {
        public static int CalculateCheckAmount(Color player, Chessboard chessboard)
        {
            Color enemyPlayer = player == Color.Black ? Color.White : Color.Black;
            List<Coordinate> piecePositions = chessboard.GetPiecesPosition(enemyPlayer);
            int checkAmount = 0;
            foreach (Coordinate origin in piecePositions)
            {
                if (chessboard.GetReadOnlySquare(origin).ReadOnlyPiece!.IsHittingTheEnemyKing()) { checkAmount++; };
            }

            return checkAmount;
        }

        public static bool IsInCheck(Color player, Chessboard chessboard)
        {
            return CalculateCheckAmount(player, chessboard) > 0;
        }
    }
}