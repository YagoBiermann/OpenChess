namespace OpenChess.Domain
{
    internal static class EnPassantHandler
    {
        public static bool IsEnPassantMove(Coordinate origin, Coordinate destination, IReadOnlyChessboard chessboard)
        {
            IReadOnlyPiece? piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;

            if (piece is not Pawn) return false;
            if (destination == chessboard.EnPassant.Position) return true;
            return false;
        }

        public static IReadOnlyPiece? Handle(Coordinate origin, Coordinate destination, Chessboard chessboard)
        {
            IReadOnlyPiece? piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
            if (piece is not Pawn pawn) return null;
            if (IsEnPassantMove(origin, destination, chessboard) && pawn.CanCaptureByEnPassant)
            {
                chessboard.ChangePiecePosition(origin, destination);

                Coordinate vulnerablePawnPosition = chessboard.EnPassant.GetVulnerablePawn!.Origin;
                chessboard.RemovePiece(vulnerablePawnPosition);

            }

            return piece;
        }
    }
}