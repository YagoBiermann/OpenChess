namespace OpenChess.Domain
{
    internal interface IMoveCalculator
    {
        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination);
        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece);
        public void CalculateAndCacheAllMoves();
        public List<PieceRangeOfAttack> CalculateMoves(IReadOnlyPiece piece);
        public List<PieceLineOfSight> CalculateLineOfSight(IReadOnlyPiece piece);
        public List<PieceRangeOfAttack> CalculateKingMoves(Color player);
        public List<PieceRangeOfAttack> CalculateAllMoves();
    }
}