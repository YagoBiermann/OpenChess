namespace OpenChess.Domain
{
    internal class StalemateValidation(Match match, IMoveCalculator movesCalculator) : PositionValidation(match, movesCalculator)
    {
        public override CurrentPositionStatus ValidatePosition()
        {
            Color opponentPlayer = _match.OpponentPlayer!.Color;
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
            else return base.ValidatePosition();
        }
    }
}