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

        public bool IsPromoting(Coordinate origin, Coordinate destination)
        {
            IReadOnlySquare square = _chessboard.GetReadOnlySquare(origin);
            if (square.ReadOnlyPiece is not Pawn pawn) return false;

            bool isWhitePromoting = pawn.ForwardDirection is Up && destination.Row == '8';
            bool isBlackPromoting = pawn.ForwardDirection is Down && destination.Row == '1';

            return isWhitePromoting ^ isBlackPromoting;
        }
    }
}