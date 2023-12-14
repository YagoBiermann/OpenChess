namespace OpenChess.Domain
{
    internal class CastlingHandler : MoveHandler
    {
        public CastlingHandler(Chessboard chessboard) : base(chessboard) { }

        public override HandledMove Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (IsCastling(origin, destination))
            {
                return null;
            }
            else { return base.Handle(origin, destination, promotingPiece); }
        }

        private bool IsCastling(Coordinate origin, Coordinate destination)
        {
            bool isNotKingPosition = origin.Column != 'E' || !(origin.Row == '1' || origin.Row == '8');
            if (isNotKingPosition) return false;
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
            if (piece is not King) return false;
            if (origin.Row == '1' && !WhiteCastlingPositions.Contains(destination)) return false;
            if (origin.Row == '8' && !BlackCastlingPositions.Contains(destination)) return false;

            return true;
        }

        {
        private bool IsKingSideCastlingAvailable(Color color)
        {
            if (color == Color.Black) return _chessboard.CastlingAvailability.HasBlackKingSide;
            return _chessboard.CastlingAvailability.HasWhiteKingSide;
        }

        private bool IsQueenSideCastlingAvailable(Color color)
        {
            if (color == Color.Black) return _chessboard.CastlingAvailability.HasBlackQueenSide;
            return _chessboard.CastlingAvailability.HasWhiteQueenSide;
        }

        private bool HasKingOrRookMoved(List<Coordinate> defaultPositions)
        {
            return !HasPiece(defaultPositions);
        }

        private bool HasPieceInBetween(List<Coordinate> castlingPositions)
        {
            return HasPiece(castlingPositions);
        }

        private bool HasPiece(List<Coordinate> castlingPositions)
        {
            bool hasPiece = false;
            foreach (Coordinate position in castlingPositions)
            {
                hasPiece = _chessboard.GetReadOnlySquare(position).HasPiece;
                if (hasPiece) break;
            }

            return hasPiece;
        }

        private bool AnyPieceHittingTheCastlingSquares(List<Coordinate> castlingPositions)
        {
            ILegalMoves legalMoves = new LegalMoves(_chessboard);
            Color enemyPlayer = ColorUtils.GetOppositeColor(_chessboard.Turn);
            List<Coordinate> piecePositions = _chessboard.GetPiecesPosition(enemyPlayer);
            bool isHitting = false;

            foreach (Coordinate position in piecePositions)
            {
                IReadOnlyPiece currentPiece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                isHitting = Check.IsHittingTheEnemyKing(currentPiece, _chessboard);
                if (isHitting) break;

                List<MoveDirections> moves = legalMoves.CalculateLegalMoves(currentPiece);
                castlingPositions.ForEach(p =>
                {
                    isHitting = moves.Where(m => m.Coordinates.Contains(p)).Any();
                    if (isHitting) return;
                });
                if (isHitting) break;
            }

            return isHitting;
        }

        private bool IsCastlingKingSide(Coordinate destination, Color player)
        {
            return GetKingSidePositions(player).Contains(destination);
        }

        private bool IsCastlingQueenSide(Coordinate destination, Color player)
        {
            return GetQueenSidePositions(player).Contains(destination);
        }

        private List<Coordinate> GetCastlingSide(Coordinate destination, Color player)
        {
            if (IsCastlingKingSide(destination, player)) return GetKingSidePositions(player);
            if (IsCastlingQueenSide(destination, player)) return GetQueenSidePositions(player);
            throw new ChessboardException("Castling side could not be determined");
        }

        private List<Coordinate> GetPossiblyHittedPositions(Coordinate destination, Color color)
        {
            List<Coordinate> positions = GetCastlingSide(destination, color);
            List<Coordinate> hittenPositions = new() { positions[0], positions[1], positions[2] };

            return hittenPositions;
        }
            return isCastlingKingSide ? kingSidePositions : queenSidePositions;
        }

        }

        {
            {
        private static List<Coordinate> GetQueenSidePositions(Color player)
        {
            string row = player == Color.Black ? "8" : "1";


            List<Coordinate> positions = new()
                {
                    Coordinate.GetInstance($"E{row}"),
                    Coordinate.GetInstance($"D{row}"),
                    Coordinate.GetInstance($"C{row}"),
                    Coordinate.GetInstance($"B{row}"),
                    Coordinate.GetInstance($"A{row}"),
                };

            return positions;
        }

        private static List<Coordinate> GetKingSidePositions(Color player)
        {
            string row = player == Color.Black ? "8" : "1";


            List<Coordinate> positions = new()
                {
                    Coordinate.GetInstance($"E{row}"),
                    Coordinate.GetInstance($"F{row}"),
                    Coordinate.GetInstance($"G{row}"),
                    Coordinate.GetInstance($"H{row}"),
                };

            return positions;
        }
    }
}