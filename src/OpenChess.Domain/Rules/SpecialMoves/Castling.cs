namespace OpenChess.Domain
{
    internal class Castling : MoveHandler
    {
        public bool HasWhiteKingSide { get; init; }
        public bool HasWhiteQueenSide { get; init; }
        public bool HasBlackKingSide { get; init; }
        public bool HasBlackQueenSide { get; init; }

        public Castling()
        {
            HasWhiteKingSide = true;
            HasWhiteQueenSide = true;
            HasBlackKingSide = true;
            HasBlackQueenSide = true;
        }

        public Castling(bool AvailabilityforAll)
        {
            HasWhiteKingSide = AvailabilityforAll;
            HasWhiteQueenSide = AvailabilityforAll;
            HasBlackKingSide = AvailabilityforAll;
            HasBlackQueenSide = AvailabilityforAll;
        }

        public Castling(bool whiteKingSide, bool whiteQueenSide, bool blackKingSide, bool blackQueenSide)
        {
            HasWhiteKingSide = whiteKingSide;
            HasWhiteQueenSide = whiteQueenSide;
            HasBlackKingSide = blackKingSide;
            HasBlackQueenSide = blackQueenSide;
        }

        public override string ToString()
        {
            string castlingAvailability = "";
            if (HasWhiteKingSide) castlingAvailability += "K";
            if (HasWhiteQueenSide) castlingAvailability += "Q";
            if (HasBlackKingSide) castlingAvailability += "k";
            if (HasBlackQueenSide) castlingAvailability += "q";
            if (!HasWhiteKingSide && !HasWhiteQueenSide && !HasBlackKingSide && !HasBlackQueenSide) castlingAvailability += "-";

            return castlingAvailability;
        }

        public static bool IsCastling(Coordinate origin, Coordinate destination, IReadOnlyChessboard chessboard)
        {
            bool isNotKingPosition = origin.Column != 'E' || !(origin.Row == '1' || origin.Row == '8');
            if (isNotKingPosition) return false;
            IReadOnlyPiece? piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
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