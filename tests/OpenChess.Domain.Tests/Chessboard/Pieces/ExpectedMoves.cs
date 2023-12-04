using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class ExpectedMoves
    {
        internal static MoveDirections GetMove(Coordinate origin, Direction direction, int amount)
        {
            MoveDirections move = new(direction, Coordinate.CalculateSequence(origin, direction, amount));

            return move;
        }
    }
}