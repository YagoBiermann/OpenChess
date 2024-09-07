namespace OpenChess.Domain
{
    public class Color
    {
        public static char Black { get; } = 'b';
        public static char White { get; } = 'w';
        public char Value { get; }

        public Color(char color)
        {
            if (!(color == Black || color == White)) throw new ChessboardException("Invalid Color!");
            Value = color;
        }

        public static Color GetRandomColor()
        {
            List<char> values = [Black, White];
            Random random = new();
            return (Color)values[random.Next(values.Count)];
        }

        public static explicit operator Color(char value)
        {
            return new Color(value);
        }

        public static explicit operator Color(string value)
        {
            if (value.ToLower() == "white" || value.ToLower() == "w") return new Color(White);
            if (value.ToLower() == "black" || value.ToLower() == "b") return new Color(Black);
            throw new ChessboardException($"Could not cast the value {value} to color.");
        }

        public static explicit operator Color(int value)
        {
            if (value == 0) return new Color(White);
            if (value == 1) return new Color(Black);
            throw new ChessboardException($"Could not cast the value {value} to color.");
        }
        public static Color GetOppositeColor(Color color)
        {
            return color.Value == White ? new Color(Black) : new Color(White);
        }


        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj is Color other)
            {
                return Value == other.Value;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static bool operator ==(Color c1, Color c2)
        {
            if (c1 is null && c2 is null)
                return true;
            if (c1 is null || c2 is null)
                return false;

            return c1.Value == c2.Value;
        }

        public static bool operator !=(Color c1, Color c2)
        {
            return !(c1 == c2);
        }
    }
}