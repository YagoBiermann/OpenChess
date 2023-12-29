using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class ExpectedMoves
    {
        internal static PieceRangeOfAttack GetMove(Coordinate origin, Direction direction, int amount, IReadOnlyPiece piece)
        {
            PieceRangeOfAttack move = new(direction, Coordinate.CalculateSequence(origin, direction, amount), piece);

            return move;
        }
    }
}