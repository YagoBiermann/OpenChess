namespace OpenChess.Domain
{
    internal abstract class PositionValidation(Match match, IMoveCalculator movesCalculator) : IPositionValidation
    {
        protected IPositionValidation? _next;
        protected IMoveCalculator _movesCalculator = movesCalculator;
        protected Match _match = match;

        public IPositionValidation SetNext(IPositionValidation validation)
        {
            _next = validation;
            return _next;
        }

        public virtual Status ValidatePosition()
        {
            if (_next is null) { return new(GameStatus.InProgress, GameResult.None, CheckStatus.NotInCheck); }
            else { return _next.ValidatePosition(); }
        }
    }
}