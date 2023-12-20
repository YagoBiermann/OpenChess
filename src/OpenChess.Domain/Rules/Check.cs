namespace OpenChess.Domain
{
    internal static class Check
    {
        public static int CalculateCheckAmount(Color player, Chessboard chessboard)
        {
            Color enemyPlayer = ColorUtils.GetOppositeColor(player);
            List<Coordinate> piecePositions = chessboard.GetPiecesPosition(enemyPlayer);
            int checkAmount = 0;
            foreach (Coordinate origin in piecePositions)
            {
                IReadOnlyPiece piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece!;
                if (IsHittingTheEnemyKing(piece, chessboard)) { checkAmount++; };
            }

            return checkAmount;
        }

        public static bool IsInCheck(Color player, Chessboard chessboard)
        {
            return CalculateCheckAmount(player, chessboard) > 0;
        }

        public static bool IsHittingTheEnemyKing(IReadOnlyPiece piece, Chessboard chessboard)
        {
            IMoveCalculatorStrategy strategy = new CheckMoveStrategy();
            List<MoveDirections> moveRange = new MovesCalculator(chessboard, strategy).CalculateMoves(piece);
            bool isHitting = false;

            foreach (MoveDirections move in moveRange)
            {
                if (!move.Coordinates.Any()) continue;
                IReadOnlySquare square = chessboard.GetReadOnlySquare(move.Coordinates.Last());
                if (square.HasPiece && square.ReadOnlyPiece is King) { isHitting = true; break; }
            }

            return isHitting;
        }
    }
}