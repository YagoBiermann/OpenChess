namespace OpenChess.Domain
{
    internal abstract class MoveHandler : IMoveHandler
    {
        protected IMoveHandler? _nextHandler = null;
        protected Chessboard _chessboard;
        public MoveHandler(Chessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public virtual HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (_nextHandler is null) { return HandleDefaultMove(origin, destination); }
            else { return _nextHandler.Handle(origin, destination, promotingPiece); }
        }

        public IMoveHandler SetNext(IMoveHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        private HandledMove HandleDefaultMove(Coordinate origin, Coordinate destination)
        {
            Square originSquare = _chessboard.GetSquare(origin);
            Square destinationSquare = _chessboard.GetSquare(destination);
            Piece piece = originSquare.Piece!;
            Piece? capturedPiece = destinationSquare.Piece;
            originSquare.Piece = null;
            destinationSquare.Piece = piece;

            return new(destinationSquare.Piece, capturedPiece);
        }
    }
}