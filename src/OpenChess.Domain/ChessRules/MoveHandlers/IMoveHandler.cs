namespace OpenChess.Domain
{
    internal interface IMoveHandler
    {
        public IMoveHandler SetNext(IMoveHandler handler);
        public MovePlayed Handle(IReadOnlyPiece piece, Coordinate destination, string? promotingPiece = null);
    }
}