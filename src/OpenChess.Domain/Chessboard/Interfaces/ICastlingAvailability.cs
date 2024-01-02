namespace OpenChess.Domain
{
    internal interface ICastlingAvailability
    {
        public bool IsWhiteKingSideAvailable { get; }
        public bool IsWhiteQueenSideAvailable { get; }
        public bool IsBlackKingSideAvailable { get; }
        public bool IsBlackQueenSideAvailable { get; }
        public void UpdateAvailability(Coordinate origin, Color player);
    }
}