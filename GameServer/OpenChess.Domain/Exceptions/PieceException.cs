namespace OpenChess.Domain
{
    public class PieceException : Exception
    {
        public PieceException() { }
        public PieceException(string message) : base(message) { }
        public PieceException(string message, Exception inner) : base(message, inner) { }
        public PieceException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}