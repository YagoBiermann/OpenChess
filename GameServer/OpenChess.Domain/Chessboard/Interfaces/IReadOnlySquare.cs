namespace OpenChess.Domain
{
    public interface IReadOnlySquare
    {
        public IReadOnlyPiece? ReadOnlyPiece { get; }
        public Coordinate Coordinate { get; }
        public Color Color { get; }
        public bool HasPiece { get; }
        public bool HasEnemyPiece(Color currentPlayer);
    }
}