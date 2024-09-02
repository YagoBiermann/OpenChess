using OpenChess.Domain;
using Redis.OM;
using StackExchange.Redis;

namespace OpenChess.Application
{
    public class IndexCreationService(RedisConnectionProvider redis) : IHostedService
    {
        private readonly RedisConnectionProvider _redis = redis;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await _redis.Connection.CreateIndexAsync(typeof(MatchPersistenceModel));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}