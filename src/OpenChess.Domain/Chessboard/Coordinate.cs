using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OpenChess.Test")]
namespace OpenChess.Domain
{
    internal class Coordinate
    {
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
        public char Row;
        public char Column;

        public Coordinate(int col, int row)
        {
            Column = _columnMapping[col];
            Row = _rowMapping[row];
        }

        public override string ToString()
        {
            return $"{Column}{Row}";
        }
    }
}