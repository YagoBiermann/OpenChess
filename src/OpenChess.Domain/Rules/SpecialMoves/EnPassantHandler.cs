namespace OpenChess.Domain
{
    internal class EnPassantHandler : MoveHandler
    {
        public EnPassantHandler(Chessboard chessboard) : base(chessboard) { }

        public void Clear()
        {
            _chessboard.EnPassant = null;
        }

        public void SetVulnerablePawn(IReadOnlyPiece? piece)
        {
            if (piece is not Pawn pawn) return;
            if (!IsVulnerableToEnPassant(pawn)) return;

            _chessboard.EnPassant = GetEnPassantPosition(pawn);
        }

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
                if (_chessboard.EnPassant is null) return null;

                Direction direction = _chessboard.EnPassant!.Row == '3' ? new Up() : new Down();
                Coordinate pawnPosition = Coordinate.CalculateNextPosition(_chessboard.EnPassant, direction)!;

                return _chessboard.GetReadOnlySquare(pawnPosition).ReadOnlyPiece;
            }
        }

        private bool IsEnPassantMove(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;

            if (piece is not Pawn) return false;
            if (destination == _chessboard.EnPassant) return true;
            return false;
        }

        private Coordinate? GetEnPassantPosition(Pawn pawn)
        {
            if (!IsVulnerableToEnPassant(pawn)) return null;
            return Coordinate.CalculateNextPosition(pawn.Origin, Direction.Opposite(pawn.ForwardDirection));
        }

        private bool IsVulnerableToEnPassant(Pawn pawn)
        {
            bool isBlackVulnerable = pawn.Color == Color.Black && pawn.Origin.Row == '5';
            bool isWhiteVulnerable = pawn.Color == Color.White && pawn.Origin.Row == '4';

            return isBlackVulnerable ^ isWhiteVulnerable;
        }

        private bool CanCaptureByEnPassant(Pawn pawn)
        {
            if (_chessboard.EnPassant is null) return false;

            foreach (Direction direction in pawn.Directions)
            {
                if (direction is Up || direction is Down) continue;
                Coordinate? diagonal = Coordinate.CalculateSequence(pawn.Origin, direction, pawn.MoveAmount).FirstOrDefault();
                if (_chessboard.EnPassant == diagonal) return true;
            }

            return false;
        }
    }
}