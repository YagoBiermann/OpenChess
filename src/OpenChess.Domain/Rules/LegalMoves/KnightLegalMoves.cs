
namespace OpenChess.Domain
{
    internal class KnightLegalMoves : ILegalMoves
    {
        private IReadOnlyChessboard _chessboard;
        public KnightLegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            List<MoveDirections> legalMoves = new() { };
            List<MoveDirections> moveRange = piece.CalculateMoveRange();

            foreach (MoveDirections move in moveRange)
            {
                List<Coordinate> currentPosition = move.Coordinates;
                bool isOutOfChessboard = !currentPosition.Any();
                if (isOutOfChessboard) { legalMoves.Add(new(move.Direction, currentPosition)); continue; };

                IReadOnlySquare currentSquare = _chessboard.GetReadOnlySquare(currentPosition.FirstOrDefault()!);
                if (!currentSquare.HasPiece) { legalMoves.Add(new(move.Direction, currentPosition)); continue; }

                bool isAllyPieceOrKing = !currentSquare.HasEnemyPiece(piece.Color) || currentSquare.HasTypeOfPiece(typeof(King));
                if (isAllyPieceOrKing) { legalMoves.Add(new(move.Direction, new())); continue; };

                legalMoves.Add(new(move.Direction, currentPosition));
            }

            return legalMoves;
        }
    }
}