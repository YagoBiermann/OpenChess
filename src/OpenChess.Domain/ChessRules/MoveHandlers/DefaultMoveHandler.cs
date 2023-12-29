namespace OpenChess.Domain
{
    internal class DefaultMoveHandler : MoveHandler
    {
        public DefaultMoveHandler(Chessboard chessboard, IMoveCalculator moveCalculator) : base(chessboard, moveCalculator) { }

        public override MovePlayed Handle(IReadOnlyPiece piece, Coordinate destination, string? promotingPiece = null)
        {
            ThrowIfIllegalMove(piece, destination);
            return base.Handle(piece, destination, promotingPiece);
        }
    }
}