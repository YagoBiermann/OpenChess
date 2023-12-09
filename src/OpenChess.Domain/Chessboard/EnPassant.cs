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

        public IReadOnlyPiece? GetVulnerablePawn
        {
            get
            {
                if (Position is null) return null;

                Direction direction = Position!.Row == '3' ? new Up() : new Down();
                Coordinate pawnPosition = Coordinate.CalculateNextPosition(Position, direction)!;

                return _chessboard.GetReadOnlySquare(pawnPosition).ReadOnlyPiece;
            }
        }

        
    }
}