namespace OpenChess.Domain
{
    internal class PawnTextMoveBuilder : PGNBuilder
    {
        private Coordinate _origin;
        private Coordinate _destination;
        public PawnTextMoveBuilder(int count, Coordinate origin, Coordinate destination) : base(count)
        {
            _origin = origin;
            _destination = destination;
        }
        public override PGNBuilder Build()
        {
            Result = AppendCount(_count) + _destination.ToString().ToLower();
            return this;
        }
    }
}