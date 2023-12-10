namespace OpenChess.Domain
{
    internal class PawnTextMoveBuilder : PGNBuilder
    {
        private Coordinate _origin;
        private Coordinate _destination;
        private bool _appendPromotingSign = false;
        private char? _promotingPiece;
        public PawnTextMoveBuilder(int count, Coordinate origin, Coordinate destination, char? promotingPiece = null) : base(count)
        {
            _origin = origin;
            _destination = destination;
            _promotingPiece = promotingPiece;
            if (promotingPiece is not null) _appendPromotingSign = true;
        }
        public override PGNBuilder Build()
        {
            Result = AppendCount(_count) + _destination.ToString().ToLower();
            return this;
        }

        protected override PGNBuilder BuildCaptureSign()
        {
            int index = Result.IndexOf(" ");
            Result = Result.Insert(index + 1, $"{_origin.Column.ToString().ToLower()}x");
            return this;
        }
        private PawnTextMoveBuilder BuildPromotionSign() { Result += $"={char.ToUpper((char)_promotingPiece!)}"; return this; }
    }
}