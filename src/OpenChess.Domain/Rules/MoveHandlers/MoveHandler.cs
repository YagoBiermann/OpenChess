namespace OpenChess.Domain
{
    internal abstract class MoveHandler : IMoveHandler
    {
        protected IMoveHandler? _nextHandler = null;
        protected Chessboard _chessboard;
        protected LegalMoves _legalMoves;
        public MoveHandler(Chessboard chessboard)
        {
            _chessboard = chessboard;
            _legalMoves = new LegalMoves(_chessboard);
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

        protected void ThrowIfIllegalMove(Coordinate origin, Coordinate destination)
        {
            if (!_legalMoves.IsLegalMove(origin, destination)) throw new ChessboardException("Invalid move!");
        }

        private MovePlayed HandleDefaultMove(Coordinate origin, Coordinate destination)
        {
            Square originSquare = _chessboard.GetSquare(origin);
            Square destinationSquare = _chessboard.GetSquare(destination);
            Piece piece = originSquare.Piece!;
            Piece? capturedPiece = destinationSquare.Piece;
            originSquare.Piece = null;
            destinationSquare.Piece = piece;

            MoveType movePlayed = piece is Pawn ? MoveType.PawnMove : MoveType.DefaultMove;
            return new(destinationSquare.Piece, capturedPiece, movePlayed);
        }
    }
}