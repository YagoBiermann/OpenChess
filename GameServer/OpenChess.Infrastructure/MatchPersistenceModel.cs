using Redis.OM.Modeling;

namespace OpenChess.Domain
{
    [Document(StorageType = StorageType.Json, Prefixes = ["Matches"])]
    public class MatchPersistenceModel(string matchId, List<PlayerPersistenceModel> players, string fen, List<string> pgnMoves, string status, int time, string? winnerId, string currentTurnStartedAt, string createdAt)
    {
        [Indexed]
        [RedisIdField]
        public string MatchId { get; set; } = matchId;
        [Indexed(CascadeDepth = 1)]
        public List<PlayerPersistenceModel> Players { get; set; } = players;
        public string Fen { get; set; } = fen;
        public List<string> PgnMoves { get; set; } = pgnMoves;
        public string Status { get; set; } = status;
        public int Time { get; set; } = time;
        public string? WinnerId { get; set; } = winnerId;
        public string CurrentTurnStartedAt { get; set; } = currentTurnStartedAt;
        public string CreatedAt { get; set; } = createdAt;
    }
}