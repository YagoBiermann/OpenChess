
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

        public override char Name => Color == Color.Black ? 'p' : 'P';

        public override List<Direction> Directions => Color == Color.Black ? BlackDirections() : WhiteDirections();

        public override bool IsLongRange => false;

        public override List<Move> CalculateMoveRange()
        {
            List<Move> moves = new();

            foreach (Direction direction in Directions)
            {
                if (direction is Up || direction is Down)
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

        private List<Direction> BlackDirections()
        {
            List<Direction> directions = new()
            {
                new Down(),
                new LowerLeft(),
                new LowerRight()
            };

            return directions;
        }

        private List<Direction> WhiteDirections()
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