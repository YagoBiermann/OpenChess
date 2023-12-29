namespace OpenChess.Domain
{
    internal class DefaultMoveHandler : MoveHandler
    {
        public DefaultMoveHandler(Chessboard chessboard, IMoveCalculator moveCalculator) : base(chessboard, moveCalculator) { }

        public override MovePlayed Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            ThrowIfIllegalMove(origin, destination);
            return base.Handle(origin, destination, promotingPiece);
        }
    }
}