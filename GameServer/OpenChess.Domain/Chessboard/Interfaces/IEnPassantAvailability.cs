namespace OpenChess.Domain
{
    public interface IEnPassantAvailability
    {
        Coordinate? EnPassantPosition { get; }
        public void ClearEnPassant();
        public void SetVulnerablePawn(IReadOnlyPiece? piece, Coordinate previousOrigin);
        bool IsAvailable { get; }
    }
}