using System.Text.RegularExpressions;

namespace OpenChess.Domain
{
    internal class Promotion : MoveHandler
    {
        public Promotion(Chessboard chessboard) : base(chessboard) { }
        public bool IsPromoting(Coordinate origin, Coordinate destination)
        {
            IReadOnlySquare square = _chessboard.GetReadOnlySquare(origin);
            if (square.ReadOnlyPiece is not Pawn pawn) return false;

            bool isWhitePromoting = pawn.ForwardDirection is Up && destination.Row == '8';
            bool isBlackPromoting = pawn.ForwardDirection is Down && destination.Row == '1';

            return isWhitePromoting ^ isBlackPromoting;
        }

        public static bool IsValidString(string value)
        {
            Regex rx = new(@"^([qbrn]{1})$", RegexOptions.IgnoreCase);
            return rx.IsMatch(value);
        }

        public static string DefaultPiece { get { return "Q"; } }
    }
}