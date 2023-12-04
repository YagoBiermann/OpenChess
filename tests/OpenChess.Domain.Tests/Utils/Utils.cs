using OpenChess.Domain;

namespace OpenChess.Tests
{
    internal static class Utils
    {
        public static Type? GetPieceType(char piece)
        {
            Type? pieceType;

            if (char.ToLower(piece) == 'k') { pieceType = typeof(King); }
            else if (char.ToLower(piece) == 'p') { pieceType = typeof(Pawn); }
            else if (char.ToLower(piece) == 'n') { pieceType = typeof(Knight); }
            else if (char.ToLower(piece) == 'b') { pieceType = typeof(Bishop); }
            else if (char.ToLower(piece) == 'r') { pieceType = typeof(Rook); }
            else if (char.ToLower(piece) == 'q') { pieceType = typeof(Queen); }
            else { pieceType = null; }

            return pieceType;
        }
    }
}