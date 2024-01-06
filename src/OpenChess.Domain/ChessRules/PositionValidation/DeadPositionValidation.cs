namespace OpenChess.Domain
{
    internal class DeadPositionValidation : PositionValidation
    {
        public DeadPositionValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }

        public override CheckState ValidatePosition(CheckState? checkState = null)
        {
            if (checkState != CheckState.NotInCheck) return base.ValidatePosition(checkState);
            List<IReadOnlyPiece> allPieces = _match.Chessboard.GetAllPieces();
            if (HasOnlyKings(allPieces) || HasOnlyBishopsInSameTile(allPieces, _match) || HasOnlyKnight(allPieces)) { return CheckState.Draw; }

            return base.ValidatePosition(checkState);
        }

        private static bool HasOnlyKings(List<IReadOnlyPiece> allPieces)
        {
            return !allPieces.FindAll(p => p is not King).Any();
        }
        private static bool HasOnlyBishopsInSameTile(List<IReadOnlyPiece> allPieces, Match match)
        {
            bool hasOtherPieces = allPieces.FindAll(p => p is not Bishop && p is not King).Any();
            if (hasOtherPieces) { return false; }

            var whiteSquaresHasOnlyBishops = match.Chessboard.GetReadOnlySquares().FindAll(s => s.Color == Color.White).FindAll(s => s.ReadOnlyPiece is Bishop).Any();
            var blackSquaresHasOnlyBishops = match.Chessboard.GetReadOnlySquares().FindAll(s => s.Color == Color.Black).FindAll(s => s.ReadOnlyPiece is Bishop).Any();

            return whiteSquaresHasOnlyBishops ^ blackSquaresHasOnlyBishops;
        }

        private static bool HasOnlyKnight(List<IReadOnlyPiece> allPieces)
        {
            bool thereAreNotOtherPieces = !allPieces.FindAll(p => p is not Knight && p is not King).Any();
            bool thereIsNoMoreThanOneKnight = allPieces.FindAll(p => p is Knight).Count == 1;
            return thereIsNoMoreThanOneKnight && thereAreNotOtherPieces;
        }
    }
}