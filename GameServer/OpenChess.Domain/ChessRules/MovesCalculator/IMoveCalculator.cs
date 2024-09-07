namespace OpenChess.Domain
{
    internal interface IMoveCalculator
    {
        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination);
        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece);
        public bool IsPinned(IReadOnlyPiece piece, out bool canMove);
        public void CalculateAndCacheAllMoves();
        public void ClearCache();
        public List<PieceAttackRange> CalculateRangeOfAttack(IReadOnlyPiece piece);
        public List<PieceLineOfSight> CalculateLineOfSight(IReadOnlyPiece piece);
        public List<PieceAttackRange> CalculateKingMoves(Color player);
        public List<PieceAttackRange> CalculatePawnMoves(Pawn pawn);
        public List<PieceAttackRange> CalculateLegalMoves(IReadOnlyPiece piece);
        public List<PieceAttackRange> CalculateAllMoves();
    }
}