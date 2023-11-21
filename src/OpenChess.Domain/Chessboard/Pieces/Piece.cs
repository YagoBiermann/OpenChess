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