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

        public List<List<MoveDirections>> CalculateAllMoves(Color player)
        {
            List<Coordinate> piecesPosition = _chessboard.GetPiecesPosition(player);
            List<List<MoveDirections>> allLegalMoves = new();

            foreach (Coordinate position in piecesPosition)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece ?? throw new ChessboardException("Could not calculate all legal moves because piece was not found");
                List<MoveDirections> moves = CalculateMoves(piece);
                allLegalMoves.Add(moves);
            }

            return allLegalMoves;
        }

        public bool IsLegalMove(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece ?? throw new ChessboardException("Piece not found!");
            List<MoveDirections> legalMoves = CalculateMoves(piece);

            return legalMoves.Exists(m => m.Coordinates.Contains(destination));
        }
    }
}