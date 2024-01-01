using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class ExpectedMoves
    {
        internal static PieceLineOfSight GetLineOfSight(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, Direction direction, int amount)
        {
            var lineOfSight = Coordinate.CalculateSequence(piece.Origin, direction, amount);
            var piecesInLineOfSight = PieceDistances.CalculateDistance(piece, chessboard.GetPieces(lineOfSight));
            PieceLineOfSight move = new(piece, direction, lineOfSight, piecesInLineOfSight);

            return move;
        }
    }
}