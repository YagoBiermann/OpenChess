namespace OpenChess.Domain
{
    internal abstract class Piece
    {
        public Coordinate Origin { get; set; }
        public Color Color { get; }
        public abstract char Name { get; }
        public abstract List<Direction> Directions { get; }
        public abstract int MoveAmount { get; }

        public Piece(Color color, Coordinate origin)
        {
            Color = color;
            Origin = origin;
        }
    }
}