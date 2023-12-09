namespace OpenChess.Domain
{
    internal static class EnPassantHandler
    {
        public static IReadOnlyPiece? Handle(Coordinate origin, Coordinate destination, Chessboard chessboard)
        {
            IReadOnlyPiece? piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;
            if (piece is not Pawn pawn) return null;
            if (chessboard.EnPassant.IsEnPassantMove(origin, destination) && pawn.CanCaptureByEnPassant)
            {
                chessboard.ChangePiecePosition(origin, destination);

                Coordinate vulnerablePawnPosition = chessboard.EnPassant.GetVulnerablePawn!.Origin;
                chessboard.RemovePiece(vulnerablePawnPosition);

            }

            return piece;
        }
    }
}