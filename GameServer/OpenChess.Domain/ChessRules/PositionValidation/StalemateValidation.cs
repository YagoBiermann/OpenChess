namespace OpenChess.Domain
{
    internal class StalemateValidation(Match match, IMoveCalculator movesCalculator) : PositionValidation(match, movesCalculator)
    {
        public override Status ValidatePosition()
        {
            Color opponentPlayer = _match.OpponentPlayer!.Color;
            var opponentPieces = _match.Chessboard.GetPieces(opponentPlayer);
            List<Coordinate> moves = [];

            foreach (var piece in opponentPieces)
            {
                if (piece is King)
                {
                    moves.AddRange(_movesCalculator.CalculateKingMoves(piece.Color).SelectMany(m => m.AttackRange));
                    continue;
                }
                if (_movesCalculator.IsPinned(piece, out bool canCaptureTheEnemyPiece) && !canCaptureTheEnemyPiece) continue;
                moves.AddRange(_movesCalculator.CalculateLegalMoves(piece).SelectMany(m => m.AttackRange));
            }

            if (moves.Count == 0) return new(GameStatus.Finished, GameResult.Draw, CheckStatus.NotInCheck);
            else return base.ValidatePosition();
        }
    }
}