namespace OpenChess.Domain
{
    internal readonly struct CastlingAvailability
    {
        public bool WhiteKingSide { get; init; }
        public bool WhiteQueenSide { get; init; }
        public bool BlackKingSide { get; init; }
        public bool BlackQueenSide { get; init; }


        public CastlingAvailability()
        {
            WhiteKingSide = true;
            WhiteQueenSide = true;
            BlackKingSide = true;
            BlackQueenSide = true;
        }
        
        public CastlingAvailability(bool AvailabilityforAll)
        {
            WhiteKingSide = AvailabilityforAll;
            WhiteQueenSide = AvailabilityforAll;
            BlackKingSide = AvailabilityforAll;
            BlackQueenSide = AvailabilityforAll;
        }

        public CastlingAvailability(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
        {
            WhiteKingSide = whiteKingSide;
            WhiteQueenSide = whiteQueenSide;
            BlackKingSide = blackKingSide;
            BlackQueenSide = blackQueenSide;
        }
    }
}