namespace OpenChess.Domain
{
    internal readonly record struct PieceRangeOfAttack
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate> LineOfSight { get; }
        public List<Coordinate> RangeOfAttack { get; }
        public IReadOnlyPiece? NearestPiece { get; }
        public List<IReadOnlyPiece> AllPiecesInLineOfSight { get; }
        public bool IsHittingTheEnemyKing { get; }

        public PieceRangeOfAttack(IReadOnlyPiece piece, Direction direction, List<Coordinate> lineOfSight, List<Coordinate> rangeOfAttack, List<IReadOnlyPiece> allPiecesInLineOfSight, IReadOnlyPiece? nearestPiece = null, bool isHittingTheEnemyKing = false)
        {
            Piece = piece;
            Direction = direction;
            LineOfSight = lineOfSight;
            RangeOfAttack = rangeOfAttack;
            AllPiecesInLineOfSight = allPiecesInLineOfSight;
            NearestPiece = nearestPiece;
            IsHittingTheEnemyKing = isHittingTheEnemyKing;
        }
    }
}