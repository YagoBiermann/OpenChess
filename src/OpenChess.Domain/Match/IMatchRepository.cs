namespace OpenChess.Domain
{
    internal interface IMatchRepository
    {
        public MatchInfo GetById(string id);
        public Task Save(MatchInfo matchInfo);
    }
}