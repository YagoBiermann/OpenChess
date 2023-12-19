namespace OpenChess.Domain
{
    internal interface IMoveCalculatorStrategy
    {
        public List<Coordinate> Calculate(IReadOnlyChessboard chessboard, IReadOnlyPiece piece, List<Coordinate> rangeOfAttack);
    }
}