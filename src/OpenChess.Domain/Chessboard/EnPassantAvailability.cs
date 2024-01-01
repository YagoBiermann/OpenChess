namespace OpenChess.Domain
{
    internal class EnPassantAvailability : IEnPassantAvailability
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

        public void SetVulnerablePawn(IReadOnlyPiece? piece, Coordinate previousOrigin)
        {
            if (piece is not Pawn pawn) return;
            if (!IsVulnerableToEnPassant(pawn, previousOrigin)) return;

            EnPassantPosition = GetEnPassantPosition(pawn, previousOrigin);
        }
        private static Coordinate? GetEnPassantPosition(Pawn pawn, Coordinate previousOrigin)
        {
            if (!IsVulnerableToEnPassant(pawn, previousOrigin)) return null;
            return Coordinate.CalculateNextPosition(pawn.Origin, Direction.Opposite(pawn.ForwardDirection));
        }

        private static bool IsVulnerableToEnPassant(Pawn pawn, Coordinate previousOrigin)
        {
            bool isBlackVulnerable = pawn.Color == Color.Black && previousOrigin.Row == '7' && pawn.Origin.Row == '5';
            bool isWhiteVulnerable = pawn.Color == Color.White && previousOrigin.Row == '2' && pawn.Origin.Row == '4';

            return isBlackVulnerable ^ isWhiteVulnerable;
        }

    }
}