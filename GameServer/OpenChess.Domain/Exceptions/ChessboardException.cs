namespace OpenChess.Domain
{
    public class ChessboardException : Exception
    {
        public ChessboardException() { }
        public ChessboardException(string message) : base(message) { }
        public ChessboardException(string message, Exception inner) : base(message, inner) { }
    }
}