namespace OpenChess.Domain
{
    internal abstract class PGNBuilder
    {
        protected int _count;
        public bool AppendCheckSign = false;
        public bool AppendCheckMateSign = false;
        public bool AppendCaptureSign = false;
        public string Result { get; protected set; } = "";

        public PGNBuilder(int count)
        {
            _count = count;
        }

        protected string AppendCount(int count)
        {
            return Result = $"{count}. " + Result;
        }
        public PGNBuilder AppendCheckSign() { Result += "+"; return this; }
        public PGNBuilder AppendCheckMateSign() { Result += "#"; return this; }
        public abstract PGNBuilder AppendCaptureSign();
        public static string BuildKingSideCastlingString()
        {
            return "O-O";
        }
        public static string BuildQueenSideCastlingString()
        {
            return "O-O-O";
        }
        public abstract PGNBuilder Build();
    }
}