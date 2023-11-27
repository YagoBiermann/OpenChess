namespace OpenChess.Domain
{
    internal abstract class Piece
    {
        public Coordinate Origin { get; set; }
        public Color Color { get; }
        public abstract char Name { get; }
        public abstract List<Direction> Directions { get; }
        public abstract bool IsLongRange { get; }

        public Piece(Color color, Coordinate origin)
        {
            Color = color;
            Origin = origin;
        }

        public int MoveAmount
        {
            get { return IsLongRange ? 8 : 1; }
        }

        public virtual List<Move> CalculateMoveRange()
        {
            List<Move> moveRange = new();

            foreach (Direction direction in Directions)
            {
                List<Coordinate> coordinates = Coordinate.CalculateSequence(Origin, direction, MoveAmount);
                Move move = new(direction, coordinates);
                moveRange.Add(move);
            }

            return moveRange;
        }

        public virtual List<Move> CalculateLegalMoves(Chessboard chessboard)
        {
            List<Move> legalMoves = new();
            List<Move> moveRange = CalculateMoveRange();

            foreach (Move move in moveRange)
            {
                List<Coordinate> piecesPosition = chessboard.FindPieces(move.Coordinates);
                if (!piecesPosition.Any())
                {
                    legalMoves.Add(new(move.Direction, move.Coordinates));
                    continue;
                }
                PieceDistances nearestPiece = (PieceDistances)Coordinate.CalculateNearestDistance(Origin, piecesPosition)!;

                List<Coordinate> attackingRange = Coordinate.CalculateSequence(Origin, move.Direction, nearestPiece.DistanceFromOrigin);
                Square square = chessboard.GetSquare(nearestPiece.PiecePosition);
                bool isKing = square.HasTypeOfPiece(typeof(King));

                if (!square.HasEnemyPiece(Color) || isKing) attackingRange.RemoveAt(attackingRange.Count - 1);

                legalMoves.Add(new(move.Direction, attackingRange));
            }

            return legalMoves;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Piece otherPiece = (Piece)obj;
            return Origin.Equals(otherPiece.Origin) && Color.Equals(otherPiece.Color);
        }

        public override int GetHashCode()
        {
            return Origin.GetHashCode() ^ Color.GetHashCode();
        }

        public static Piece Create(char type, Coordinate origin)
        {
            Color color = char.IsUpper(type) ? Color.White : Color.Black;

            return char.ToUpper(type) switch
            {
                'K' => new King(color, origin),
                'Q' => new Queen(color, origin),
                'R' => new Rook(color, origin),
                'B' => new Bishop(color, origin),
                'N' => new Knight(color, origin),
                'P' => new Pawn(color, origin),
                _ => throw new PieceException($"{type} does not represent a piece"),
            };
        }
    }
}