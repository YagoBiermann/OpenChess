namespace OpenChess.Domain
{
    internal enum Color
    {
        Black = 'b',
        White = 'w'
    }

    internal static class ColorUtils
    {
        public static Color GetRandomColor()
        {
            Array values = Enum.GetValues(typeof(Color));
            Random random = new();
            return (Color)values.GetValue(random.Next(values.Length))!;
        }

        public static Color GetOppositeColor(Color color)
        {
            return color is Color.White ? Color.Black : Color.White;
        }
        public static Color TryParseColor(char color)
        {
            bool colorExists = Enum.IsDefined(typeof(Color), (int)color);
            if (!colorExists) throw new MatchException($"Could not cast the value {color} to a color.");
            return (Color)color;
        }
    }
}