namespace OpenChess.Domain
{
    internal class EnPassantAvailability
    {
        public Coordinate? EnPassantPosition { get; private set; }
        public bool IsAvailable { get; private set; }

        public EnPassantAvailability(Coordinate? position)
        {
            EnPassantPosition = position;
            if (position is null) IsAvailable = false;
        }

        public void ClearEnPassant()
        {
            EnPassantPosition = null;
        }

        public void SetVulnerablePawn(IReadOnlyPiece? piece)
        {
            if (piece is not Pawn pawn) return;
            if (!IsVulnerableToEnPassant(pawn)) return;

            EnPassantPosition = GetEnPassantPosition(pawn);
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

    }
}