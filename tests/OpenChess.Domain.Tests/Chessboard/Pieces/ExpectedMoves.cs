using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class ExpectedMoves
    {
        internal static Move GetMove(Coordinate origin, Direction direction, int amount)
        {
            Move move = new(direction, Coordinate.CalculateSequence(origin, direction, amount));

            return move;
        }
    }
}