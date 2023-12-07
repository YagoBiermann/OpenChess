namespace OpenChess.Domain
{
    internal class DefaultTextMoveBuilder : PGNBuilder
    {
        private IReadOnlyPiece _movedPiece;
        private Coordinate _destination;
        public DefaultTextMoveBuilder(int count, IReadOnlyPiece piece, Coordinate destination) : base(count)
        {
            _movedPiece = piece;
            _destination = destination;
        }
        public override PGNBuilder Build()
        {
            Result = $"{AppendCount(_count)}{char.ToUpper(_movedPiece.Name)}{_destination.ToString().ToLower()}";
            return this;
        }

        public override PGNBuilder AppendCaptureSign()
        {
            int index = Result.IndexOf(_movedPiece.Name.ToString().ToUpper());
            Result = Result.Insert(index + 1, "x");
            return this;
        }
    }
}