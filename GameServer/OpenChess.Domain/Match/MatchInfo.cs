namespace OpenChess.Domain
{
    public readonly struct MatchInfo
    {
        public Guid MatchId { get; }
        public List<PlayerInfo> Players { get; }
        public string Fen { get; }
        public List<string> PgnMoves { get; }
        public Status Status { get; }
        public Time Time { get; }
        public Color? Winner { get; } = null;
        public DateTime CurrentTurnStartedAt { get; }
        public DateTime CreatedAt { get; }

        public MatchInfo(string matchId, List<PlayerInfo> players, string fen, List<string> pgnMoves, string status, int time, string currentTurnStartedAt, string createdAt, char? winner = null)
        {
            MatchId = Match.TryParseId(matchId);
            Players = players;
            PgnMoves = pgnMoves;
            bool isTurnParsed = DateTime.TryParse(currentTurnStartedAt, out DateTime parsedCurrentTurnStartedAt);
            bool isCreatedAtParsed = DateTime.TryParse(createdAt, out DateTime parsedCreatedAt);

            if (!isTurnParsed) throw new MatchException($"The string '{currentTurnStartedAt}' is not a valid datetime.");
            if (!isCreatedAtParsed) throw new MatchException($"The string '{CreatedAt}' is not a valid datetime.");
            Status = (Status)status;
            Fen = fen;

            Time = new Time(time);
            CurrentTurnStartedAt = parsedCurrentTurnStartedAt;
            CreatedAt = parsedCreatedAt;
            if (winner is null) return;
            Winner = ColorUtils.TryParseColor((char)winner);
        }
    }
}