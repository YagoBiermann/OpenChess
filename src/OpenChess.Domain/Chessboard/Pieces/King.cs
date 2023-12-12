
namespace OpenChess.Domain
{
    internal class King : Piece
    {
        public King(Color color, Coordinate origin) : base(color, origin)
        {
        }

        public override char Name => Color == Color.Black ? 'k' : 'K';

        public override List<Direction> Directions
        {
            get
            {
                List<Direction> directions = new()
                {
                    new Up(),
                    new Down(),
                    new Left(),
                    new Right(),
                    new UpperLeft(),
                    new UpperRight(),
                    new LowerLeft(),
                    new LowerRight()
                };

                return directions;
            }
        }

        public override bool IsLongRange => false;
    }
}