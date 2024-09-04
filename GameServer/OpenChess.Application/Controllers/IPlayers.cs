namespace OpenChess.Application
{
    public interface IPlayers
    {
        Task Play(string origin, string destination);
        Task CreateMatch();
        Task<JoinMatchDTO> JoinMatch(JoinMatchDTO matchDTO);
        Task Resign(ResignDTO resignDTO);
        Task<string> ErrorMessage(string message);
    }
}
