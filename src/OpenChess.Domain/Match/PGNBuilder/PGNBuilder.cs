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

        public static string BuildPawnPGN(int count, MovePlayed move, CheckState checkState)
        {
            int moveCount = count;
            var builder = new PawnTextMoveBuilder(moveCount, move);
            SetBuilderSign(builder, move, checkState);

            return builder.Build().Result;
        }

        public static string BuildDefaultPGN(int count, MovePlayed move, CheckState checkState)
        {
            int moveCount = count;
            var builder = new DefaultTextMoveBuilder(moveCount, move);
            SetBuilderSign(builder, move, checkState);

            return builder.Build().Result;
        }

        protected static void SetBuilderSign(PGNBuilder builder, MovePlayed move, CheckState checkState)
        {
            if (move.PieceCaptured is not null) builder.AppendCaptureSign = true;
            bool IsInCheck = checkState == CheckState.Check || checkState == CheckState.DoubleCheck;
            if (IsInCheck) builder.AppendCheckSign = true;
            else if (checkState == CheckState.Checkmate) builder.AppendCheckMateSign = true;
        }
    }
}