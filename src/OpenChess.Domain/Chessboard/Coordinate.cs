using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OpenChess.Test")]
namespace OpenChess.Domain
{
    internal class Coordinate
    {
        static private HashSet<Coordinate> _cache = new();
        public char Row;
        public char Column;
        private Dictionary<int, char> _columnMapping { get; } = new Dictionary<int, char>()
        {
            {0, 'A'},
            {1, 'B'},
            {2, 'C'},
            {3, 'D'},
            {4, 'E'},
            {5, 'F'},
            {6, 'G'},
            {7, 'H'}
        };
        private Dictionary<int, char> _rowMapping { get; } = new Dictionary<int, char>()
        {
            {0, '1'},
            {1, '2'},
            {2, '3'},
            {3, '4'},
            {4, '5'},
            {5, '6'},
            {6, '7'},
            {7, '8'}
        };

        public Coordinate(int col, int row)
        {
            if (!IsValidRow(row)) { throw new CoordinateException("The row number is invalid!"); };
            if (!IsValidColumn(col)) { throw new CoordinateException("The column number is invalid!"); };
            Column = _columnMapping[col];
            Row = _rowMapping[row];
        }

        public Coordinate(string notation)
        {
            if (notation.Length > 2) throw new CoordinateException("Invalid Algebraic notation!");
            if (!IsValidColumn(notation[0]) || !IsValidRow(notation[1])) { throw new CoordinateException("Invalid Algebraic notation!"); };
            Column = notation[0];
            Row = notation[1];
        }

        public static Coordinate GetInstance(string notation)
        {
            Coordinate? coordinate = _cache.FirstOrDefault(c => c.ToString() == notation);
            if (coordinate is null)
            {
                coordinate = new(notation);
                _cache.Add(coordinate);
            }
            return coordinate;
        }
        
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (!(obj is Coordinate)) return false;
            Coordinate c = (Coordinate)obj;
            return Row == c.Row && Column == c.Column;
        }

        public override int GetHashCode() => (Column, Row).GetHashCode();

        public override string ToString()
        {
            return $"{Column}{Row}";
        }

        public bool IsValidRow(int value)
        {
            return _rowMapping.Where(kv => kv.Key.Equals(value)).ToList().Any();
        }

        public bool IsValidColumn(int value)
        {
            return _columnMapping.Where(kv => kv.Key.Equals(value)).ToList().Any();
        }

        public bool IsValidRow(char value)
        {
            return _rowMapping.Where(kv => kv.Value.Equals(value)).ToList().Any();
        }

        public bool IsValidColumn(char value)
        {
            return _columnMapping.Where(kv => kv.Value.Equals(value)).ToList().Any();
        }
    }
}