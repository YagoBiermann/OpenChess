namespace OpenChess.Domain
{
    internal readonly record struct PieceLineOfSight
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate> LineOfSight { get; }
        public List<PieceDistances> PiecesInLineOfSight { get; }
        public bool AnyPieceInLineOfSight { get => PiecesInLineOfSight.Any(); }

        public PieceLineOfSight(IReadOnlyPiece piece, Direction direction, List<Coordinate> lineOfSight, List<PieceDistances> piecesInLineOfSight)
        {
            Piece = piece;
            Direction = direction;
            LineOfSight = lineOfSight;
            PiecesInLineOfSight = piecesInLineOfSight;
        }
    }
}