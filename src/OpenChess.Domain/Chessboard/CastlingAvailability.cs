namespace OpenChess.Domain
{
    internal struct CastlingAvailability
    {
        public bool HasWhiteKingSide { get; set; }
        public bool HasWhiteQueenSide { get; set; }
        public bool HasBlackKingSide { get; set; }
        public bool HasBlackQueenSide { get; set; }

        public CastlingAvailability()
        {
            HasWhiteKingSide = true;
            HasWhiteQueenSide = true;
            HasBlackKingSide = true;
            HasBlackQueenSide = true;
        }

        public CastlingAvailability(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
        {
            HasWhiteKingSide = whiteKingSide;
            HasWhiteQueenSide = whiteQueenSide;
            HasBlackKingSide = blackKingSide;
            HasBlackQueenSide = blackQueenSide;
        }

        public void UpdateAvailability(Coordinate origin, Color player)
        {
            if (player is Color.White && origin.Equals(Coordinate.GetInstance("E1"))) { HasWhiteKingSide = false; HasWhiteQueenSide = false; }
            if (player is Color.White && origin.Equals(Coordinate.GetInstance("A1"))) { HasWhiteQueenSide = false; };
            if (player is Color.White && origin.Equals(Coordinate.GetInstance("H1"))) { HasWhiteKingSide = false; };
            if (player is Color.Black && origin.Equals(Coordinate.GetInstance("E8"))) { HasBlackKingSide = false; HasBlackQueenSide = false; }
            if (player is Color.Black && origin.Equals(Coordinate.GetInstance("A8"))) { HasBlackQueenSide = false; };
            if (player is Color.Black && origin.Equals(Coordinate.GetInstance("H8"))) { HasBlackKingSide = false; };
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