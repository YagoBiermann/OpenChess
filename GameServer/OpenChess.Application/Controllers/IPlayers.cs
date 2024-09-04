namespace OpenChess.Application
{
    public interface IPlayers
    {
        Task Play(string origin, string destination);
        Task CreateMatch();
        Task<JoinMatchDTO> JoinMatch(JoinMatchDTO matchDTO);
        Task Resign(EndGameDTO resignDTO);
        Task Timeout(EndGameDTO resignDTO);
        Task<string> ErrorMessage(string message);
    }
}
