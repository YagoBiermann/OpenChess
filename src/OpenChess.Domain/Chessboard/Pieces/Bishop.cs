
namespace OpenChess.Domain
{
    internal class Bishop : Piece
    {
        public Bishop(Color color, Coordinate origin, Chessboard chessboard) : base(color, origin, chessboard)
        {
        }

        public override char Name => Color == Color.Black ? 'b' : 'B';

        public override bool IsLongRange => true;

        public override List<Direction> Directions
        {
            get
            {
                List<Direction> directions = new()
                {
                    new UpperLeft(),
                    new UpperRight(),
                    new LowerLeft(),
                    new LowerRight()
                };

                return directions;
            }
        }
    }
}