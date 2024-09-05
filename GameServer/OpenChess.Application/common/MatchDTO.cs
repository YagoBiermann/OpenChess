namespace OpenChess.Application
{
    public readonly struct MatchDTO(string timeRemaining, string fen, string status, char? winner)
    {
        public string TimeRemaining { get; } = timeRemaining;
        public string Fen { get; } = fen;
        public string Status { get; } = status;
        public char? Winner { get; } = winner;
        public Dictionary<string, List<string>> AvailableMoves { get; }
        public string LastMove { get; }
    }
}