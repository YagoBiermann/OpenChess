namespace OpenChess.Domain
{
    internal class CheckmateValidation(Match match, IMoveCalculator movesCalculator, CheckValidation checkValidation) : PositionValidation(match, movesCalculator)
    {
        private CheckValidation _checkValidation = checkValidation;

        public override Status ValidatePosition()
        {
            CheckStatus checkStatus = _checkValidation.GetCheckStatus(_match.OpponentPlayer!.Color);
            if (checkStatus == CheckStatus.NotInCheck) return base.ValidatePosition();
            if (IsInCheckmate(_match.OpponentPlayer!.Color, checkStatus)) return new(GameStatus.Finished, GameResult.Checkmate, checkStatus);
            else { return base.ValidatePosition(); }
        }

        private bool IsInCheckmate(Color player, CheckStatus checkStatus)
        {
            return !CanCheckBeSolved(player, checkStatus);
        }

        private bool CanCheckBeSolved(Color player, CheckStatus checkStatus)
        {
            if (checkStatus == CheckStatus.DoubleCheck) return CanSolveByMovingTheKing(player);
            return CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(player) || CanSolveByMovingTheKing(player);
        }

        private bool CanSolveByMovingTheKing(Color player)
        {
            return _movesCalculator.CalculateKingMoves(player).Any();
        }

        private bool CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(Color player)
        {
            List<IReadOnlyPiece> allyPieces = _match.Chessboard.GetPieces(player);
            var enemyMovesHittingTheKing = CalculateMovesHittingTheEnemyKing(ColorUtils.GetOppositeColor(player));
            var positionsAvailableToSolveTheCheck = PositionsAvailableToSolveTheCheck(enemyMovesHittingTheKing.First());

            foreach (IReadOnlyPiece piece in allyPieces)
            {
                if (piece is King) continue;
                if (_movesCalculator.IsPinned(piece, out bool canCaptureTheEnemyPiece)) continue;
                List<PieceAttackRange> moves = [.. _movesCalculator.CalculateLegalMoves(piece)];
                var rangeOfAttackFromAllyPiece = moves.SelectMany(m => m.AttackRange).ToList();
                if (rangeOfAttackFromAllyPiece.Intersect(positionsAvailableToSolveTheCheck).Any()) return true;
            }

            return false;
        }

        private static List<Coordinate> PositionsAvailableToSolveTheCheck(PieceAttackRange rangeOfAttackFromEnemyPieceHittingTheKing)
        {
            List<Coordinate> positionsAvailable = new(rangeOfAttackFromEnemyPieceHittingTheKing.AttackRange);
            IReadOnlyPiece enemyPiece = rangeOfAttackFromEnemyPieceHittingTheKing.Piece;
            IReadOnlyPiece allyKing = rangeOfAttackFromEnemyPieceHittingTheKing.NearestPiece!;

            positionsAvailable.Add(enemyPiece.Origin);
            positionsAvailable.Remove(allyKing.Origin);

            return positionsAvailable;
        }

        private List<PieceAttackRange> CalculateMovesHittingTheEnemyKing(Color player)
        {
            return _movesCalculator.CalculateAllMoves().Where(m => m.IsHittingTheEnemyKing && m.Piece.Color == player).ToList();
        }
    }
}