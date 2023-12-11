
namespace OpenChess.Domain
{
    internal class PawnLegalMoves : ILegalMoves
    {
        private IReadOnlyChessboard _chessboard;
        public PawnLegalMoves(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
        }
        
        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece)
        {
            throw new NotImplementedException();
        }
    }
}