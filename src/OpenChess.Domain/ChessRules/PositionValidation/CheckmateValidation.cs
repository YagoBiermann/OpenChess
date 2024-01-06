namespace OpenChess.Domain
{
    internal class CheckmateValidation : PositionValidation
    {
        public CheckmateValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }
        public override CheckState ValidatePosition(CheckState? checkState = null)
        {
            if (!(checkState == CheckState.Check || checkState == CheckState.DoubleCheck)) return base.ValidatePosition(checkState);
            CheckHandler checkHandler = new(_match.Chessboard, _movesCalculator);
            bool isInCheckmate = checkHandler.IsInCheckmate(_match.OpponentPlayerColor!.Value, out CheckState status);

            if (isInCheckmate) return CheckState.Checkmate;
            else { return base.ValidatePosition(status); }
        }
    }
}