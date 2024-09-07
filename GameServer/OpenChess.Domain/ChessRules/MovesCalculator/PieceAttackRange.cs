namespace OpenChess.Domain
{
    internal readonly record struct PieceAttackRange
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate> AttackRange { get; }
        public IReadOnlyPiece? NearestPiece { get; }
        public bool IsHittingTheEnemyKing { get => NearestPiece is King && NearestPiece.Color != Piece.Color; }

        public PieceAttackRange(IReadOnlyPiece piece, Direction direction, List<Coordinate> attackRange, IReadOnlyPiece? nearestPiece = null)
        {
            Piece = piece;
            Direction = direction;
            AttackRange = attackRange;
            NearestPiece = nearestPiece;
        }
    }
}