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

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj is not Direction) return false;
            Direction other = (Direction)obj;

            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public static Direction Multiply(Direction direction, int amount)
        {
            if (amount < 1) throw new ChessboardException("Amount can not be less than 1!");
            return new Direction(direction.X * amount, direction.Y * amount);
        }
    }
}