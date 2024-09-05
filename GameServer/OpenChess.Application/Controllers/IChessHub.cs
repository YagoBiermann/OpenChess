namespace OpenChess.Application
{
    public interface IChessHub
    {
        Task GameStatus(MatchDTO matchDTO);
        Task CreateMatch();
        Task<JoinMatchDTO> JoinMatch(JoinMatchDTO matchDTO);
        Task Resign(EndGameDTO resignDTO);
        Task Timeout(EndGameDTO resignDTO);
        Task<string> ErrorMessage(string message);
    }
}
