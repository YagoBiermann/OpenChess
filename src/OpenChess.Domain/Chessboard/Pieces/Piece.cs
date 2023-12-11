namespace OpenChess.Domain
{
    internal abstract class Piece : IReadOnlyPiece
    {
        public Coordinate Origin { get; set; }
        public Color Color { get; }
        public abstract char Name { get; }
        public abstract List<Direction> Directions { get; }
        public abstract bool IsLongRange { get; }
        private IReadOnlyChessboard _chessboard;

        public Piece(Color color, Coordinate origin, IReadOnlyChessboard chessboard)
        {
            Color = color;
            Origin = origin;
            _chessboard = chessboard;
        }

        public int MoveAmount
        {
            get { return IsLongRange ? 8 : 1; }
        }

        public virtual List<MoveDirections> CalculateMoveRange()
        {
            List<MoveDirections> moveRange = new();

            foreach (Direction direction in Directions)
            {
                List<Coordinate> coordinates = Coordinate.CalculateSequence(Origin, direction, MoveAmount);
                MoveDirections move = new(direction, coordinates);
                moveRange.Add(move);
            }

            return moveRange;
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

        protected IReadOnlyChessboard Chessboard { get { return _chessboard; } }
    }
}