namespace OpenChess.Domain
{
    internal interface IMoveHandler
    {
        public IMoveHandler SetNext(IMoveHandler handler);
        public MovePlayed Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null);
    }
}