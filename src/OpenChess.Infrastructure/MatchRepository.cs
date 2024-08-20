using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using OpenChess.Domain;
using StackExchange.Redis;

namespace OpenChess.Infrastructure
{
    internal class MatchRepository : IMatchRepository
    {
        IConnectionMultiplexer _connection;
        public MatchRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            _connection = connectionMultiplexer;
        }
        public MatchInfo GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task Save(MatchInfo matchInfo)
        {
            throw new NotImplementedException();
        }
    }
}