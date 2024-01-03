namespace OpenChess.Domain
{
    internal struct CastlingAvailability : ICastlingAvailability
    {
        private Dictionary<string, List<char>> _castlingPiecesPosition = new()
        {
            {"E1", new() {'K','Q'}},
            {"A1", new() {'Q'}},
            {"H1", new() {'K'}},
            {"E8", new() {'k', 'q'}},
            {"H8", new() {'k'}},
            {"A8", new() {'q'}},
        };
        public Dictionary<char, bool> IsAvailableAt { get; private set; } = new()
        {
            {'K', true},
            {'Q', true},
            {'k', true},
            {'q', true},
        };
        public static char WhiteKingSide = 'K';
        public static char WhiteQueenSide = 'Q';
        public static char BlackKingSide = 'k';
        public static char BlackQueenSide = 'q';
        public CastlingAvailability() { }

        public CastlingAvailability(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
        {
            IsAvailableAt['K'] = whiteKingSide;
            IsAvailableAt['Q'] = whiteQueenSide;
            IsAvailableAt['k'] = blackKingSide;
            IsAvailableAt['q'] = blackQueenSide;
        }

        public void UpdateAvailability(Coordinate origin, Color player)
        {
            Dictionary<char, bool> newCastlingAvailability = IsAvailableAt;
            _castlingPiecesPosition[origin.ToString()].ForEach(v => { newCastlingAvailability[v] = false; });
            IsAvailableAt = newCastlingAvailability;
        }
    }
}