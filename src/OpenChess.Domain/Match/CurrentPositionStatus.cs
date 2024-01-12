namespace OpenChess.Domain
{
    enum CurrentPositionStatus
    {
        NotInCheck,
        Check,
        DoubleCheck,
        Checkmate,
        Draw,
        Timeout,
    }
}