using StackExchange.Redis;

public class MoveTrackingService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _database;
    private string GetMovesKey(string matchId) => $"match:{matchId}:moves";

    public MoveTrackingService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _database = _redis.GetDatabase();
    }

    public async Task<string?> GetMoveAsync(string matchId, string moveId)
    {
        var exists = await _database.SetContainsAsync(GetMovesKey(matchId), moveId);
        if (!exists) return null;

        return moveId;
    }

    public async Task AddMoveAsync(string matchId, string moveId)
    {
        var key = GetMovesKey(matchId);
        var tran = _database.CreateTransaction();
        tran.AddCondition(Condition.KeyExists(key));
        _ = tran.KeyDeleteAsync(key);
        _ = tran.SetAddAsync(key, moveId);
        await tran.ExecuteAsync();
    }
}
