namespace OpenChess.Domain
{
    internal readonly struct PieceDistances
    {
        public int DistanceFromOrigin { get; }
        public IReadOnlyPiece Piece { get; }
        public PieceDistances(int distanceBetween, IReadOnlyPiece piece)
        {
            DistanceFromOrigin = distanceBetween;
            Piece = piece;
        }

        public static int CalculateDistance(Coordinate origin, Coordinate position)
        {
            int rowDifference = Math.Abs(origin.RowToInt - position.RowToInt);
            int colDifference = Math.Abs(origin.ColumnToInt - position.ColumnToInt);
            return Math.Max(rowDifference, colDifference);
        }

        public static List<PieceDistances> CalculateDistance(IReadOnlyPiece pieceOfReference, List<IReadOnlyPiece> pieces)
        {
            List<PieceDistances> distances = new();
            foreach (IReadOnlyPiece piece in pieces)
            {
                int distance = CalculateDistance(pieceOfReference.Origin, piece.Origin);
                distances.Add(new(distance, piece));
            }
            distances.Sort((d1, d2) => d1.DistanceFromOrigin.CompareTo(d2.DistanceFromOrigin));

            return distances;
        }

        public static PieceDistances CalculateNearestDistance(List<PieceDistances> distances)
        {
            int minDistance = distances.Min(d => d.DistanceFromOrigin);
            PieceDistances nearestPiece = distances.Find(d => d.DistanceFromOrigin == minDistance);

            return nearestPiece;
        }
    }
}