namespace OpenChess.Domain
{
    internal readonly record struct MoveDirections
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate> Coordinates { get; }

        public MoveDirections(Direction direction, List<Coordinate> coordinates, IReadOnlyPiece piece)
        {
            Direction = direction;
            Coordinates = coordinates;
            Piece = piece;
        }
    }
}