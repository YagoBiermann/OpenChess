namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private IReadOnlyChessboard _chessboard;
        public CheckHandler(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public bool IsInCheck(Color player, out int checkAmount)
        {
            checkAmount = CalculateCheckAmount(player);
            return checkAmount > 0;
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
    }
}