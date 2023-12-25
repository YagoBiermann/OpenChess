
namespace OpenChess.Domain
{
    internal class CheckmateCalculator : IMoveCalculator
    {
        private IReadOnlyChessboard _chessboard;
        private IMoveCalculator _legalMoveCalculator;
        public CheckmateCalculator(IReadOnlyChessboard chessboard)
        {
            _chessboard = chessboard;
            _legalMoveCalculator = new LegalMovesCalculator(_chessboard);
        }

        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece)
        {
            if (piece is Pawn pawn)
            {
                List<MoveDirections> pawnMoves = pawn.CalculateMoveRange();
                pawnMoves.RemoveAll(m => m.Direction.Equals(pawn.ForwardDirection));
                return pawnMoves;
            }

            List<MoveDirections> moves = _legalMoveCalculator.CalculateMoves(piece);
            foreach (MoveDirections move in moves)
            {
                if (!move.Coordinates.Any()) continue;
                IReadOnlyPiece? pieceAtLastPosition = _chessboard.GetReadOnlySquare(move.Coordinates.Last()).ReadOnlyPiece;
                if (pieceAtLastPosition is null) continue;
                bool isEnemyKing = pieceAtLastPosition is King && pieceAtLastPosition.Color != piece.Color;
                if (!isEnemyKing) continue;

                Coordinate? nextPosition = Coordinate.CalculateNextPosition(pieceAtLastPosition.Origin, move.Direction);
                if (nextPosition is null) continue;
                move.Coordinates.Add(nextPosition);
            }

            return moves;
        }
    }
}