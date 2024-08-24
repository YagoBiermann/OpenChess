using StackExchange.Redis;

public class ConnectionTrackingService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    public ConnectionTrackingService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    private string GetPlayerConnectionsKey(string playerId) => $"player:{playerId}:connections";

    public async Task AddConnectionAsync(string playerId, string connectionId)
    {
        var key = GetPlayerConnectionsKey(playerId);
        await _database.SetAddAsync(key, connectionId);
    }

    public async Task RemoveConnectionAsync(string playerId, string connectionId)
    {
        var key = GetPlayerConnectionsKey(playerId);
        await _database.SetRemoveAsync(key, connectionId);

        var remainingConnections = await _database.SetLengthAsync(key);
        if (remainingConnections == 0)
        {
            await _database.KeyDeleteAsync(key);
        }
    }
    public async Task<List<string>> GetPlayerConnectionsAsync(string playerId)
    {
        var key = GetPlayerConnectionsKey(playerId);
        var connections = await _database.SetMembersAsync(key);
        return connections.Select(c => (string)c).ToList();
    }

    public async Task<bool> IsPlayerConnectedAsync(string playerId)
    {
        var key = GetPlayerConnectionsKey(playerId);
        return await _database.KeyExistsAsync(key) && await _database.SetLengthAsync(key) > 0;
    }
}
