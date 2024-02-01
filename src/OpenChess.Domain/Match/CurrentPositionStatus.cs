namespace OpenChess.Domain
{
    internal enum CurrentPositionStatus
    {
        NotInCheck,
        Check,
        DoubleCheck,
        Checkmate,
        Draw,
        Timeout,
        Undefined,
    }
}