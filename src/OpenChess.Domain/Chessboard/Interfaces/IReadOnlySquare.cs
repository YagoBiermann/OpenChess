namespace OpenChess.Domain
{
    internal interface IReadOnlySquare
    {
        public IReadOnlyPiece? ReadOnlyPiece { get; }
        public Coordinate Coordinate { get; }
        public bool HasPiece { get; }
        public bool HasTypeOfPiece(Type piece);
        public bool HasEnemyPiece(Color currentPlayer);
    }
}