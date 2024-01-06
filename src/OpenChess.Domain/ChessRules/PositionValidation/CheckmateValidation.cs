namespace OpenChess.Domain
{
    internal class CheckmateValidation : PositionValidation
    {
        public CheckmateValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }
        public override CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            if (!(checkState == CurrentPositionStatus.Check || checkState == CurrentPositionStatus.DoubleCheck)) return base.ValidatePosition(checkState);
            CheckHandler checkHandler = new(_match.Chessboard, _movesCalculator);
            bool isInCheckmate = checkHandler.IsInCheckmate(_match.OpponentPlayerColor!.Value, out CurrentPositionStatus status);

            if (isInCheckmate) return CurrentPositionStatus.Checkmate;
            else { return base.ValidatePosition(status); }
        }
    }
}