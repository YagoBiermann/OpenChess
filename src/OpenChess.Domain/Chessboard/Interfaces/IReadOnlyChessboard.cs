namespace OpenChess.Domain
{
    internal interface IReadOnlyChessboard
    {
        public Color Turn { get; }
        public Castling Castling { get; }
        public Coordinate? EnPassant { get; set; }
        public int HalfMove { get; }
        public int FullMove { get; }
        public IReadOnlySquare GetReadOnlySquare(Coordinate coordinate);
        public IReadOnlySquare GetReadOnlySquare(string coordinate);
        public List<Coordinate> GetPiecesPosition(List<Coordinate> range);
        public List<Coordinate> GetPiecesPosition(Color player);
        public string ToString();
    }
}