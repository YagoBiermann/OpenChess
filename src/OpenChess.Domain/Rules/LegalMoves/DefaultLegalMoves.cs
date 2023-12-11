
namespace OpenChess.Domain
{
    internal class DefaultLegalMoves : ILegalMoves
    {
        private IReadOnlyChessboard _chessboard;
        public DefaultLegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }
        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            throw new NotImplementedException();
        }
    }
}