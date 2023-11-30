
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

        public override List<MovePositions> CalculateMoveRange()
        {
            List<MovePositions> moves = new();

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

        public override List<MovePositions> CalculateLegalMoves(Chessboard chessboard)
        {
            List<MovePositions> legalMoves = new();
            List<MovePositions> moveRange = CalculateMoveRange();

            foreach (MovePositions move in moveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> currentCoordinates = move.Coordinates;

                if (currentDirection.Equals(ForwardDirection))
                {
                    List<Coordinate> forwardMoves = CalculateForwardMoves(chessboard, currentCoordinates);
                    legalMoves.Add(new(currentDirection, forwardMoves));
                    continue;
                };

                List<Coordinate> emptyList = new();
                MovePositions emptyPosition = new(currentDirection, emptyList);
                MovePositions sameCurrentPosition = new(currentDirection, currentCoordinates);

                Coordinate? diagonal = currentCoordinates.FirstOrDefault();
                bool diagonalIsOutOfChessboard = diagonal is null;
                if (diagonalIsOutOfChessboard) { legalMoves.Add(emptyPosition); continue; };

                Square square = chessboard.GetSquare(diagonal!);
                bool isEnPassant = diagonal!.Equals(chessboard.EnPassant);
                if (isEnPassant) { legalMoves.Add(sameCurrentPosition); continue; };
                if (!square.HasPiece) { legalMoves.Add(emptyPosition); continue; }
                bool hasAllyPiece = !square.HasEnemyPiece(Color);
                bool hasKing = square.HasTypeOfPiece(typeof(King));
                if (hasAllyPiece || hasKing) { legalMoves.Add(emptyPosition); continue; }

                legalMoves.Add(sameCurrentPosition);
            }

            return legalMoves;
        }

        private List<Coordinate> CalculateForwardMoves(Chessboard chessboard, List<Coordinate> forwardCoordinates)
        {
            List<Coordinate> piecesPosition = chessboard.FindPieces(forwardCoordinates);

            List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(Origin, piecesPosition);
            bool noPiecesForward = !distances.Any();
            if (noPiecesForward) return forwardCoordinates;
            CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);

            List<Coordinate> forwardMoves = Coordinate.CalculateSequence(Origin, ForwardDirection, nearestPiece.DistanceBetween);
            int lastPosition = forwardMoves.Count - 1;
            forwardMoves.RemoveAt(lastPosition);

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