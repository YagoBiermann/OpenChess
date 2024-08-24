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

        public async Task<MatchInfo?> GetById(string id)
        {
            var loadedMatch = await _connection.GetDatabase().StringGetAsync(id);
            if (!loadedMatch.HasValue) return null;
            var restoredMatch = JsonConvert.DeserializeObject<MatchInfo>(loadedMatch!);
            return restoredMatch;
        }

        public async Task Create(MatchInfo matchInfo)
        {
            var match = await _connection.GetDatabase().StringGetAsync(matchInfo.MatchId.ToString());
            if (match.HasValue) throw new MatchException("Match already exists!");
            await Update(matchInfo);
        }

        public async Task Update(MatchInfo matchInfo)
        {
            var converter = new StringEnumConverter();
            string matchJSON = JsonConvert.SerializeObject(matchInfo, converter);
            Console.WriteLine(matchJSON);
            bool isSaved = await _connection.GetDatabase().StringSetAsync(matchInfo.MatchId.ToString(), matchJSON);
            if (!isSaved)
            {
                throw new Exception("Failed to save match data.");
            }
        }
    }
}