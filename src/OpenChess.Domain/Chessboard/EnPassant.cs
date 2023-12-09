namespace OpenChess.Domain
{
    internal record EnPassant
    {
        public Coordinate? Position;
        private Chessboard _chessboard;

        public EnPassant(Coordinate? coordinate, Chessboard chessboard)
        {
            _chessboard = chessboard;
            Position = coordinate;
        }

        
    }
}