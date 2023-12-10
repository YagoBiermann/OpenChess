namespace OpenChess.Domain
{
    internal readonly struct Castling
    {
        public bool HasWhiteKingSide { get; init; }
        public bool HasWhiteQueenSide { get; init; }
        public bool HasBlackKingSide { get; init; }
        public bool HasBlackQueenSide { get; init; }


        public Castling()
        {
            HasWhiteKingSide = true;
            HasWhiteQueenSide = true;
            HasBlackKingSide = true;
            HasBlackQueenSide = true;
        }

        public Castling(bool AvailabilityforAll)
        {
            HasWhiteKingSide = AvailabilityforAll;
            HasWhiteQueenSide = AvailabilityforAll;
            HasBlackKingSide = AvailabilityforAll;
            HasBlackQueenSide = AvailabilityforAll;
        }

        public Castling(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
        {
            HasWhiteKingSide = whiteKingSide;
            HasWhiteQueenSide = whiteQueenSide;
            HasBlackKingSide = blackKingSide;
            HasBlackQueenSide = blackQueenSide;
        }

        public override string ToString()
        {
            string castlingAvailability = "";
            if (HasWhiteKingSide) castlingAvailability += "K";
            if (HasWhiteQueenSide) castlingAvailability += "Q";
            if (HasBlackKingSide) castlingAvailability += "k";
            if (HasBlackQueenSide) castlingAvailability += "q";
            if (!HasWhiteKingSide && !HasWhiteQueenSide && !HasBlackKingSide && !HasBlackQueenSide) castlingAvailability += "-";

            return castlingAvailability;
        }
    }
}