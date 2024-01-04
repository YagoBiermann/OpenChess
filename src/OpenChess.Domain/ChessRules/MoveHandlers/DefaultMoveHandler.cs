namespace OpenChess.Domain
{
    internal class DefaultMoveHandler : MoveHandler
    {
        public DefaultMoveHandler(Match match, Chessboard chessboard, IMoveCalculator moveCalculator) : base(match, chessboard, moveCalculator) { }

        public override MovePlayed Handle(IReadOnlyPiece piece, Coordinate destination, string? promotingPiece = null)
        {
            ThrowIfIllegalMove(piece, destination);
            return base.Handle(piece, destination, promotingPiece);
        }
    }
}