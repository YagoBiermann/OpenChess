namespace OpenChess.Domain
{
    internal interface IMoveCalculator
    {
        public List<MoveDirections> CalculateMoves(IReadOnlyPiece piece);
    }
}