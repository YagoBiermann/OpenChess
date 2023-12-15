namespace OpenChess.Domain
{
    internal record MovePlayed(IReadOnlyPiece PieceMoved, IReadOnlyPiece? PieceCaptured, MoveType MoveType = MoveType.DefaultMove);
}