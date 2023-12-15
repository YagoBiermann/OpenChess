namespace OpenChess.Domain
{
    internal record MovePlayed(Coordinate Origin, Coordinate Destination, IReadOnlyPiece PieceMoved, IReadOnlyPiece? PieceCaptured, MoveType MoveType = MoveType.DefaultMove, string? PromotedPiece = null);
}