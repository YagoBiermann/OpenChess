namespace OpenChess.Domain
{
    internal class MoveCounterValidation : IPositionValidation
    {
        private Match _match;
        private IPositionValidation? _next;
        public MoveCounterValidation(Match match)
        {
            _match = match;
        }

        public IPositionValidation SetNext(IPositionValidation validation)
        {
            _next = validation;
            return validation;
        }

        public CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            bool halfMoveCounterHits100 = _match.HalfMove == 100;
            if (halfMoveCounterHits100) return CurrentPositionStatus.Draw;
            else return _next!.ValidatePosition();
        }
    }
}