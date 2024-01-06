namespace OpenChess.Domain
{
    internal class CheckValidation : PositionValidation
    {
        public CheckValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }
        public override CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            CheckHandler checkHandler = new(_match.Chessboard, _movesCalculator);
            checkHandler.IsInCheck(_match.OpponentPlayerColor!.Value, out CurrentPositionStatus status);
            return base.ValidatePosition(status);
        }
    }
}