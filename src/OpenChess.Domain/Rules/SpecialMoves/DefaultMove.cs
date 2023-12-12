namespace OpenChess.Domain
{
    internal class DefaultMove : MoveHandler
    {
        public DefaultMove(Chessboard chessboard) : base(chessboard) { }

        public override HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            return base.Handle(origin, destination, promotingPiece);
        }
    }
}