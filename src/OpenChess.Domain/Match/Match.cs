namespace OpenChess.Domain
{
    internal class Match
    {
        public Guid Id { get; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; }
        private Stack<string> _pgn { get; }
        private Player? _currentPlayer { get; set; }
        private MatchStatus _status { get; set; }
        private Player? _winner { get; set; }
    }
}