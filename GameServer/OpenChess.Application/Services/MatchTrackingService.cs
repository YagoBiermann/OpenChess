using StackExchange.Redis;

public class MatchTrackingService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public MatchTrackingService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    private string GetMatchConnectionsKey(string matchId) => $"match:{matchId}:connections";
    private string GetPlayerKey(string playerId) => $"player:{playerId}:match";

    public async Task JoinMatchAsync(string matchId, string playerId, string connectionId)
    {
        var matchConnectionKey = GetMatchConnectionsKey(matchId);
        var matchIdKey = GetPlayerKey(playerId);
        await _database.SetAddAsync(matchConnectionKey, connectionId);
        await _database.StringSetAsync(matchIdKey, playerId);
    }

    public async Task RemovePlayerFromMatchAsync(string matchId, string connectionId, string playerId)
    {
        var matchConnectionKey = GetMatchConnectionsKey(matchId);
        var matchIdKey = GetPlayerKey(playerId);
        await _database.SetRemoveAsync(matchConnectionKey, connectionId);
        await _database.KeyDeleteAsync(matchIdKey);

        var remainingConnections = await _database.SetLengthAsync(matchConnectionKey);
        if (remainingConnections == 0)
        {
            await _database.KeyDeleteAsync(matchConnectionKey);
        }
    }

    public async Task<string?> GetMatchIdFromPlayerAsync(string playerId)
    {
        var key = GetPlayerKey(playerId);
        var matchId = await _database.StringGetAsync(key);
        return matchId;
    }

    public async Task<bool> IsPlayerInMatchAsync(string playerId)
    {
        var key = GetPlayerKey(playerId);
        var value = await _database.StringGetAsync(key);
        return value.HasValue;
    }
}
