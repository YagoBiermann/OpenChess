namespace OpenChess.Domain
{
    internal readonly struct MatchInfo
    {
        public Guid MatchId { get; }
        public List<PlayerInfo> Players { get; }
        public string Fen { get; }
        public Stack<string> PgnMoves { get; }
        public MatchStatus Status { get; }
        public Time Time { get; }
        public Guid? WinnerId { get; } = null;
        public DateTime CurrentTurnStartedAt { get; }
        public DateTime CreatedAt { get; }

        public MatchInfo(string matchId, List<PlayerInfo> players, string fen, Stack<string> pgnMoves, string status, int time, string currentTurnStartedAt, string createdAt, string? winnerId = null)
        {
            MatchId = Match.TryParseId(matchId);
            Players = players;
            PgnMoves = pgnMoves;
            bool wasParsed = Enum.TryParse(status, out MatchStatus result);
            if (!wasParsed) throw new MatchException($"The string: {status} is not a valid status");
            Status = result;
            Fen = fen;

            if (!Enum.IsDefined(typeof(Time), time)) { throw new MatchException($"The given time {time} is not valid"); }
            Time = (Time)time;
            CurrentTurnStartedAt = DateTime.Parse(currentTurnStartedAt);
            CreatedAt = DateTime.Parse(createdAt);
            if (winnerId is null) return;
            WinnerId = Match.TryParseId(winnerId!);
        }
    }
}