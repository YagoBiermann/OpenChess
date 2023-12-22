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

        public static string BuildPawnPGN(int count, Coordinate origin, Coordinate destination, bool pieceWasCaptured, string? promotingPiece, CheckCondition checkCondition)
        {
            int moveCount = count;
            char? parsedPromotionPiece = promotingPiece is not null ? char.Parse(promotingPiece) : null;
            var builder = new PawnTextMoveBuilder(moveCount, origin, destination, parsedPromotionPiece);
            SetBuilderSign(builder, checkCondition, pieceWasCaptured);

            return builder.Build().Result;
        }

        public static string BuildDefaultPGN(int count, IReadOnlyPiece pieceMoved, Coordinate destination, bool pieceWasCaptured, CheckCondition checkCondition)
        {
            int moveCount = count;
            var builder = new DefaultTextMoveBuilder(moveCount, pieceMoved, destination);
            SetBuilderSign(builder, checkCondition, pieceWasCaptured);

            return builder.Build().Result;
        }

        protected static void SetBuilderSign(PGNBuilder builder, CheckCondition checkCondition, bool pieceWasCaptured)
        {
            if (pieceWasCaptured) builder.AppendCaptureSign = true;
            if (checkCondition == CheckCondition.Check) builder.AppendCheckSign = true;
            else if (checkCondition == CheckCondition.Checkmate) builder.AppendCheckMateSign = true;
        }
    }
}