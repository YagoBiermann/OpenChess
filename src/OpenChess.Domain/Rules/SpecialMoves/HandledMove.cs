namespace OpenChess.Domain
{
    internal record HandledMove(IReadOnlyPiece PieceMoved, IReadOnlyPiece? PieceCaptured, MoveType moveType = MoveType.Default);
}