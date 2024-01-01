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

        [DataRow("4K3/4R3/2p5/8/7P/8/8/k3r3 w - - 0 1", "E7")]
        [DataRow("4K3/4P3/8/2p5/7P/8/8/k3r3 w - - 0 1", "E7")]
        [DataRow("8/4K3/4RQ2/6b1/7p/8/8/k3r3 w - - 0 1", "F6")]
        [TestMethod]
        public void IsPinned_ShouldReturnTrue(string fen, string origin)
        {
            Chessboard chessboard = new(fen);
            IReadOnlyPiece piece = chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin)).ReadOnlyPiece!;
            Assert.IsTrue(chessboard.MovesCalculator.IsPinned(piece));
        }

        [DataRow("8/8/8/8/7P/1r2p1K1/8/k7 w - - 0 1", "E3")]
        [DataRow("8/8/8/8/7P/1r2p1K1/8/k7 w - - 0 1", "G3")]
        [DataRow("4K3/3R4/2p5/8/7P/8/8/k7 w - - 0 1", "D7")]
        [DataRow("4K3/3R4/2p5/8/7P/8/8/k3r3 w - - 0 1", "D7")]
        [DataRow("3K4/8/8/2p5/7P/8/8/kn2r3 w - - 0 1", "B1")]
        [DataRow("8/8/8/2p5/7P/2K5/3R4/k3r3 w - - 0 1", "D2")]
        [TestMethod]
        public void IsPinned_ShouldReturnFalse(string fen, string origin)
        {
            Chessboard chessboard = new(fen);
            IReadOnlyPiece piece = chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin)).ReadOnlyPiece!;
            Assert.IsFalse(chessboard.MovesCalculator.IsPinned(piece));
        }

    }
}