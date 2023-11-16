using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("OpenChess.Test")]
namespace OpenChess.Domain
{
    internal class Direction
    {
        private int _x { get; }
        private int _y { get; }

        public Direction(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public int X
        {
            get => _x;
        }

        public int Y
        {
            get => _y;
        }
    }
}