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
    }
}