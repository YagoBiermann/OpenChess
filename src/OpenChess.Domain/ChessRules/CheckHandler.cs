namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _movesCalculator;
        public CheckHandler(IReadOnlyChessboard chessboard, IMoveCalculator movesCalculator)
        {
            _chessboard = chessboard;
            _movesCalculator = movesCalculator;
        }

        public bool IsInCheckmate(Color player, out CurrentPositionStatus checkState)
        {
            if (!IsInCheck(player, out checkState)) return false; //remove this
            if (CanCheckBeSolved(player, checkState)) return false;
            checkState = CurrentPositionStatus.Checkmate;
            return true;
        }

        public bool IsInCheck(Color player, out CurrentPositionStatus checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CurrentPositionStatus.NotInCheck;
        }

        private int CalculateCheckAmount(Color player)
        {
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(ColorUtils.GetOppositeColor(player));
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
            List<IReadOnlyPiece> allyPieces = _chessboard.GetPieces(player);
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