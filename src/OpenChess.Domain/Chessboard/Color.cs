namespace OpenChess.Domain
{
    internal enum Color
    {
        Black = 'b',
        White = 'w'
    }

    internal static class ColorUtils
    {
        public static Color GetOppositeColor(Color color)
        {
            return color == Color.Black ? Color.White : Color.Black;
        }

        public static Color TryParseColor(char color)
        {
            bool colorExists = Enum.IsDefined(typeof(Color), (int)color);
            if (!colorExists) throw new MatchException($"Could not cast the value {color} to a color.");
            return (Color)color;
        }
    }
}