namespace OpenChess.Domain
{
    internal interface IReadOnlyChessboard
    {
        public ICastlingAvailability CastlingAvailability { get; }
        public IEnPassantAvailability EnPassantAvailability { get; }
        public int HalfMove { get; }
        public int FullMove { get; }
        public IReadOnlySquare GetReadOnlySquare(string coordinate);
        public IReadOnlySquare GetReadOnlySquare(Coordinate position);
        public IReadOnlyPiece? GetPiece(Coordinate position);
        public List<IReadOnlyPiece> GetPieces(List<Coordinate> range);
        public List<IReadOnlyPiece> GetPieces(Color player);
        public List<IReadOnlyPiece> GetAllPieces();
    }
}