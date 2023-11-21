namespace OpenChess.Domain
{
    internal readonly record struct Move
    {
        public Direction Direction { get; }
        public List<Coordinate> Coordinates { get; }

        public Move(Direction direction, List<Coordinate> coordinates)
        {
            Direction = direction;
            Coordinates = coordinates;
        }
    }
}