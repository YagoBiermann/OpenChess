namespace OpenChess.Domain
{
    internal enum Color
    {
        Black,
        White
    }

    internal static class ColorUtils
    {
        public static Color GetOppositeColor(Color color)
        {
            return color == Color.Black ? Color.White : Color.Black;
        }
    }
}