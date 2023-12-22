namespace OpenChess.Domain
{
    internal class PawnTextMoveBuilder : PGNBuilder
    {
        private Coordinate _origin;
        private Coordinate _destination;
        private bool _appendPromotingSign = false;
        private char? _promotingPiece = null;
        public PawnTextMoveBuilder(int count, MovePlayed move) : base(count)
        {
            _origin = move.Origin;
            _destination = move.Destination;
            if (move.PromotedPiece is not null)
            {
                _appendPromotingSign = true;
                _promotingPiece = char.Parse(move.PromotedPiece);
            };
        }
        public override PGNBuilder Build()
        {
            Result = AppendCount(_count) + _destination.ToString().ToLower();
            if (AppendCaptureSign) { BuildCaptureSign(); }
            if (_appendPromotingSign) { BuildPromotionSign(); }
            if (AppendCheckSign) { BuildCheckSign(); } else if (AppendCheckMateSign) { BuildCheckMateSign(); }
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