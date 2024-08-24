namespace OpenChess.Domain
{
    internal interface ICastlingAvailability
    {
        public Dictionary<char, bool> IsAvailableAt { get; }
        public void UpdateAvailability(Coordinate origin, Color player);
    }
}