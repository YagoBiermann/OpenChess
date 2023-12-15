namespace OpenChess.Domain
{
    internal readonly record struct MoveDirections
    {
        public Direction Direction { get; }
        public List<Coordinate> Coordinates { get; }

        public MoveDirections(Direction direction, List<Coordinate> coordinates)
        {
            Direction = direction;
            Coordinates = coordinates;
        }
    }
}