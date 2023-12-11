
namespace OpenChess.Domain
{
    internal class LegalMoves : ILegalMoves
    {
        private IReadOnlyChessboard _chessboard;
        public LegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            return GetMoveStrategy(piece).CalculateLegalMoves(piece);
        }

        public bool IsLegalMove(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece ?? throw new ChessboardException("Piece not found!");
            List<MoveDirections> legalMoves = CalculateLegalMoves(piece);

            return legalMoves.Exists(m => m.Coordinates.Contains(destination));
        }

        private ILegalMoves GetMoveStrategy(IReadOnlyPiece piece)
        {
            if (piece is Pawn) { return new PawnLegalMoves(_chessboard); }
            if (piece is Knight) { return new KnightLegalMoves(_chessboard); }

            return new DefaultLegalMoves(_chessboard);
        }
    }
}