namespace OpenChess.Domain
{
    internal interface IReadOnlySquare
    {
        public IReadOnlyPiece? ReadOnlyPiece { get; }
        public Coordinate Coordinate { get; }
        public bool HasPiece { get; }
        public bool HasEnemyPiece(Color currentPlayer);
    }
}