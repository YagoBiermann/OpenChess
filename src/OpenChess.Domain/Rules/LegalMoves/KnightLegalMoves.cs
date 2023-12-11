
namespace OpenChess.Domain
{
    internal class KnightLegalMoves : ILegalMoves
    {
        private IReadOnlyChessboard _chessboard;
        public KnightLegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }
        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            throw new NotImplementedException();
        }
    }
}