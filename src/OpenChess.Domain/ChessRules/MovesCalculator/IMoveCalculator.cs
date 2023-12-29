namespace OpenChess.Domain
{
    internal interface IMoveCalculator
    {
        public bool PieceCanSolveTheCheck(IReadOnlyPiece piece);
        public bool CanMoveToPosition(IReadOnlyPiece piece, Coordinate destination);
        public bool IsHittingTheEnemyKing(IReadOnlyPiece piece);
        public List<MoveDirections> CalculateMovesHittingTheEnemyKing(Color player);
        public void CalculateAndCacheAllMoves();
        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece);
    }
}