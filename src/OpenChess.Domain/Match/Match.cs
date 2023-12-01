namespace OpenChess.Domain
{
    internal class Match
    {
        public Guid Id { get; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; }
        private Player? _currentPlayer { get; set; }
        private Stack<string> _pgn = new();
        private MatchStatus _status { get; set; }
        private Player? _winner { get; set; }
        private TimeSpan _time { get; }

        public Match(Time time)
        {
            Id = Guid.NewGuid();
            _chessboard = new(FEN.InitialPosition);
            _currentPlayer = null;
            _status = MatchStatus.NotStarted;
            _winner = null;
            _time = TimeSpan.FromMinutes((int)time);
        }

        public void Join(Player player)
        {
            if (IsFull()) throw new MatchException("Match is full!");
            bool sameColor = GetPlayerBy(player.Color) is not null;
            if (sameColor) throw new MatchException($"Match already contains a player of same color!");

            _players.Add(player);
            if (!IsFull()) { return; };

            Player? whitePlayer = GetPlayerBy(Color.White);
            _currentPlayer = whitePlayer;
            _status = MatchStatus.InProgress;
        }

        public bool IsFull()
        {
            return _players.Count == _players.Capacity;
        }

        public Player? GetPlayerBy(Color color)
        {
            return _players.Where(p => p.Color == color).FirstOrDefault();
        }

        public MatchStatus Status { get { return _status; } }
        public Guid? CurrentPlayer { get { return _currentPlayer?.Id; } }
        public Time Time { get { return (Time)_time.Minutes; } }
        public Guid? Winner { get { return _winner?.Id; } }
        public Stack<string> Moves { get { return new Stack<string>(_pgn); } }
        public string Chessboard { get { return _chessboard.ToString(); } }
    }
}