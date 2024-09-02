namespace OpenChess.Domain
{
    public readonly struct MatchDTO(string matchId, List<PlayerDTO> playerData, string fen, string status, string? winner)
    {
        public string MatchId { get; } = matchId;
        public List<PlayerDTO> Players { get; } = playerData;
        public string Fen { get; } = fen;
        public string Status { get; } = status;
        public string? WinnerId { get; } = winner;
        public Dictionary<string, List<string>> Moves { get; }
        public string LastMove { get; }
    }
}