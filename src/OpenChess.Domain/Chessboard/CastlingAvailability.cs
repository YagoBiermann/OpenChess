namespace OpenChess.Domain
{
    internal struct CastlingAvailability : ICastlingAvailability
    {
        private Dictionary<string, List<char>> _castlingPiecesPosition { get; }
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
        }

        public CastlingAvailability(Dictionary<char, bool> castlingAvailability)
        {
            IsAvailableAt = castlingAvailability;
            _castlingPiecesPosition = new()
            {
                {"E1", new() {'K','Q'}},
                {"A1", new() {'Q'}},
                {"H1", new() {'K'}},
                {"E8", new() {'k', 'q'}},
                {"H8", new() {'k'}},
                {"A8", new() {'q'}},
            };
        }

        public void UpdateAvailability(Coordinate origin, Color player)
        {
            Dictionary<char, bool> newCastlingAvailability = IsAvailableAt;
            _castlingPiecesPosition[origin.ToString()].ForEach(v => { newCastlingAvailability[v] = false; });
            IsAvailableAt = newCastlingAvailability;
        }

        public override readonly string ToString()
        {
            string castlingAvailability = "";

            foreach (var kv in IsAvailableAt)
            {
                bool isAvailable = kv.Value;
                char castling = kv.Key;
                if (isAvailable) { castlingAvailability += castling; }
            }

            return castlingAvailability;
        }
    }
}