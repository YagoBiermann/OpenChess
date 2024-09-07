
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
                bool isBlackFirstMove = Color.Value == Color.Black && Origin.Row == '7';
                bool isWhiteFirstMove = Color.Value == Color.White && Origin.Row == '2';
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
            get { return Color.Value == Color.White ? new Up() : new Down(); }
        }

        public override char Name => Color.Value == Color.Black ? 'p' : 'P';

        public override List<Direction> Directions => Color.Value == Color.Black ? BlackDirections() : WhiteDirections();

        public override bool IsLongRange => false;

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