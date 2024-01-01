using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class MovesCalculatorTests
    {

        [DataRow("r3k2r/2p3b1/6b1/8/1n6/2B5/2B1Q3/R3K2R w - - 0 1", "E2")]
        [DataRow("8/8/8/8/5p1P/6K1/1r6/k7 w - - 0 1", "F4")]
        [DataRow("8/8/8/5p2/7P/3k1K2/1r6/4n3 w - - 0 1", "E1")]
        [TestMethod]
        public void IsHittingTheEnemyKing_ShouldReturnTrue(string fen, string origin)
        {
            Chessboard chessboard = new(fen);
            IReadOnlyPiece piece = chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin)).ReadOnlyPiece!;
            Assert.IsTrue(chessboard.MovesCalculator.IsHittingTheEnemyKing(piece));
        }

        [DataRow("r3k2r/2p1q1b1/6b1/8/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "E2")]
        [DataRow("r3k2r/2p1q1b1/6b1/8/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "C3")]
        [DataRow("8/8/8/5p2/7P/3k1K2/1r6/4n3 w - - 0 1", "B2")]
        [TestMethod]
        public void IsHittingTheEnemyKing_ShouldReturnFalse(string fen, string origin)
        {
            Chessboard chessboard = new(fen);
            IReadOnlyPiece piece = chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin)).ReadOnlyPiece!;
            Assert.IsFalse(chessboard.MovesCalculator.IsHittingTheEnemyKing(piece));
        }

    }
}