namespace OpenChess.Domain
{
    internal enum MoveType
    {
        DefaultMove,
        QueenSideCastlingMove,
        KingSideCastlingMove,
        PawnPromotionMove,
        EnPassantMove,
        PawnMove,
    }
}