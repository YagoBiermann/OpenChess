
namespace OpenChess.Domain
{
    internal class Pawn : Piece
    {
        public Pawn(Color color, Coordinate origin, Chessboard chessboard) : base(color, origin, chessboard)
        {
        }

        public Coordinate? GetEnPassantPosition
        {
            get
            {
                if (!IsVulnerableToEnPassant) return null;
                return Coordinate.CalculateNextPosition(Origin, Direction.Opposite(ForwardDirection));
            }
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

        public override List<MoveDirections> CalculateMoveRange()
        {
            List<MoveDirections> moves = new();

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

        public override List<MoveDirections> CalculateLegalMoves()
        {
            List<MoveDirections> legalMoves = new();
            List<MoveDirections> moveRange = CalculateMoveRange();

            foreach (MoveDirections move in moveRange)
            {
                Direction currentDirection = move.Direction;
                List<Coordinate> currentCoordinates = move.Coordinates;

                if (currentDirection.Equals(ForwardDirection))
                {
                    List<Coordinate> forwardMoves = CalculateForwardMoves(Chessboard, currentCoordinates);
                    legalMoves.Add(new(currentDirection, forwardMoves));
                    continue;
                };

                List<Coordinate> emptyList = new();
                MoveDirections emptyPosition = new(currentDirection, emptyList);
                MoveDirections sameCurrentPosition = new(currentDirection, currentCoordinates);

                Coordinate? diagonal = currentCoordinates.FirstOrDefault();
                bool diagonalIsOutOfChessboard = diagonal is null;
                if (diagonalIsOutOfChessboard) { legalMoves.Add(emptyPosition); continue; };

                IReadOnlySquare square = Chessboard.GetReadOnlySquare(diagonal!);
                bool isEnPassant = diagonal!.Equals(Chessboard.EnPassant);
                if (isEnPassant) { legalMoves.Add(sameCurrentPosition); continue; };
                if (!square.HasPiece) { legalMoves.Add(emptyPosition); continue; }
                bool hasAllyPiece = !square.HasEnemyPiece(Color);
                bool hasKing = square.HasTypeOfPiece(typeof(King));
                if (hasAllyPiece || hasKing) { legalMoves.Add(emptyPosition); continue; }

                legalMoves.Add(sameCurrentPosition);
            }

            return legalMoves;
        }

        private List<Coordinate> CalculateForwardMoves(IReadOnlyChessboard chessboard, List<Coordinate> forwardCoordinates)
        {
            List<Coordinate> piecesPosition = chessboard.GetPiecesPosition(forwardCoordinates);

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