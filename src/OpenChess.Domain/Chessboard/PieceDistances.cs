namespace OpenChess.Domain
{
    internal readonly struct PieceDistances
    {
        public int DistanceFromOrigin { get; }
        public Coordinate PiecePosition { get; }
        public PieceDistances(int distance, Coordinate piece)
        {
            DistanceFromOrigin = distance;
            PiecePosition = piece;
        }

        public static PieceDistances FindNearest(List<PieceDistances> piecePositions)
        {
            int minDistance = piecePositions.Min(d => d.DistanceFromOrigin);
            PieceDistances nearestPiece = piecePositions.Find(d => d.DistanceFromOrigin == minDistance);
            return nearestPiece;
        }
    }
}