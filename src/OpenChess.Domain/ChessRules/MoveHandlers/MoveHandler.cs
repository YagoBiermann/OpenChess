namespace OpenChess.Domain
{
    internal abstract class MoveHandler : IMoveHandler
    {
        protected IMoveHandler? _nextHandler = null;
        protected Chessboard _chessboard;
        protected IMoveCalculator _movesCalculator;
        public MoveHandler(Chessboard chessboard, IMoveCalculator moveCalculator)
        {
            _chessboard = chessboard;
            _movesCalculator = moveCalculator;
        }

        public virtual MovePlayed Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (_nextHandler is null) { return HandleDefaultMove(origin, destination); }
            else { return _nextHandler.Handle(origin, destination, promotingPiece); }
        }

        public IMoveHandler SetNext(IMoveHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        protected void ThrowIfIllegalMove(IReadOnlyPiece piece, Coordinate destination)
        {
            if (!_movesCalculator.CanMoveToPosition(piece, destination)) throw new ChessboardException("Invalid move!");
        }

        private MovePlayed HandleDefaultMove(Coordinate origin, Coordinate destination)
        {
            Piece? piece = _chessboard.RemovePiece(origin);
            Piece? capturedPiece = _chessboard.RemovePiece(destination);
            _chessboard.AddPiece(destination, piece!.Name, piece.Color);
            Piece pieceMoved = _chessboard.GetSquare(destination).Piece!;
            MoveType movePlayed = piece is Pawn ? MoveType.PawnMove : MoveType.DefaultMove;

            return new(origin, destination, pieceMoved, capturedPiece, movePlayed);
        }
    }
}