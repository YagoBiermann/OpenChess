namespace OpenChess.Domain
{
    public enum GameStatus
    {
        NotStarted,
        NotInCheck,
        Check,
        Checkmate,
        Draw,
        Timeout,
        Resign
    }
}