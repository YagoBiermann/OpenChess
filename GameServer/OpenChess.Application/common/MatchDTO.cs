namespace OpenChess.Application
{
    public readonly struct MatchDTO(string matchId, List<PlayerDTO> playerData, string fen, string status, char? winner)
    {
        public string MatchId { get; } = matchId;
        public List<PlayerDTO> Players { get; } = playerData;
        public string Fen { get; } = fen;
        public string Status { get; } = status;
        public char? Winner { get; } = winner;
        public Dictionary<string, List<string>> AvailableMoves { get; }
        public string LastMove { get; }
    }
}