namespace OpenChess.Domain
{
    internal readonly record struct MoveDirections
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate>? FullRange { get; }
        public List<Coordinate>? RangeOfAttack { get; }
        public IReadOnlyPiece? NearestPiece { get; }
        public List<CoordinateDistances>? PiecesWithDistanceFromOrigin { get; }

        public MoveDirections(IReadOnlyPiece piece, Direction direction, List<Coordinate>? fullMoveRange = null, List<Coordinate>? attackingMoveRange = null, List<CoordinateDistances>? allPiecesInMoveRange = null, IReadOnlyPiece? nearestPiece = null)
        {
            Piece = piece;
            Direction = direction;
            FullRange = fullMoveRange;
            RangeOfAttack = attackingMoveRange;
            PiecesWithDistanceFromOrigin = allPiecesInMoveRange;
            NearestPiece = nearestPiece;
        }
    }
}