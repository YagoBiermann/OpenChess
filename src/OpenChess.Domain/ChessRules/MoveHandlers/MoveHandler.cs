namespace OpenChess.Domain
{
    internal abstract class MoveHandler : IMoveHandler
    {
        protected IMoveHandler? _nextHandler = null;
        protected Chessboard _chessboard;
        protected Match _match;
        protected IMoveCalculator _movesCalculator;
        public MoveHandler(Match match, Chessboard chessboard, IMoveCalculator moveCalculator)
        {
            _chessboard = chessboard;
            _movesCalculator = moveCalculator;
            _match = match;
        }

        public virtual MovePlayed Handle(IReadOnlyPiece piece, Coordinate destination, string? promotingPiece = null)
        {
            if (_nextHandler is null) { return HandleDefaultMove(piece, destination); }
            else { return _nextHandler.Handle(piece, destination, promotingPiece); }
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

        private MovePlayed HandleDefaultMove(IReadOnlyPiece piece, Coordinate destination)
        {
            _chessboard.RemovePiece(piece.Origin);
            Piece? capturedPiece = _chessboard.RemovePiece(destination);
            _chessboard.AddPiece(destination, piece!.Name, piece.Color);
            Piece pieceMoved = _chessboard.GetSquare(destination).Piece!;
            MoveType movePlayed = piece is Pawn ? MoveType.PawnMove : MoveType.DefaultMove;

            return new(piece.Origin, destination, pieceMoved, capturedPiece, movePlayed);
        }
    }
}