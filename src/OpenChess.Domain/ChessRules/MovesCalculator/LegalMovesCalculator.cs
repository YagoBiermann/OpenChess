namespace OpenChess.Domain
{
    internal class LegalMovesCalculator : IMoveCalculator
    {
        private IReadOnlyChessboard _chessboard;
        public LegalMovesCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
        {
            IMoveCalculatorStrategy strategy = new IncludeEnemyPieceStrategy();
            return new MovesCalculator(_chessboard, strategy).CalculateMoves(piece);
        }

        public bool IsLegalMove(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece ?? throw new ChessboardException("Piece not found!");
            List<MoveDirections> legalMoves = CalculateMoves(piece);

            return legalMoves.Exists(m => m.Coordinates.Contains(destination));
        }
    }
}