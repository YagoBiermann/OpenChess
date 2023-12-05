namespace OpenChess.Domain
{
    internal record Move(Guid PlayerId, Coordinate Origin, Coordinate Destination, string? Promoting = null);
}