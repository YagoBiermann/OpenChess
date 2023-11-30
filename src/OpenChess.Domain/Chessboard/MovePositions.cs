namespace OpenChess.Domain
{
    internal readonly record struct MovePositions
    {
        public Direction Direction { get; }
        public List<Coordinate> Coordinates { get; }

        public MovePositions(Direction direction, List<Coordinate> coordinates)
        {
            Direction = direction;
            Coordinates = coordinates;
        }
    }
}