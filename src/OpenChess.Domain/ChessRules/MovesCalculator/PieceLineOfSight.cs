namespace OpenChess.Domain
{
    internal readonly record struct PieceLineOfSight
    {
        public IReadOnlyPiece Piece { get; }
        public Direction Direction { get; }
        public List<Coordinate> LineOfSight { get; }
        public List<IReadOnlyPiece> AllPiecesInLineOfSight { get; }
        public bool AnyPieceInLineOfSight { get => AllPiecesInLineOfSight.Any(); }

        public PieceLineOfSight(IReadOnlyPiece piece, Direction direction, List<Coordinate> lineOfSight, List<IReadOnlyPiece> allPiecesInLineOfSight)
        {
            Piece = piece;
            Direction = direction;
            LineOfSight = lineOfSight;
            AllPiecesInLineOfSight = allPiecesInLineOfSight;
        }
    }
}