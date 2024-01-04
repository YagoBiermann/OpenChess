namespace OpenChess.Domain
{
    internal class EnPassantHandler : MoveHandler
    {
        public EnPassantHandler(Match match, Chessboard chessboard, IMoveCalculator moveCalculator) : base(match, chessboard, moveCalculator) { }

        public override MovePlayed Handle(IReadOnlyPiece piece, Coordinate destination, string? promotingPiece = null)
        {
            if (IsEnPassantMove(piece, destination))
            {
                ThrowIfIllegalMove(piece, destination);
                var pawn = (Pawn)piece;
                if (!CanCaptureByEnPassant(pawn)) throw new ChessboardException("This pawn cannot capture by en passant!");

                var move = base.Handle(piece, destination);
                Coordinate vulnerablePawnPosition = GetVulnerablePawn!.Origin;
                IReadOnlyPiece? pieceCaptured = _chessboard.RemovePiece(vulnerablePawnPosition);

                return new(piece.Origin, destination, move.PieceMoved, pieceCaptured, MoveType.EnPassantMove);
            }
            else { return base.Handle(piece, destination, promotingPiece); }
        }

        private IReadOnlyPiece? GetVulnerablePawn
        {
            get
            {
                if (_chessboard.EnPassantAvailability.EnPassantPosition is null) return null;

                Direction direction = _chessboard.EnPassantAvailability.EnPassantPosition!.Row == '3' ? new Up() : new Down();
                Coordinate pawnPosition = Coordinate.CalculateNextPosition(_chessboard.EnPassantAvailability.EnPassantPosition, direction)!;

                return _chessboard.GetPiece(pawnPosition);
            }
        }

        private bool IsEnPassantMove(IReadOnlyPiece piece, Coordinate destination)
        {
            if (piece is not Pawn) return false;
            if (destination == _chessboard.EnPassantAvailability.EnPassantPosition) return true;
            return false;
        }

        private bool CanCaptureByEnPassant(Pawn pawn)
        {
            if (_chessboard.EnPassantAvailability.EnPassantPosition is null) return false;

            foreach (Direction direction in pawn.Directions)
            {
                if (direction is Up || direction is Down) continue;
                Coordinate? diagonal = Coordinate.CalculateSequence(pawn.Origin, direction, pawn.MoveAmount).FirstOrDefault();
                if (_chessboard.EnPassantAvailability.EnPassantPosition == diagonal) return true;
            }

            return false;
        }
    }
}