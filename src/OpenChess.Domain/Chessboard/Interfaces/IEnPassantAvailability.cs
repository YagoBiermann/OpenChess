namespace OpenChess.Domain
{
    internal interface IEnPassantAvailability
    {
        Coordinate? EnPassantPosition { get; }
        bool IsAvailable { get; }
    }
}