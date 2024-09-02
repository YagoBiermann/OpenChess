using Redis.OM.Modeling;

namespace OpenChess.Domain
{
    [Document(StorageType = StorageType.Json, Prefixes = ["Players"])]
    public class PlayerPersistenceModel(string id, char color, string currentMatch, long timeRemaining)
    {
        [Indexed(PropertyName = "PlayerId")]
        [RedisIdField]
        public string Id { get; set; } = id;
        public char Color { get; set; } = color;
        public string CurrentMatch { get; set; } = currentMatch;
        public long TimeRemaining { get; set; } = timeRemaining;
    }
}