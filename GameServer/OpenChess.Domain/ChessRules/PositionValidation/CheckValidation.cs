namespace OpenChess.Domain
{
    internal class CheckValidation : PositionValidation
    {
        public CheckValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }
        public override CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            var checkStatus = GetCheckStatus(_match.OpponentPlayer!.Color);
            return base.ValidatePosition(checkStatus);
        }

        public bool IsInCheck(Color player)
        {
            var checkStatus = GetCheckStatus(player);
            return checkStatus != CurrentPositionStatus.NotInCheck;
        }

        public CurrentPositionStatus GetCheckStatus(Color player)
        {
            int checkAmount = CalculateCheckAmount(player);
            var checkStatus = GetCheckState(checkAmount);

            return checkStatus;
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