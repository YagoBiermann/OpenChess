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

        public bool IsHittingTheEnemyKing(Chessboard chessboard)
        {
            List<Move> moveRange = CalculateMoveRange();
            bool isHitting = false;

            foreach (Move move in moveRange)
            {
                if (this is Pawn pawn && move.Direction.Equals(pawn.ForwardDirection))
                {
                    continue;
                }

                List<Coordinate> pieces = chessboard.FindPieces(move.Coordinates);
                if (!pieces.Any()) continue;
                List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(Origin, pieces);
                CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances)!;
                Square square = chessboard.GetSquare(nearestPiece.Position);
                if (square.HasEnemyPiece(Color) && square.HasTypeOfPiece(typeof(King))) { isHitting = true; break; }
            }

            return isHitting;
        }

        public virtual List<Move> CalculateLegalMoves(Chessboard chessboard)
        {
            List<Move> legalMoves = new();
            List<Move> moveRange = CalculateMoveRange();

            foreach (Move move in moveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> pieces = chessboard.FindPieces(move.Coordinates);
                if (!pieces.Any())
                {
                    legalMoves.Add(move);
                    continue;
                }

                List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(Origin, pieces);
                CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);

                List<Coordinate> rangeOfAttack = move.Coordinates.Take(nearestPiece.DistanceBetween).ToList();
                Square square = chessboard.GetSquare(nearestPiece.Position);
                bool isKing = square.HasTypeOfPiece(typeof(King));

                int lastPosition = rangeOfAttack.Count - 1;
                if (!square.HasEnemyPiece(Color) || isKing) rangeOfAttack.RemoveAt(lastPosition);

                legalMoves.Add(new(currentDirection, rangeOfAttack));
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