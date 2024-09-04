namespace OpenChess.Application
{
    public interface IPlayers
    {
        Task Play(string origin, string destination);
        Task CreateMatch();
        Task JoinMatch();
        Task Resign();
        Task<string> ErrorMessage(string message);
    }
}
