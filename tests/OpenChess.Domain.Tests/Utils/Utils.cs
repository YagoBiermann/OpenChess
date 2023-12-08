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

        public static Direction GetDirection(string direction) => direction switch
        {
            "Up" => new Up(),
            "Down" => new Down(),
            "Left" => new Left(),
            "Right" => new Right(),
            "UpperRight" => new UpperRight(),
            "UpperLeft" => new UpperLeft(),
            "LowerRight" => new LowerRight(),
            "LowerLeft" => new LowerLeft(),
            _ => throw new Exception($"{direction} is not a valid direction!")
        };

        public static Color ColorFromChar(char color)
        {
            return color == 'b' ? Color.Black : Color.White;
        }
    }
}