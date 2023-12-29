namespace OpenChess.Domain
{
    internal readonly record struct PieceRangeOfAttack
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate>? FullRange { get; }
        public List<Coordinate>? RangeOfAttack { get; }
        public IReadOnlyPiece? NearestPiece { get; }
        public List<PieceDistances>? PiecesWithDistanceFromOrigin { get; }
        public bool IsHittingTheEnemyKing { get; }

        public PieceRangeOfAttack(IReadOnlyPiece piece, Direction direction, List<Coordinate>? fullMoveRange = null, List<Coordinate>? attackingMoveRange = null, List<PieceDistances>? allPiecesInMoveRange = null, IReadOnlyPiece? nearestPiece = null, bool isHittingTheEnemyKing = false)
        {
            Piece = piece;
            Direction = direction;
            FullRange = fullMoveRange;
            RangeOfAttack = attackingMoveRange;
            PiecesWithDistanceFromOrigin = allPiecesInMoveRange;
            NearestPiece = nearestPiece;
            IsHittingTheEnemyKing = isHittingTheEnemyKing;
        }
    }
}