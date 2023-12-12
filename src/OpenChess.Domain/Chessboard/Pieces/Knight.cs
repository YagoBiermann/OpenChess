
namespace OpenChess.Domain
{
    internal class Knight : Piece
    {
        public Knight(Color color, Coordinate origin) : base(color, origin)
        {
        }

        public override char Name => Color == Color.Black ? 'n' : 'N';
        public override bool IsLongRange => false;

        public override List<Direction> Directions
        {
            get
            {
                List<Direction> directions = new()
                {
                    new Direction(1,2),
                    new Direction(-1,2),
                    new Direction(1,-2),
                    new Direction(-1,-2),
                    new Direction(2,1),
                    new Direction(2,-1),
                    new Direction(-2,-1),
                    new Direction(-2,1)
                };

                return directions;
            }
        }
    }
}