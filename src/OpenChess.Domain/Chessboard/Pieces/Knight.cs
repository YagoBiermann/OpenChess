
namespace OpenChess.Domain
{
    internal class Knight : Piece
    {
        public Knight(Color color, Coordinate origin, Chessboard chessboard) : base(color, origin, chessboard)
        {
        }

        public override char Name => Color == Color.Black ? 'n' : 'N';
        public override bool IsLongRange => false;

        public override List<MovePositions> CalculateLegalMoves()
        {
            List<MovePositions> legalMoves = new() { };
            List<MovePositions> moveRange = CalculateMoveRange();

            foreach (MovePositions move in moveRange)
            {
                List<Coordinate> currentPosition = move.Coordinates;
                bool isOutOfChessboard = !currentPosition.Any();
                if (isOutOfChessboard) { legalMoves.Add(new(move.Direction, currentPosition)); continue; };

                IReadOnlySquare currentSquare = Chessboard.GetReadOnlySquare(currentPosition.FirstOrDefault()!);
                if (!currentSquare.HasPiece) { legalMoves.Add(new(move.Direction, currentPosition)); continue; }

                bool isAllyPieceOrKing = !currentSquare.HasEnemyPiece(Color) || currentSquare.HasTypeOfPiece(typeof(King));
                if (isAllyPieceOrKing) { legalMoves.Add(new(move.Direction, new())); continue; };

                legalMoves.Add(new(move.Direction, currentPosition));
            }

            return legalMoves;
        }

        public override List<Direction> Directions
        {
            get
            {
                List<Direction> directions = new()
                {
                    new Direction(1,2),
                    new Direction(-1,2),
                    new Direction(1,-2),
                    new Direction(-1,-2),
                    new Direction(2,1),
                    new Direction(2,-1),
                    new Direction(-2,-1),
                    new Direction(-2,1)
                };

                return directions;
            }
        }
    }
}