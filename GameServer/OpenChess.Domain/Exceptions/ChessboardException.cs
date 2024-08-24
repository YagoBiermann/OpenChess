namespace OpenChess.Domain
{
    public class ChessboardException : System.Exception
    {
        public ChessboardException() { }
        public ChessboardException(string message) : base(message) { }
        public ChessboardException(string message, System.Exception inner) : base(message, inner) { }
        public ChessboardException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}