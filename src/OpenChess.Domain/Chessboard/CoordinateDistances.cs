namespace OpenChess.Domain
{
    internal readonly struct CoordinateDistances
    {
        public int DistanceBetween { get; }
        public IReadOnlyPiece Piece { get; }
        public CoordinateDistances(int distanceBetween, IReadOnlyPiece piece)
        {
            DistanceBetween = distanceBetween;
            Piece = piece;
        }

        public static int CalculateDistance(Coordinate origin, Coordinate position)
        {
            int rowDifference = Math.Abs(origin.RowToInt - position.RowToInt);
            int colDifference = Math.Abs(origin.ColumnToInt - position.ColumnToInt);
            return Math.Max(rowDifference, colDifference);
        }

        public static List<CoordinateDistances> CalculateDistance(Coordinate origin, List<IReadOnlyPiece> pieces)
        {
            List<CoordinateDistances> distances = new();
            foreach (IReadOnlyPiece piece in pieces)
            {
                int distance = CalculateDistance(origin, piece.Origin);
                distances.Add(new(distance, piece));
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