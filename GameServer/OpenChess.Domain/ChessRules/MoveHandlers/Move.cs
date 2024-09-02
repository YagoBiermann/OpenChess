namespace OpenChess.Domain
{
    public record Move(Guid PlayerId, Coordinate Origin, Coordinate Destination, string? Promoting = null);
}