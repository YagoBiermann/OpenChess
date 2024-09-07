namespace OpenChess.Domain
{
    internal class CheckValidation(Match match, IMoveCalculator movesCalculator) : PositionValidation(match, movesCalculator)
    {
        public override Status ValidatePosition()
        {
            CheckStatus checkStatus = GetCheckStatus(_match.OpponentPlayer!.Color);
            bool isInCheck = checkStatus != CheckStatus.NotInCheck;
            if (isInCheck) return new(GameStatus.InProgress, GameResult.None, checkStatus);
            else { return base.ValidatePosition(); }
        }

        public bool IsInCheck(Color player)
        {
            var checkStatus = GetCheckStatus(player);
            return checkStatus != CheckStatus.NotInCheck;
        }

        public CheckStatus GetCheckStatus(Color player)
        {
            int checkAmount = CalculateCheckAmount(player);
            var checkStatus = ParseCheckAmountToCheckStatus(checkAmount);

            return checkStatus;
        }

        private int CalculateCheckAmount(Color player)
        {
            List<IReadOnlyPiece> pieces = _match.Chessboard.GetPieces(ColorUtils.GetOppositeColor(player));
            int checkAmount = 0;

            foreach (IReadOnlyPiece piece in pieces) { if (_movesCalculator.IsHittingTheEnemyKing(piece)) { checkAmount++; }; }

            return checkAmount;
        }

        private static CheckStatus ParseCheckAmountToCheckStatus(int checkAmount)
        {
            return checkAmount switch
            {
                0 => CheckStatus.NotInCheck,
                1 => CheckStatus.Check,
                2 => CheckStatus.DoubleCheck,
                _ => throw new MatchException("The game could not compute the current check state")
            };
        }
    }
}