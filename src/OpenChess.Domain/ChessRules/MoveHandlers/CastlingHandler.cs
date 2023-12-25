namespace OpenChess.Domain
{
    internal class CastlingHandler : MoveHandler
    {
        public CastlingHandler(Chessboard chessboard) : base(chessboard) { }

        public override MovePlayed Handle(Coordinate origin, Coordinate destination, string? promotingPiece = null)
        {
            if (IsCastling(origin, destination))
            {
                if (!CanCastle(destination, _chessboard.Turn)) { throw new ChessboardException("Cannot castle!"); }
                return DoCastle(origin, destination, _chessboard.Turn);
            }
            else { return base.Handle(origin, destination, promotingPiece); }
        }

        private bool IsCastling(Coordinate origin, Coordinate destination)
        {
            bool isNotKingPosition = origin.Column != 'E' || !(origin.Row == '1' || origin.Row == '8');
            if (isNotKingPosition) return false;
            IReadOnlyPiece? piece = _chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
            if (piece is not King) return false;
            if (origin.Row == '1' && !GetCastlingDestinations(Color.White).Contains(destination)) return false;
            if (origin.Row == '8' && !GetCastlingDestinations(Color.Black).Contains(destination)) return false;

            return true;
        }

        private bool CanCastle(Coordinate destination, Color player)
        {
            bool isCastlingKingSide = IsCastlingKingSide(destination, player);
            bool isCastlingQueenSide = IsCastlingQueenSide(destination, player);
            if (isCastlingKingSide && !IsKingSideCastlingAvailable(player)) return false;
            if (isCastlingQueenSide && !IsQueenSideCastlingAvailable(player)) return false;
            if (AnyPieceHittingTheCastlingSquares(GetPossiblyHittedPositions(destination, player))) return false;
            if (HasPieceInBetween(PiecesInBetween(player, isCastlingKingSide))) return false;
            if (HasKingOrRookMoved(DefaultPiecePosition(player, isCastlingKingSide))) return false;
            return true;
        }

        private MovePlayed DoCastle(Coordinate origin, Coordinate destination, Color player)
        {
            List<Coordinate> piecePositions = GetCastlingSide(destination, player);
            Coordinate kingPosition = piecePositions.First();
            Coordinate rookPosition = piecePositions.Last();
            Coordinate kingDestination = piecePositions[2];
            Coordinate rookDestination = piecePositions[1];

            Piece king = _chessboard.RemovePiece(kingPosition)!;
            Piece rook = _chessboard.RemovePiece(rookPosition)!;

            _chessboard.AddPiece(kingDestination, king.Name, king.Color);
            _chessboard.AddPiece(rookDestination, rook.Name, rook.Color);

            MoveType moveType = GetMoveType(destination, player);

            return new(origin, destination, king, null, moveType);
        }

        private MoveType GetMoveType(Coordinate destination, Color player)
        {
            return IsCastlingKingSide(destination, player) ? MoveType.KingSideCastlingMove : MoveType.QueenSideCastlingMove;
        }

        private bool IsKingSideCastlingAvailable(Color color)
        {
            if (color == Color.Black) return _chessboard.CastlingAvailability.IsBlackKingSideAvailable;
            return _chessboard.CastlingAvailability.IsWhiteKingSideAvailable;
        }

        private bool IsQueenSideCastlingAvailable(Color color)
        {
            if (color == Color.Black) return _chessboard.CastlingAvailability.IsBlackQueenSideAvailable;
            return _chessboard.CastlingAvailability.IsWhiteQueenSideAvailable;
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
            IMoveCalculator legalMoves = new LegalMovesCalculator(_chessboard);
            Color enemyPlayer = _chessboard.Opponent;
            List<Coordinate> piecePositions = _chessboard.GetPiecesPosition(enemyPlayer);
            bool isHitting = false;

            foreach (Coordinate position in piecePositions)
            {
                IReadOnlyPiece currentPiece = _chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;
                List<MoveDirections> moves;
                if (currentPiece is Pawn) { moves = currentPiece.CalculateMoveRange(); }
                else { moves = legalMoves.CalculateMoves(currentPiece); };

                foreach (MoveDirections move in moves)
                {
                    var lastPosition = move.Coordinates.LastOrDefault();
                    if (lastPosition is null) { isHitting = false; break; }
                    if (castlingPositions.Contains(lastPosition)) { isHitting = true; break; }
                }

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

        private static List<Coordinate> PiecesInBetween(Color color, bool isCastlingKingSide)
        {
            List<Coordinate> kingSidePositions = new()
            {
                GetKingSidePositions(color)[1],
                GetKingSidePositions(color)[2],
            };
            List<Coordinate> queenSidePositions = new()
            {
                GetQueenSidePositions(color)[1],
                GetQueenSidePositions(color)[2],
                GetQueenSidePositions(color)[3],
            };

            return isCastlingKingSide ? kingSidePositions : queenSidePositions;
        }

        private static List<Coordinate> DefaultPiecePosition(Color color, bool isCastlingKingSide)
        {
            List<Coordinate> kingSidePositions = new()
            {
                GetKingSidePositions(color)[0],
                GetKingSidePositions(color)[3],
            };
            List<Coordinate> queenSidePositions = new()
            {
                GetQueenSidePositions(color)[0],
                GetQueenSidePositions(color)[4],
            };

            return isCastlingKingSide ? kingSidePositions : queenSidePositions;
        }

        private static List<Coordinate> GetCastlingDestinations(Color color)
        {

            List<Coordinate> positions = new()
            {
                GetKingSidePositions(color)[2],
                GetQueenSidePositions(color)[2],
            };

            return positions;
        }

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