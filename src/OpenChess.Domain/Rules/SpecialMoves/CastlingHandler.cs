namespace OpenChess.Domain
{
    internal class CastlingHandler : MoveHandler
    {
        public CastlingHandler(Chessboard chessboard) : base(chessboard) { }

        public override HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (IsCastling(origin, destination))
            {
                return null;
            }
            else { return base.Handle(origin, destination, promotingPiece); }
        }

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

        private bool IsCastlingKingSide(Coordinate destination, Color player)
        {
            return GetKingSidePositions(player).Contains(destination);
        }

        private bool IsCastlingQueenSide(Coordinate destination, Color player)
        {
            return GetQueenSidePositions(player).Contains(destination);
        }

        private List<Coordinate> GetCastlingSide(Coordinate destination, Color player)
        {
            if (IsCastlingKingSide(destination, player)) return GetKingSidePositions(player);
            if (IsCastlingQueenSide(destination, player)) return GetQueenSidePositions(player);
            throw new ChessboardException("Castling side could not be determined");
        }


        }

        private static List<Coordinate> BlackCastlingPositions
        {
            get
            {
                List<Coordinate> positions = new()
        private static List<Coordinate> GetQueenSidePositions(Color player)
        {
            string row = player == Color.Black ? "8" : "1";


            List<Coordinate> positions = new()
                {
                    Coordinate.GetInstance($"E{row}"),
                    Coordinate.GetInstance($"D{row}"),
                    Coordinate.GetInstance($"C{row}"),
                    Coordinate.GetInstance($"B{row}"),
                    Coordinate.GetInstance($"A{row}"),
                };

            return positions;
        }

        private static List<Coordinate> GetKingSidePositions(Color player)
        {
            string row = player == Color.Black ? "8" : "1";


            List<Coordinate> positions = new()
                {
                    Coordinate.GetInstance($"E{row}"),
                    Coordinate.GetInstance($"F{row}"),
                    Coordinate.GetInstance($"G{row}"),
                    Coordinate.GetInstance($"H{row}"),
                };

            return positions;
        }
    }
}