namespace OpenChess.Domain
{
    internal class StalemateValidation : PositionValidation
    {
        public StalemateValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }

        public override CheckState ValidatePosition(CheckState? checkState = null)
        {
            if (checkState != CheckState.NotInCheck) return base.ValidatePosition(checkState);
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

            if (!moves.Any()) return CheckState.Draw;
            else return base.ValidatePosition(checkState);
        }
    }
}