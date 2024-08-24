namespace OpenChess.Domain
{
    internal class DefaultTextMoveBuilder : PGNBuilder
    {
        private IReadOnlyPiece _movedPiece;
        private Coordinate _destination;
        public DefaultTextMoveBuilder(int count, MovePlayed move) : base(count)
        {
            _movedPiece = move.PieceMoved;
            _destination = move.Destination;
        }
        public override PGNBuilder Build()
        {
            Result = $"{AppendCount(_count)}{char.ToUpper(_movedPiece.Name)}{_destination.ToString().ToLower()}";
            if (AppendCaptureSign) { BuildCaptureSign(); }
            if (AppendCheckSign) { BuildCheckSign(); } else if (AppendCheckMateSign) { BuildCheckMateSign(); }
            return this;
        }

        protected override PGNBuilder BuildCaptureSign()
        {
            int index = Result.IndexOf(_movedPiece.Name.ToString().ToUpper());
            Result = Result.Insert(index + 1, "x");
            return this;
        }
    }
}