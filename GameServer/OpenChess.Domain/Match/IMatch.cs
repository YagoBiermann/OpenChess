namespace OpenChess.Domain
{
    public interface IMatch
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }
        public int HalfMove { get; }
        public int FullMove { get; }
        public IReadOnlyList<IReadOnlyPlayer> Players { get; }
        public IReadOnlyList<string> PgnMoves { get; }
        public CurrentPositionStatus CurrentPositionStatus { get; }
        public DateTime CurrentTurnStartedAt { get; }
        public string Fen { get; }
        public bool HasNotStarted();
        public bool HasStarted();
        public bool HasFinished();
        public MatchStatus Status { get; }
        public IReadOnlyPlayer? CurrentPlayerInfo { get; }
        public IReadOnlyPlayer? OpponentPlayerInfo { get; }
        public Color? CurrentPlayerColor { get; }
        public Color? OpponentPlayerColor { get; }
        public Time Duration { get; }
        public Color? Winner { get; }
        public void FinishWithTimeout(string? playerId);
        public void FinishWithResign(string playerId);
        public void Play(Move move);
        public void Join(string playerId, int color);
        public IReadOnlyChessboard Chessboard { get; }
    }
}