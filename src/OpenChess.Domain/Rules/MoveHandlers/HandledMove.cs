namespace OpenChess.Domain
{
    internal record HandledMove(IReadOnlyPiece PieceMoved, IReadOnlyPiece? PieceCaptured, MoveType MoveType = MoveType.DefaultMove);
}