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
        protected PGNBuilder BuildCheckSign() { Result += "+"; return this; }
        protected PGNBuilder BuildCheckMateSign() { Result += "#"; return this; }
        protected abstract PGNBuilder BuildCaptureSign();
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