namespace OpenChess.Domain
{
    internal class CastlingHandler : MoveHandler
    {
        public CastlingHandler(Chessboard chessboard) : base(chessboard) { }

        private bool IsCastling(Coordinate origin, Coordinate destination)
        {
            bool isNotKingPosition = origin.Column != 'E' || !(origin.Row == '1' || origin.Row == '8');
            if (isNotKingPosition) return false;
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
            if (piece is not King) return false;
            if (origin.Row == '1' && !WhiteCastlingPositions.Contains(destination)) return false;
            if (origin.Row == '8' && !BlackCastlingPositions.Contains(destination)) return false;

            return true;
        }

        public override HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (IsCastling(origin, destination))
            {
                return null;
            }
            else { return base.Handle(origin, destination, promotingPiece); }
        }

        private static List<Coordinate> WhiteCastlingPositions
        {
            get
            {
                List<Coordinate> positions = new()
                {
                    Coordinate.GetInstance("G1"),
                    Coordinate.GetInstance("C1"),
                };

                return positions;
            }
        }

        private static List<Coordinate> BlackCastlingPositions
        {
            get
            {
                List<Coordinate> positions = new()
                {
                    Coordinate.GetInstance("G8"),
                    Coordinate.GetInstance("C8"),
                };

                return positions;
            }
        }
    }
}