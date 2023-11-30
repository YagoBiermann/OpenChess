namespace OpenChess.Domain
{
    internal class Match
    {
        Guid Id { get; }
        private List<Player> _players = new(2);
        private Chessboard _chessboard { get; }
        private Player _currentPlayer { get; set; }
        private Stack<string> _pgn { get; }
        private MatchStatus _status { get; set; }
        private Player? _winner { get; set; }
    }
}