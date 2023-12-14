using System.Text.RegularExpressions;

namespace OpenChess.Domain
{
    internal class PromotionHandler : MoveHandler
    {
        public PromotionHandler(Chessboard chessboard) : base(chessboard) { }

        public override HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (IsPromoting(origin, destination))
            {
                ThrowIfIllegalMove(origin, destination);
                string promotingTo = promotingPiece ?? DefaultPiece;

                if (!IsValidString(promotingTo)) throw new ChessboardException("Invalid promoting piece");
                IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
                if (piece is not Pawn) throw new ChessboardException("Cannot handle promotion because piece is not a pawn.");
                IReadOnlyPiece? pieceCaptured = base.Handle(origin, destination).PieceCaptured;

                _chessboard.AddPiece(destination, char.Parse(promotingTo), _chessboard.Turn);
                IReadOnlyPiece pieceMoved = _chessboard.GetReadOnlySquare(destination).ReadOnlyPiece!;

                return new(pieceMoved, pieceCaptured);
            }
            else { return base.Handle(origin, destination, promotingPiece); }
        }

        public static string DefaultPiece { get { return "Q"; } }

        private bool IsPromoting(Coordinate origin, Coordinate destination)
        {
            IReadOnlySquare square = _chessboard.GetReadOnlySquare(origin);
            if (square.ReadOnlyPiece is not Pawn pawn) return false;

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