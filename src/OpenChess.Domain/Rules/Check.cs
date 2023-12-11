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
                IReadOnlyPiece piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece!;
                if (!IsHittingTheEnemyKing(piece, chessboard)) { checkAmount++; };
            }

            return checkAmount;
        }

        public static bool IsInCheck(Color player, Chessboard chessboard)
        {
            return CalculateCheckAmount(player, chessboard) > 0;
        }

        public static bool IsHittingTheEnemyKing(IReadOnlyPiece piece, Chessboard chessboard)
        {
            List<MoveDirections> moveRange = piece.CalculateMoveRange();
            bool isHitting = false;

            foreach (MoveDirections move in moveRange)
            {
                if (piece is Pawn pawn && move.Direction.Equals(pawn.ForwardDirection))
                {
                    continue;
                }

                List<Coordinate> pieces = chessboard.GetPiecesPosition(move.Coordinates);
                if (!pieces.Any()) continue;
                List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(piece.Origin, pieces);
                CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances)!;
                IReadOnlySquare square = chessboard.GetReadOnlySquare(nearestPiece.Position);
                if (square.HasEnemyPiece(piece.Color) && square.HasTypeOfPiece(typeof(King))) { isHitting = true; break; }
            }

            return isHitting;
        }
    }
}