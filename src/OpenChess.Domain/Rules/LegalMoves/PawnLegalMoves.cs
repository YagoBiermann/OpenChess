
namespace OpenChess.Domain
{
    internal class PawnLegalMoves : ILegalMoves
    {
        private IReadOnlyChessboard _chessboard;
        public PawnLegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            Pawn pawn = (Pawn)piece;
            List<MoveDirections> legalMoves = new();
            List<MoveDirections> moveRange = new MovesCalculator(_chessboard).CalculateLegalMoves(piece);

            foreach (MoveDirections move in moveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> currentCoordinates = move.Coordinates;
                if (currentDirection.Equals(pawn.ForwardDirection))
                {
                    List<Coordinate> forwardMoves = CalculateForwardMoves(_chessboard, currentCoordinates);
                    legalMoves.Add(new(currentDirection, forwardMoves));
                    continue;
                };

                List<Coordinate> emptyList = new();
                MoveDirections emptyPosition = new(currentDirection, emptyList);
                MoveDirections sameCurrentPosition = new(currentDirection, currentCoordinates);

                Coordinate? diagonal = currentCoordinates.FirstOrDefault();
                bool diagonalIsOutOfChessboard = diagonal is null;
                if (diagonalIsOutOfChessboard) { legalMoves.Add(emptyPosition); continue; };

                IReadOnlySquare square = _chessboard.GetReadOnlySquare(diagonal!);
                bool isEnPassant = diagonal!.Equals(_chessboard.EnPassantAvailability.EnPassantPosition);
                if (isEnPassant) { legalMoves.Add(sameCurrentPosition); continue; };
                if (!square.HasPiece) { legalMoves.Add(emptyPosition); continue; }
                bool hasAllyPiece = !square.HasEnemyPiece(pawn.Color);
                bool hasKing = square.HasTypeOfPiece(typeof(King));
                if (hasAllyPiece || hasKing) { legalMoves.Add(emptyPosition); continue; }

                legalMoves.Add(sameCurrentPosition);
            }

            return legalMoves;
        }

        private List<Coordinate> CalculateForwardMoves(IReadOnlyChessboard chessboard, List<Coordinate> forwardCoordinates)
        {
            List<Coordinate> forwardMoves = new(forwardCoordinates);
            IReadOnlySquare square = chessboard.GetReadOnlySquare(forwardMoves.Last());
            if (!square.HasPiece) { return forwardMoves; }
            int lastPosition = forwardMoves.Count - 1;
            forwardMoves.RemoveAt(lastPosition);

            return forwardMoves;
        }

    }
}