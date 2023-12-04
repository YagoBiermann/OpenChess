namespace OpenChess.Domain
{
    internal interface IReadOnlyPiece
    {
        public Coordinate Origin { get; set; }
        public Color Color { get; }
        public char Name { get; }
        public List<Direction> Directions { get; }
        public bool IsLongRange { get; }
        public int MoveAmount { get; }
        public List<MoveDirections> CalculateMoveRange();
        public bool IsHittingTheEnemyKing();
        public List<MoveDirections> CalculateLegalMoves();
    }
}