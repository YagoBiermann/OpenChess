using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class ExpectedMoves
    {
        internal static MovePositions GetMove(Coordinate origin, Direction direction, int amount)
        {
            MovePositions move = new(direction, Coordinate.CalculateSequence(origin, direction, amount));

            return move;
        }
    }
}