namespace OpenChess.Domain
{
    internal interface IReadOnlyPiece
    {
        public Coordinate Origin { get; }
        public Color Color { get; }
        public char Name { get; }
        public List<Direction> Directions { get; }
        public bool IsLongRange { get; }
        public int MoveAmount { get; }
        public List<PieceRangeOfAttack> CalculateMoveRange();
    }
}