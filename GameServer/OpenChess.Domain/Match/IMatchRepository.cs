namespace OpenChess.Domain
{
    public interface IMatchRepository
    {
        public Task<IMatch?> GetById(string id);
        public Task Create(IMatch match);
        public Task Update(IMatch match);
    }
}