namespace OpenChess.Domain
{
    internal class CheckmateValidation : PositionValidation
    {
        public CheckmateValidation(Match match, IMoveCalculator movesCalculator) : base(match, movesCalculator)
        {
        }
        
        public override CurrentPositionStatus ValidatePosition(CurrentPositionStatus? checkState = null)
        {
            if (!(checkState == CurrentPositionStatus.Check || checkState == CurrentPositionStatus.DoubleCheck)) return base.ValidatePosition(checkState);

            if (IsInCheckmate(_match.OpponentPlayerColor!.Value, checkState.Value)) return CurrentPositionStatus.Checkmate;
            else { return base.ValidatePosition(checkState); }
        }

        private bool IsInCheckmate(Color player, CurrentPositionStatus checkState)
        {
            if (CanCheckBeSolved(player, checkState)) return false;
            return true;
        }

        private bool CanCheckBeSolved(Color player, CurrentPositionStatus checkState)
        {
            if (checkState == CurrentPositionStatus.DoubleCheck) return CanSolveByMovingTheKing(player);
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
                List<PieceRangeOfAttack> moves = new();
                moves.AddRange(_movesCalculator.CalculateLegalMoves(piece));
                var rangeOfAttackFromAllyPiece = moves.SelectMany(m => m.RangeOfAttack).ToList();
                if (rangeOfAttackFromAllyPiece.Intersect(positionsAvailableToSolveTheCheck).Any()) return true;
            }

            return false;
        }

        private static List<Coordinate> PositionsAvailableToSolveTheCheck(PieceRangeOfAttack rangeOfAttackFromEnemyPieceHittingTheKing)
        {
            List<Coordinate> positionsAvailable = new(rangeOfAttackFromEnemyPieceHittingTheKing.RangeOfAttack);
            IReadOnlyPiece enemyPiece = rangeOfAttackFromEnemyPieceHittingTheKing.Piece;
            IReadOnlyPiece allyKing = rangeOfAttackFromEnemyPieceHittingTheKing.NearestPiece!;

            positionsAvailable.Add(enemyPiece.Origin);
            positionsAvailable.Remove(allyKing.Origin);

            return positionsAvailable;
        }

        private List<PieceRangeOfAttack> CalculateMovesHittingTheEnemyKing(Color player)
        {
            return _movesCalculator.CalculateAllMoves().Where(m => m.IsHittingTheEnemyKing && m.Piece.Color == player).ToList();
        }
    }
}