namespace OpenChess.Domain
{
    public class CoordinateException : Exception
    {
        public CoordinateException() { }
        public CoordinateException(string message) : base(message) { }
        public CoordinateException(string message, Exception inner) : base(message, inner) { }
        public CoordinateException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}