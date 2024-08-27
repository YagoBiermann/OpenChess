namespace OpenChess.Domain
{
    public class CoordinateException : Exception
    {
        public CoordinateException() { }
        public CoordinateException(string message) : base(message) { }
        public CoordinateException(string message, Exception inner) : base(message, inner) { }
    }
}