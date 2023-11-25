namespace OpenChess.Domain
{
    internal class Square
    {
        public Coordinate Coordinate { get; }
        private Piece? _piece;
        public bool HasPiece { get => Piece is not null; }

        public Square(Coordinate coordinate, Piece? piece = null)
        {
            Coordinate = coordinate;
            Piece = piece;
        }
        public void RemovePiece()
        {
            Piece = null;
        }

        public void AddPiece(Piece piece)
        {
            Piece = piece;
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
            return Piece?.Color != currentPlayer;
        }

        public bool HasTypeOfPiece(Type piece)
        {
            if (!HasPiece) return false;
            return Piece?.GetType() == piece;
        }
    }
}