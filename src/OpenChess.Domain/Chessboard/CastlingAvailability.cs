namespace OpenChess.Domain
{
    internal struct CastlingAvailability : ICastlingAvailability
    {
        public bool IsWhiteKingSideAvailable { get; private set; }
        public bool IsWhiteQueenSideAvailable { get; private set; }
        public bool IsBlackKingSideAvailable { get; private set; }
        public bool IsBlackQueenSideAvailable { get; private set; }

        public Dictionary<char, bool> IsAvailableAt { get; private set; }
        public static char WhiteKingSide = 'K';
        public static char WhiteQueenSide = 'Q';
        public static char BlackKingSide = 'k';
        public static char BlackQueenSide = 'q';
        public CastlingAvailability()
        {
            IsAvailableAt = new()
            {
                {'K', true},
                {'Q', true},
                {'k', true},
                {'q', true},
            };

            IsWhiteKingSideAvailable = true;
            IsWhiteQueenSideAvailable = true;
            IsBlackKingSideAvailable = true;
            IsBlackQueenSideAvailable = true;
        }

        public CastlingAvailability(Dictionary<char, bool> castlingAvailability)
        {
            IsAvailableAt = castlingAvailability;
        }

        public CastlingAvailability(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
        {
            IsWhiteKingSideAvailable = whiteKingSide;
            IsWhiteQueenSideAvailable = whiteQueenSide;
            IsBlackKingSideAvailable = blackKingSide;
            IsBlackQueenSideAvailable = blackQueenSide;
        }

        public void UpdateAvailability(Coordinate origin, Color player)
        {
            if (player is Color.White && origin.Equals(Coordinate.GetInstance("E1"))) { IsWhiteKingSideAvailable = false; IsWhiteQueenSideAvailable = false; }
            if (player is Color.White && origin.Equals(Coordinate.GetInstance("A1"))) { IsWhiteQueenSideAvailable = false; };
            if (player is Color.White && origin.Equals(Coordinate.GetInstance("H1"))) { IsWhiteKingSideAvailable = false; };
            if (player is Color.Black && origin.Equals(Coordinate.GetInstance("E8"))) { IsBlackKingSideAvailable = false; IsBlackQueenSideAvailable = false; }
            if (player is Color.Black && origin.Equals(Coordinate.GetInstance("A8"))) { IsBlackQueenSideAvailable = false; };
            if (player is Color.Black && origin.Equals(Coordinate.GetInstance("H8"))) { IsBlackKingSideAvailable = false; };
        }

        public override string ToString()
        {
            string castlingAvailability = "";
            if (IsWhiteKingSideAvailable) castlingAvailability += "K";
            if (IsWhiteQueenSideAvailable) castlingAvailability += "Q";
            if (IsBlackKingSideAvailable) castlingAvailability += "k";
            if (IsBlackQueenSideAvailable) castlingAvailability += "q";
            if (!IsWhiteKingSideAvailable && !IsWhiteQueenSideAvailable && !IsBlackKingSideAvailable && !IsBlackQueenSideAvailable) castlingAvailability += "-";

            return castlingAvailability;
        }
    }
}