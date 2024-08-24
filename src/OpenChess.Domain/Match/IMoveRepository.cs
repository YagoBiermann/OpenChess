namespace OpenChess.Domain
{
    internal interface IMoveRepository
    {
        public Task<string?> GetById(string id);
        public Task Save(string id);
    }
}