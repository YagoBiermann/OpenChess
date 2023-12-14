namespace OpenChess.Domain
{
    internal interface IMoveHandler
    {
        public IMoveHandler SetNext(IMoveHandler handler);
        public HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null);
    }
}