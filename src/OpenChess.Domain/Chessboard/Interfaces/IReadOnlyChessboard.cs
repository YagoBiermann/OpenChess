namespace OpenChess.Domain
{
    internal interface IReadOnlyChessboard
    {
        public Color CurrentPlayer { get; }
        public Color Opponent { get; }
        public ICastlingAvailability CastlingAvailability { get; }
        public IEnPassantAvailability EnPassantAvailability { get; }
        public int HalfMove { get; }
        public int FullMove { get; }
        public IReadOnlySquare GetReadOnlySquare(string coordinate);
        public IReadOnlyPiece? GetPiece(Coordinate position);
        public List<IReadOnlyPiece> GetPieces(List<Coordinate> range);
        public List<IReadOnlyPiece> GetPieces(Color player);
        public List<IReadOnlyPiece> GetAllPieces();
    }
}