
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
                throw new NotImplementedException();
            }
        }

        public override char Name => Color == Color.Black ? 'p' : 'P';

        public override List<Direction> Directions => Color == Color.Black ? BlackDirections() : WhiteDirections();

        public override bool IsLongRange => false;

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