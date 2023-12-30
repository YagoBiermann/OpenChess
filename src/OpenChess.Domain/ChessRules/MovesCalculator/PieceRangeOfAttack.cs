namespace OpenChess.Domain
{
    internal readonly record struct PieceRangeOfAttack
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate> RangeOfAttack { get; }
        public IReadOnlyPiece? NearestPiece { get; }
        public bool IsHittingTheEnemyKing { get => NearestPiece is King && NearestPiece.Color == Piece.Color; }

        public PieceRangeOfAttack(IReadOnlyPiece piece, Direction direction, List<Coordinate> rangeOfAttack, IReadOnlyPiece? nearestPiece = null)
        {
            Piece = piece;
            Direction = direction;
            RangeOfAttack = rangeOfAttack;
            NearestPiece = nearestPiece;
        }
    }
}