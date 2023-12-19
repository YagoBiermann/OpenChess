
namespace OpenChess.Domain
{
    internal class PawnLegalMoves : IMoveCalculatorStrategy
    {
        public List<Coordinate> Calculate(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, List<Coordinate> rangeOfAttack)
        {
            if (piece is not Pawn) throw new Exception("This class only handles pawn moves");
            Pawn pawn = (Pawn)piece;
            List<Coordinate> legalMoves = new(rangeOfAttack);
            Coordinate? enPassant = chessboard.EnPassantAvailability.EnPassantPosition;
            int lastPosition = legalMoves.Count - 1;
            if (!legalMoves.Any()) return legalMoves;

            IReadOnlySquare square = chessboard.GetReadOnlySquare(legalMoves.Last());
            bool isEnPassant = square.Coordinate.Equals(enPassant);
            bool hasAllyPiece = !square.HasEnemyPiece(pawn.Color);
            bool hasKing = square.HasTypeOfPiece(typeof(King));
            if (hasAllyPiece || hasKing || !square.HasPiece || !isEnPassant) { legalMoves.RemoveAt(lastPosition); }

            return legalMoves;
        }
    }
}