namespace OpenChess.Domain
{
    internal record EnPassant
    {
        public Coordinate? Position { get; private set; }
        private Chessboard _chessboard;

        public EnPassant(Coordinate? coordinate, Chessboard chessboard)
        {
            _chessboard = chessboard;
            Position = coordinate;
        }

        public IReadOnlyPiece? GetVulnerablePawn
        {
            get
            {
                if (Position is null) return null;

                Direction direction = Position!.Row == '3' ? new Up() : new Down();
                Coordinate pawnPosition = Coordinate.CalculateNextPosition(Position, direction)!;

                return _chessboard.GetReadOnlySquare(pawnPosition).ReadOnlyPiece;
            }
        }

        public void HandleUpdate(Coordinate lastMovedPiece)
        {
            Position = null;
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(lastMovedPiece).ReadOnlyPiece;
            if (piece is not Pawn pawn) return;
            if (!pawn.IsVulnerableToEnPassant) return;

            Position = pawn.GetEnPassantPosition;
        }

        public bool IsEnPassantMove(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;

            if (piece is not Pawn) return false;
            if (destination == Position) return true;
            return false;
        }
    }
}