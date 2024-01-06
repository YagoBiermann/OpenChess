namespace OpenChess.Domain
{
    internal class Square : IReadOnlySquare
    {
        public Coordinate Coordinate { get; }
        private Piece? _piece;
        public Color Color => (Coordinate.ColumnToInt + Coordinate.RowToInt) % 2 == 0 ? Color.Black : Color.White;
        public bool HasPiece { get => ReadOnlyPiece is not null; }

        public Square(Coordinate coordinate, Piece? piece = null)
        {
            Coordinate = coordinate;
            Piece = piece;
        }

        public IReadOnlyPiece? ReadOnlyPiece
        {
            get { return _piece; }
        }

        public Piece? Piece
        {
            get { return _piece; }
            set
            {
                _piece = value;
                if (_piece is not null)
                {
                    _piece.Origin = Coordinate;
                }
            }
        }

        public bool HasEnemyPiece(Color currentPlayer)
        {
            if (!HasPiece) return false;
            return ReadOnlyPiece?.Color != currentPlayer;
        }
    }
}