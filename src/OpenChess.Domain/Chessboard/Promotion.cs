using System.Text.RegularExpressions;

namespace OpenChess.Domain
{
    internal class Promotion
    {
        private Chessboard _chessboard;

        public Promotion(Chessboard chessboard)
        {
            _chessboard = chessboard;
        }
    }
}