namespace OpenChess.Application
{
    public readonly struct JoinMatchDTO(string matchId, string fen, char color, int time)
    {
        public string MatchId { get; } = matchId;
        public string Fen { get; } = fen;
        public char Color { get; } = color;
        public int Time { get; } = time;
    }
}