namespace OpenChess.Domain
{
    internal interface ICastlingAvailability
    {
        public bool HasWhiteKingSide { get; }
        public bool HasWhiteQueenSide { get; }
        public bool HasBlackKingSide { get; }
        public bool HasBlackQueenSide { get; }
    }
}