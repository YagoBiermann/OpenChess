
namespace OpenChess.Domain
{
    internal class DefaultLegalMoves : IMoveCalculator
    {
        private IReadOnlyChessboard _chessboard;
        public DefaultLegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
        {
            List<MoveDirections> legalMoves = new();
            List<MoveDirections> moveRange = new MovesCalculator(_chessboard).CalculateMoves(piece);

            foreach (MoveDirections move in moveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> rangeOfAttack = move.Coordinates;
                if (!move.Coordinates.Any()) { legalMoves.Add(new(currentDirection, rangeOfAttack)); continue; }
                IReadOnlySquare square = _chessboard.GetReadOnlySquare(rangeOfAttack.Last());
                if (!square.HasPiece) { legalMoves.Add(new(currentDirection, rangeOfAttack)); continue; }
                bool isKing = square.HasTypeOfPiece(typeof(King));

                int lastPosition = rangeOfAttack.Count - 1;
                if (!square.HasEnemyPiece(piece.Color) || isKing) rangeOfAttack.RemoveAt(lastPosition);

                legalMoves.Add(new(currentDirection, rangeOfAttack));
            }

            return legalMoves;
        }
    }
}