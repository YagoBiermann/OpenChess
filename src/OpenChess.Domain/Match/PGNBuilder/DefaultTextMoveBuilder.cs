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
    }
}