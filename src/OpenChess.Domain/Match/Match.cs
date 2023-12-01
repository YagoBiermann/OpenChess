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
    }
}