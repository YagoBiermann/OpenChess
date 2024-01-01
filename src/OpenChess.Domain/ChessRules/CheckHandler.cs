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

        public bool IsInCheckmate(Color player, out CheckState checkState)
        {
            if (!IsInCheck(player, out checkState)) return false;
            return !CanCheckBeSolved(player, checkState);
        }

        public bool IsInCheck(Color player, out CheckState checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CheckState.NotInCheck;
        }

        private int CalculateCheckAmount(Color player)
        {
            List<IReadOnlyPiece> pieces = _chessboard.GetPieces(ColorUtils.GetOppositeColor(player));
            int checkAmount = 0;

            foreach (IReadOnlyPiece piece in pieces) { if (_movesCalculator.IsHittingTheEnemyKing(piece)) { checkAmount++; }; }

            return checkAmount;
        }

        private static CheckState GetCheckState(int checkAmount)
        {
            return checkAmount switch
            {
                0 => CheckState.NotInCheck,
                1 => CheckState.Check,
                2 => CheckState.DoubleCheck,
                _ => throw new MatchException("The game could not compute the current check state")
            };
        }

        private bool CanCheckBeSolved(Color player, CheckState checkState)
        {
            if (checkState == CheckState.DoubleCheck) return CanSolveByMovingTheKing(player);
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