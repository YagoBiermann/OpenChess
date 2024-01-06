namespace OpenChess.Domain
{
    internal class CheckValidation : PositionValidation
    {
        public CheckValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }
        public override CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            IsInCheck(_match.OpponentPlayerColor!.Value, out CurrentPositionStatus status);
            return base.ValidatePosition(status);
        }

        public bool IsInCheck(Color player, out CurrentPositionStatus checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CurrentPositionStatus.NotInCheck;
        }

        private int CalculateCheckAmount(Color player)
        {
            List<IReadOnlyPiece> pieces = _match.Chessboard.GetPieces(ColorUtils.GetOppositeColor(player));
            int checkAmount = 0;

            foreach (IReadOnlyPiece piece in pieces) { if (_movesCalculator.IsHittingTheEnemyKing(piece)) { checkAmount++; }; }

            return checkAmount;
        }

        private static CurrentPositionStatus GetCheckState(int checkAmount)
        {
            return checkAmount switch
            {
                0 => CurrentPositionStatus.NotInCheck,
                1 => CurrentPositionStatus.Check,
                2 => CurrentPositionStatus.DoubleCheck,
                _ => throw new MatchException("The game could not compute the current check state")
            };
        }
    }
}