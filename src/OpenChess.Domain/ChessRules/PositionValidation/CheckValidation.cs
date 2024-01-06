namespace OpenChess.Domain
{
    internal class CheckValidation : PositionValidation
    {
        public CheckValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }
        public override CheckState ValidatePosition(CheckState? checkState = null)
        {
            CheckHandler checkHandler = new(_match.Chessboard, _movesCalculator);
            checkHandler.IsInCheck(_match.OpponentPlayerColor!.Value, out CheckState status);
            return base.ValidatePosition(status);
        }
    }
}