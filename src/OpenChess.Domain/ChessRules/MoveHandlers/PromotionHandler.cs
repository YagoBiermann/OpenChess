using System.Text.RegularExpressions;

namespace OpenChess.Domain
{
    internal class PromotionHandler : MoveHandler
    {
        public PromotionHandler(Match match, Chessboard chessboard, IMoveCalculator moveCalculator) : base(match, chessboard, moveCalculator) { }

        public override MovePlayed Handle(IReadOnlyPiece piece, Coordinate destination, string? promotingPiece = null)
        {
            if (IsPromoting(piece, destination))
            {
                ThrowIfIllegalMove(piece, destination);
                string promotingTo = promotingPiece ?? DefaultPiece;

                if (!IsValidString(promotingTo)) throw new ChessboardException("Invalid promoting piece");
                if (piece is not Pawn) throw new ChessboardException("Cannot handle promotion because piece is not a pawn.");
                IReadOnlyPiece? pieceCaptured = base.Handle(piece, destination).PieceCaptured;

                _chessboard.AddPiece(destination, char.Parse(promotingTo), _match.CurrentPlayerColor!.Value);
                IReadOnlyPiece pieceMoved = _chessboard.GetPiece(destination)!;

                return new(piece.Origin, destination, pieceMoved, pieceCaptured, MoveType.PawnPromotionMove, promotingTo);
            }
            else { return base.Handle(piece, destination, promotingPiece); }
        }

        public static string DefaultPiece { get { return "Q"; } }

        private bool IsPromoting(IReadOnlyPiece piece, Coordinate destination)
        {
            if (piece is not Pawn pawn) return false;

            bool isWhitePromoting = pawn.ForwardDirection is Up && destination.Row == '8';
            bool isBlackPromoting = pawn.ForwardDirection is Down && destination.Row == '1';

            return isWhitePromoting ^ isBlackPromoting;
        }

        private static bool IsValidString(string value)
        {
            Regex rx = new(@"^([qbrn]{1})$", RegexOptions.IgnoreCase);
            return rx.IsMatch(value);
        }
    }
}