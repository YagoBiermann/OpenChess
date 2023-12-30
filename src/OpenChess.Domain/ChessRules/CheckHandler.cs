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
            return CanCheckBeSolved(player, checkState);
        }

        public bool IsInCheck(Color player, out CheckState checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CheckState.NotInCheck;
        }


        private bool CanCheckBeSolved(Color player, CheckState checkState)
        {
            if (checkState == CheckState.DoubleCheck) return _movesCalculator.CalculateKingMoves(player).Any();
            return CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(player);
        }

        private bool CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(Color player)
        {
            List<IReadOnlyPiece> allyPieces = _chessboard.GetPieces(player);
            var enemyMovesHittingTheKing = CalculateMovesHittingTheEnemyKing(ColorUtils.GetOppositeColor(player));
            var positionsAvailableToSolveTheCheck = PositionsAvailableToSolveTheCheck(enemyMovesHittingTheKing.First());

            foreach (IReadOnlyPiece piece in allyPieces)
            {
                if (piece is King) continue;
                var moves = _movesCalculator.CalculateMoves(piece);
                var rangeOfAttackFromAllyPiece = moves.SelectMany(m => m.RangeOfAttack).ToList();

                if (rangeOfAttackFromAllyPiece.Intersect(positionsAvailableToSolveTheCheck).Any()) return true;
            }

            return false;
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

        private static List<Coordinate> PositionsAvailableToSolveTheCheck(PieceRangeOfAttack rangeOfAttackFromEnemyPieceHittingTheKing)
        {
            List<Coordinate> positionsAvailable = new(rangeOfAttackFromEnemyPieceHittingTheKing.RangeOfAttack);
            IReadOnlyPiece enemyPiece = rangeOfAttackFromEnemyPieceHittingTheKing.Piece;
            IReadOnlyPiece allyKing = rangeOfAttackFromEnemyPieceHittingTheKing.NearestPiece!;

            positionsAvailable.Add(enemyPiece.Origin);
            positionsAvailable.Remove(allyKing.Origin);

            return positionsAvailable;
        }

        private List<PieceRangeOfAttack> CalculateIntersectionWithEnemyMovesHittingTheKing(IReadOnlyPiece piece, List<Coordinate> movesTowardsTheKing)
        {
            if (piece is King) throw new ChessboardException("This method cannot handle king moves");
            List<PieceRangeOfAttack> legalMoves = _movesCalculator.CalculateMoves(piece);

            return legalMoves.FindAll(m => m.RangeOfAttack.Intersect(movesTowardsTheKing).Any());
        }

        public List<PieceRangeOfAttack> CalculateMovesHittingTheEnemyKing(Color player)
        {
            return _movesCalculator.CalculateAllMoves.Where(m => m.IsHittingTheEnemyKing && m.Piece.Color == player).ToList();
        }

    }
}