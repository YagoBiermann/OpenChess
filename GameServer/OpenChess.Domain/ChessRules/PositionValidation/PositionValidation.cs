namespace OpenChess.Domain
{
    internal abstract class PositionValidation : IPositionValidation
    {
        protected IPositionValidation? _next;
        protected IMoveCalculator _movesCalculator;
        protected Match _match;

        public PositionValidation(Match match, IMoveCalculator movesCalculator)
        {
            _match = match;
            _movesCalculator = movesCalculator;
        }

        public IPositionValidation SetNext(IPositionValidation validation)
        {
            _next = validation;
            return _next;
        }

        public virtual CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            if (_next is null) { return checkState ?? CurrentPositionStatus.NotInCheck; }
            else { return _next.ValidatePosition(checkState); }
        }
    }
}