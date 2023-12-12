namespace OpenChess.Domain
{
    internal class EnPassant : MoveHandler
    {
        public Coordinate? Position { get; private set; }

        public EnPassant(Coordinate? coordinate, Chessboard chessboard) : base(chessboard)
        {
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
            if (!IsVulnerableToEnPassant(pawn)) return;

            Position = GetEnPassantPosition(pawn);
        }

        public bool IsEnPassantMove(Coordinate origin, Coordinate destination)
        {
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;

            if (piece is not Pawn) return false;
            if (destination == Position) return true;
            return false;
        }

        public Coordinate? GetEnPassantPosition(Pawn pawn)
        {
            if (!IsVulnerableToEnPassant(pawn)) return null;
            return Coordinate.CalculateNextPosition(pawn.Origin, Direction.Opposite(pawn.ForwardDirection));
        }

        public bool IsVulnerableToEnPassant(Pawn pawn)
        {
            bool isBlackVulnerable = pawn.Color == Color.Black && pawn.Origin.Row == '5';
            bool isWhiteVulnerable = pawn.Color == Color.White && pawn.Origin.Row == '4';

            return isBlackVulnerable ^ isWhiteVulnerable;
        }

        public bool CanCaptureByEnPassant(Pawn pawn)
        {
            if (_chessboard.EnPassant.Position is null) return false;

            foreach (Direction direction in pawn.Directions)
            {
                if (direction is Up || direction is Down) continue;
                Coordinate? diagonal = Coordinate.CalculateSequence(pawn.Origin, direction, pawn.MoveAmount).FirstOrDefault();
                if (_chessboard.EnPassant.Position == diagonal) return true;
            }

            return false;
        }
    }
}