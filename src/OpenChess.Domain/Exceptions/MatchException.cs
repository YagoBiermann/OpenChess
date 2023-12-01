namespace OpenChess.Domain
{
    public class MatchException : System.Exception
    {
        public MatchException() {}
        public MatchException(string message) : base(message) {}
        public MatchException(string message, System.Exception inner) : base(message, inner) {}
        public MatchException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context) {}
    }
}