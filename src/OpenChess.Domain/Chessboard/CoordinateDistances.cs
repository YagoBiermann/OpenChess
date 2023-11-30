namespace OpenChess.Domain
{
    internal readonly struct CoordinateDistances
    {
        public Coordinate Origin { get; }
        public int DistanceBetween { get; }
        public Coordinate Position { get; }
        public CoordinateDistances(int distanceBetween, Coordinate origin, Coordinate position)
        {
            Origin = origin;
            DistanceBetween = distanceBetween;
            Position = position;
        }

        public static int CalculateDistance(Coordinate origin, Coordinate position)
        {
            int rowDifference = Math.Abs(origin.RowToInt - position.RowToInt);
            int colDifference = Math.Abs(origin.ColumnToInt - position.ColumnToInt);
            return Math.Max(rowDifference, colDifference);
        }

        public static List<CoordinateDistances> CalculateDistance(Coordinate origin, List<Coordinate> positions)
        {
            List<CoordinateDistances> distances = new();
            foreach (Coordinate position in positions)
            {
                int distance = CalculateDistance(origin, position);
                distances.Add(new(distance, origin, position));
            }
            return distances;
        }

        public static CoordinateDistances CalculateNearestDistance(List<CoordinateDistances> distances)
        {
            int minDistance = distances.Min(d => d.DistanceBetween);
            CoordinateDistances nearestPiece = distances.Find(d => d.DistanceBetween == minDistance);

            return nearestPiece;
        }
    }
}