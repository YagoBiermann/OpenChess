namespace OpenChess.Domain
{
    internal interface IMoveCalculatorStrategy
    {
        public bool ShouldIncludePiece(Color player, IReadOnlyPiece pieceAtLastPosition);
    }
}