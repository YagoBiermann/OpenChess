namespace OpenChess.Domain
{
    internal abstract class Piece : IReadOnlyPiece
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
    }
}