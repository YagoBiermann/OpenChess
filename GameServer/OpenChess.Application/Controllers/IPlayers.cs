namespace OpenChess.Application
{
    public interface IPlayers
    {
        Task Play(string origin, string destination);
        Task CreateMatch();
        Task Resign();
        Task<JoinMatchDTO> JoinMatch(JoinMatchDTO matchDTO);
        Task<string> ErrorMessage(string message);
    }
}
