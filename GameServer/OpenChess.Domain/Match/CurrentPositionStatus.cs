namespace OpenChess.Domain
{
    public enum CurrentPositionStatus
    {
        NotInCheck,
        Check,
        DoubleCheck,
        Checkmate,
        Draw,
        Timeout,
        Undefined,
        Resign
    }
}