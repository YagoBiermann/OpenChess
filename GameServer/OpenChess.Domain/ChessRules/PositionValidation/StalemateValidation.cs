namespace OpenChess.Domain
{
    internal class StalemateValidation : PositionValidation
    {
        public StalemateValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }

        public override CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            if (checkState != CurrentPositionStatus.NotInCheck) return base.ValidatePosition(checkState);
            Color opponentPlayer = _match.OpponentPlayerColor!.Value;
            var opponentPieces = _match.Chessboard.GetPieces(opponentPlayer);
            List<Coordinate> moves = new();

            foreach (var piece in opponentPieces)
            {
                if (piece is King)
                {
                    moves.AddRange(_movesCalculator.CalculateKingMoves(piece.Color).SelectMany(m => m.RangeOfAttack));
                    continue;
                }
                if (_movesCalculator.IsPinned(piece, out bool canCaptureTheEnemyPiece) && !canCaptureTheEnemyPiece) continue;
                moves.AddRange(_movesCalculator.CalculateLegalMoves(piece).SelectMany(m => m.RangeOfAttack));
            }

            if (!moves.Any()) return CurrentPositionStatus.Draw;
            else return base.ValidatePosition(checkState);
        }
    }
}