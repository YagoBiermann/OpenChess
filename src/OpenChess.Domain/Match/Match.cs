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

        public void Join(PlayerInfo playerInfo)
        {
            if (IsFull()) throw new MatchException("Match is full!");

            bool sameColor = GetPlayerByColor(playerInfo.Color) is not null;
            bool sameId = GetPlayerById(playerInfo.Id) is not null;
            if (sameColor) throw new MatchException($"Match already contains a player of same color!");
            if (sameId) throw new MatchException($"Player is already in the match!");

            Player player = CreateNewPlayer(playerInfo);
            player.Join(Id);
            _players.Add(player);
            if (!IsFull()) { return; };

            Player? whitePlayer = GetPlayerByColor(Color.White);
            _currentPlayer = whitePlayer;
            _status = MatchStatus.InProgress;
        }

        public bool IsFull()
        {
            return _players.Count == _players.Capacity;
        }

        public Player? GetPlayerByColor(Color color)
        {
            return _players.Where(p => p.Color == color).FirstOrDefault();
        }

        public Player? GetPlayerById(Guid id)
        {
            return _players.Where(p => p.Id == id).FirstOrDefault();
        }

        public MatchStatus Status { get { return _status; } }
        public Guid? CurrentPlayer { get { return _currentPlayer?.Id; } }
        public Time Time { get { return (Time)_time.Minutes; } }
        public Guid? Winner { get { return _winner?.Id; } }
        public Stack<string> Moves { get { return new Stack<string>(_pgn); } }
        public string Chessboard { get { return _chessboard.ToString(); } }
    }
}