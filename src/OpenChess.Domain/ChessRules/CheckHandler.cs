namespace OpenChess.Domain
{
    internal class CheckHandler
    {
        private Dictionary<IReadOnlyPiece, List<Coordinate>> _movesTowardsTheKing = new();
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _playerInCheckMovesCalculator;
        private LegalMovesCalculator _legalMovesCalculator;
        public CheckHandler(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            _playerInCheckMovesCalculator = new PlayerInCheckMovesCalculator(chessboard);
            _legalMovesCalculator = new LegalMovesCalculator(_chessboard);
        }

        public bool IsInCheckmate(Color player, out CheckState checkState)
        {
            if (!IsInCheck(player, out checkState)) return false;
            if (checkState == CheckState.DoubleCheck) return !CanSolveCheckByMovingTheKing(player);

            return !CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(player) && !CanSolveCheckByMovingTheKing(player);
        }

        public bool IsInCheck(Color player, out CheckState checkState)
        {
            int checkAmount = CalculateCheckAmount(player);
            checkState = GetCheckState(checkAmount);

            return checkState != CheckState.NotInCheck;
        }

        private bool CanSolveCheckByCoveringTheKingOrCapturingTheEnemyPiece(Color player)
        {
            if (_movesTowardsTheKing.Count >= 2) return false;
            List<Coordinate> opponentMovesTowardsTheKing = new(_movesTowardsTheKing.First().Value);
            List<List<MoveDirections>> allLegalMovesFromPlayer = _legalMovesCalculator.CalculateAllMoves(player);
            allLegalMovesFromPlayer.RemoveAll(m => m.Where(c => c.Piece is King).Any());
            List<Coordinate> allMoves = allLegalMovesFromPlayer.SelectMany(m => m.SelectMany(c => c.Coordinates)).ToList();
            bool canBeSolved = allMoves.Intersect(opponentMovesTowardsTheKing).Any();

            return canBeSolved;
        }

        private bool CanSolveCheckByMovingTheKing(Color player)
        {
            IReadOnlyPiece king = _chessboard.FindPiece(player, typeof(King)).First();
            List<MoveDirections> kingMoves = _playerInCheckMovesCalculator.CalculateMoves(king);
            bool canBeSolved = kingMoves.SelectMany(m => m.Coordinates).ToList().Any();

            return canBeSolved;
        }

        private int CalculateCheckAmount(Color player)
        {
            List<Coordinate> piecePositions = _chessboard.GetPiecesPosition(ColorUtils.GetOppositeColor(player));
            int checkAmount = 0;

            foreach (Coordinate position in piecePositions)
            {
                IReadOnlyPiece piece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                if (IsHittingTheEnemyKing(piece)) { checkAmount++; };
            }

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

        private bool IsHittingTheEnemyKing(IReadOnlyPiece piece)
        {
            List<Coordinate> movesTowardsTheKing = CalculateMoveTowardsTheKing(piece);
            return movesTowardsTheKing.Any();
        }

        private List<Coordinate> CalculateMoveTowardsTheKing(IReadOnlyPiece piece)
        {
            if (_movesTowardsTheKing.ContainsKey(piece)) { return _movesTowardsTheKing[piece]; };
            List<MoveDirections> moves = _legalMovesCalculator.CalculateMoves(piece);
            List<Coordinate> movesTowardsTheKing = new();

            foreach (MoveDirections move in moves)
            {
                if (!move.Coordinates.Any()) continue;
                IReadOnlySquare square = _chessboard.GetReadOnlySquare(move.Coordinates.Last());
                if (square.HasPiece && square.ReadOnlyPiece is King && square.ReadOnlyPiece.Color != piece.Color)
                {
                    Coordinate kingPosition = move.Coordinates.Last();
                    movesTowardsTheKing.Add(piece.Origin);
                    movesTowardsTheKing.AddRange(move.Coordinates);
                    movesTowardsTheKing.Remove(kingPosition);
                    _movesTowardsTheKing.Add(piece, movesTowardsTheKing);
                    break;
                }
            }

            return movesTowardsTheKing;
        }
    }
}