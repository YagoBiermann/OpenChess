namespace OpenChess.Domain
{
    internal class DefaultMoveHandler : MoveHandler
    {
        public DefaultMoveHandler(Chessboard chessboard) : base(chessboard) { }

        public override HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            ThrowIfIllegalMove(origin, destination);
            return base.Handle(origin, destination, promotingPiece);
        }
    }
}