namespace OpenChess.Domain
{
    internal interface ILegalMoves
    {
        public List<MoveDirections> CalculateLegalMoves(IReadOnlyPiece piece);
    }
}