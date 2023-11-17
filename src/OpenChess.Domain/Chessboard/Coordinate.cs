using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OpenChess.Test")]
namespace OpenChess.Domain
{
    internal class Coordinate
    {
        static private HashSet<Coordinate> _cache = new();
        public char Row;
        public char Column;
        private static Dictionary<int, char> s_columnMapping = new()
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
        private static Dictionary<int, char> s_rowMapping = new()
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

        private Coordinate(int col, int row)
        {
            if (!IsValidRow(row)) { throw new CoordinateException("The row number is invalid!"); };
            if (!IsValidColumn(col)) { throw new CoordinateException("The column number is invalid!"); };
            Column = s_columnMapping[col];
            Row = s_rowMapping[row];
        }

        private Coordinate(string notation)
        {
            if (notation.Length > 2) throw new CoordinateException("Invalid Algebraic notation!");
            if (!IsValidColumn(notation[0]) || !IsValidRow(notation[1])) { throw new CoordinateException("Invalid Algebraic notation!"); };
            Column = notation[0];
            Row = notation[1];
        }

        public static Coordinate GetInstance(int col, int row)
        {
            Coordinate? coordinate = _cache.FirstOrDefault(c => c.Equals(col, row));
            if (coordinate is null)
            {
                coordinate = new(col, row);
                _cache.Add(coordinate);
            }
            return coordinate;
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

        public int RowToInt
        {
            get
            {
                return s_rowMapping.FirstOrDefault(k => k.Value == Row).Key;
            }
        }

        public int ColumnToInt
        {
            get
            {
                return s_columnMapping.FirstOrDefault(k => k.Value == Column).Key;
            }
        }

        private bool IsValidRow(int value)
        {
            return s_rowMapping.Where(kv => kv.Key.Equals(value)).ToList().Any();
        }

        private bool IsValidColumn(int value)
        {
            return s_columnMapping.Where(kv => kv.Key.Equals(value)).ToList().Any();
        }

        private bool IsValidRow(char value)
        {
            return s_rowMapping.Where(kv => kv.Value.Equals(value)).ToList().Any();
        }

        private bool IsValidColumn(char value)
        {
            return s_columnMapping.Where(kv => kv.Value.Equals(value)).ToList().Any();
        }

        private bool Equals(int col, int row)
        {
            if (!IsValidRow(row)) { throw new CoordinateException("The row number is invalid!"); };
            if (!IsValidColumn(col)) { throw new CoordinateException("The column number is invalid!"); };

            return s_columnMapping[col] == Column && s_rowMapping[row] == Row;
        }
    }
}