using OpenChess.Domain;
using Redis.OM;
using Redis.OM.Searching;

namespace OpenChess.Infrastructure
{
    internal class MatchRepository : IMatchRepository
    {
        IRedisCollection<MatchPersistenceModel> _matchCollection;
        public MatchRepository(RedisConnectionProvider provider)
        {
            _matchCollection = provider.RedisCollection<MatchPersistenceModel>();
        }

        public async Task Update(IMatch match)
        {
            MatchPersistenceModel matchPersistenceModel = MatchPersistenceMapper.ToPersistenceModel(match);
            await _matchCollection.UpdateAsync(matchPersistenceModel);
        }
        public async Task Create(IMatch match)
        {
            MatchPersistenceModel matchPersistenceModel = MatchPersistenceMapper.ToPersistenceModel(match);
            await _matchCollection.InsertAsync(matchPersistenceModel);
        }

        public async Task<IMatch?> GetById(string id)
        {
            MatchPersistenceModel? matchRawData = await _matchCollection.FindByIdAsync(id);
            if (matchRawData is null) return null;
            IMatch match = MatchPersistenceMapper.ToMatch(matchRawData);

            return match;
        }
    }
}