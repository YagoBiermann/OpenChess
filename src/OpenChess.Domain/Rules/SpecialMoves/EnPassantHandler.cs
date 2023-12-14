namespace OpenChess.Domain
{
    internal class EnPassantHandler : MoveHandler
    {
        public EnPassantHandler(Chessboard chessboard) : base(chessboard) { }

        public override HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (IsEnPassantMove(origin, destination))
            {
                ThrowIfIllegalMove(origin, destination);
                IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
                var pawn = (Pawn)piece!;
                if (!CanCaptureByEnPassant(pawn)) throw new ChessboardException("This pawn cannot capture by en passant!");

                var move = base.Handle(origin, destination);
                Coordinate vulnerablePawnPosition = GetVulnerablePawn!.Origin;
                IReadOnlyPiece? pieceCaptured = _chessboard.RemovePiece(vulnerablePawnPosition);

                return new(move.PieceMoved, pieceCaptured);
            }
            else { return base.Handle(origin, destination, promotingPiece); }
        }

        private IReadOnlyPiece? GetVulnerablePawn
        {
            get
            {
                if (_chessboard.EnPassantAvailability.EnPassantPosition is null) return null;

                Direction direction = _chessboard.EnPassantAvailability.EnPassantPosition!.Row == '3' ? new Up() : new Down();
                Coordinate pawnPosition = Coordinate.CalculateNextPosition(_chessboard.EnPassantAvailability.EnPassantPosition, direction)!;

                return _chessboard.GetReadOnlySquare(pawnPosition).ReadOnlyPiece;
            }
        }

        private bool IsEnPassantMove(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;

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