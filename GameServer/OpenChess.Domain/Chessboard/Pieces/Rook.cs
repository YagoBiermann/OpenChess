
namespace OpenChess.Domain
{
    internal class Rook : Piece
    {
        public Rook(Color color, Coordinate origin) : base(color, origin)
        {
        }

        public override char Name => Color == Color.Black ? 'r' : 'R';
        public override List<Direction> Directions
        {
            get
            {
                List<Direction> directions = new()
                {
                    new Up(),
                    new Down(),
                    new Left(),
                    new Right()
                };

                return directions;
            }
        }

        public override bool IsLongRange => true;
    }
}