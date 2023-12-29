namespace OpenChess.Domain
{
    internal interface IMoveCalculator
    {
        public bool PieceCanSolveTheCheck(IReadOnlyPiece piece);
        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination);
        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece);
        public List<PieceRangeOfAttack> CalculateMovesHittingTheEnemyKing(Color player);
        public void CalculateAndCacheAllMoves();
        public List<PieceRangeOfAttack> CalculateMoves(IReadOnlyPiece piece);
    }
}