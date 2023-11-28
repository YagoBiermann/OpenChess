
namespace OpenChess.Domain
{
    internal class Pawn : Piece
    {
        public Pawn(Color color, Coordinate origin) : base(color, origin)
        {
        }

        public bool IsFirstMove
        {
            get
            {
                bool isBlackFirstMove = Color == Color.Black && Origin.Row == '7';
                bool isWhiteFirstMove = Color == Color.White && Origin.Row == '2';
                return isBlackFirstMove || isWhiteFirstMove;
            }
        }

        public int ForwardMoveAmount
        {
            get
            {
                return IsFirstMove ? 2 : 1;
            }
        }

        public Direction ForwardDirection
        {
            get { return Color == Color.White ? new Up() : new Down(); }
        }

        public override char Name => Color == Color.Black ? 'p' : 'P';

        public override List<Direction> Directions => Color == Color.Black ? BlackDirections() : WhiteDirections();

        public override bool IsLongRange => false;

        public override List<Move> CalculateMoveRange()
        {
            List<Move> moves = new();

            foreach (Direction direction in Directions)
            {
                if (direction.Equals(ForwardDirection))
                {
                    List<Coordinate> forward = Coordinate.CalculateSequence(Origin, direction, ForwardMoveAmount);
                    moves.Add(new(direction, forward));
                    continue;
                }

                List<Coordinate> coordinates = Coordinate.CalculateSequence(Origin, direction, MoveAmount);
                moves.Add(new(direction, coordinates));
            }

            return moves;
        }

        public override List<Move> CalculateLegalMoves(Chessboard chessboard)
        {
            List<Move> legalMoves = new();
            List<Move> moveRange = CalculateMoveRange();

            foreach (Move move in moveRange)
            {
                if (move.Direction.Equals(ForwardDirection))
                {
                    List<Coordinate> forwardMoves = CalculateForwardMoves(chessboard, move.Coordinates);
                    legalMoves.Add(new(move.Direction, forwardMoves));
                    continue;
                };

                Coordinate? attackingPosition = move.Coordinates.FirstOrDefault();
                if (attackingPosition is null) { legalMoves.Add(new(move.Direction, new())); continue; };

                Square square = chessboard.GetSquare(attackingPosition);
                bool isEnPassant = attackingPosition.Equals(chessboard.EnPassant);
                if (isEnPassant) { legalMoves.Add(new(move.Direction, move.Coordinates)); continue; };
                if (!square.HasPiece) { legalMoves.Add(new(move.Direction, new())); continue; }
                bool hasAllyPiece = !square.HasEnemyPiece(Color);
                bool hasKing = square.HasTypeOfPiece(typeof(King));
                if (hasAllyPiece || hasKing) { legalMoves.Add(new(move.Direction, new())); continue; }

                legalMoves.Add(new(move.Direction, move.Coordinates));
            }

            return legalMoves;
        }

        private List<Coordinate> CalculateForwardMoves(Chessboard chessboard, List<Coordinate> forwardCoordinates)
        {
            List<Coordinate> piecesPosition = chessboard.FindPieces(forwardCoordinates);

            List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(Origin, piecesPosition);
            if (!distances.Any()) return forwardCoordinates;
            CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);

            List<Coordinate> forwardMoves = Coordinate.CalculateSequence(Origin, ForwardDirection, nearestPiece.DistanceBetween);
            forwardMoves.RemoveAt(forwardMoves.Count - 1);

            return forwardMoves;
        }

        private static List<Direction> BlackDirections()
        {
            List<Direction> directions = new()
            {
                new Down(),
                new LowerLeft(),
                new LowerRight()
            };

            return directions;
        }

        private static List<Direction> WhiteDirections()
        {
            List<Direction> directions = new()
            {
                new Up(),
                new UpperLeft(),
                new UpperRight()
            };

            return directions;
        }
    }
}