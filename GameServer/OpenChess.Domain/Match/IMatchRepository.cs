namespace OpenChess.Domain
{
    public interface IMatchRepository
    {
        public Task<MatchInfo?> GetById(string id);
        public Task Create(MatchInfo matchInfo);
        public Task Update(MatchInfo matchInfo);
    }
}