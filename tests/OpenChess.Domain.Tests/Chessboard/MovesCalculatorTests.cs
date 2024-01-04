using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class MovesCalculatorTests
    {
        private static List<Coordinate> GetExpectedKingMoves(int move)
        {
            Dictionary<int, List<Coordinate>> expectedKingMoves = new()
            {
                { 1, new() { Coordinate.GetInstance("C6") } },
                { 2, new() { Coordinate.GetInstance("D4"), Coordinate.GetInstance("B5"), Coordinate.GetInstance("B6")} },
                { 3, new() { Coordinate.GetInstance("D4"), Coordinate.GetInstance("B5"), Coordinate.GetInstance("B6"), Coordinate.GetInstance("D6")} },
                { 4, new() { Coordinate.GetInstance("D4"), Coordinate.GetInstance("B5"), Coordinate.GetInstance("B6"), Coordinate.GetInstance("D6")} },
                { 5, new() { Coordinate.GetInstance("B4"), Coordinate.GetInstance("B5"), Coordinate.GetInstance("D6"), Coordinate.GetInstance("D5")} },
            };

            return expectedKingMoves[move];
        }

        [DataRow("r3k2r/2p3b1/6b1/8/1n6/2B5/2B1Q3/R3K2R w - - 0 1", "E2")]
        [DataRow("8/8/8/8/5p1P/6K1/1r6/k7 w - - 0 1", "F4")]
        [DataRow("8/8/8/5p2/7P/3k1K2/1r6/4n3 w - - 0 1", "E1")]
        [TestMethod]
        public void IsHittingTheEnemyKing_ShouldReturnTrue(string fen, string origin)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            IMoveCalculator movesCalculator = new MovesCalculator(match.Chessboard);
            IReadOnlyPiece piece = match.Chessboard.GetPiece(Coordinate.GetInstance(origin))!;

            Assert.IsTrue(movesCalculator.IsHittingTheEnemyKing(piece));
        }

        [DataRow("r3k2r/2p1q1b1/6b1/8/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "E2")]
        [DataRow("r3k2r/2p1q1b1/6b1/8/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "C3")]
        [DataRow("8/8/8/5p2/7P/3k1K2/1r6/4n3 w - - 0 1", "B2")]
        [TestMethod]
        public void IsHittingTheEnemyKing_ShouldReturnFalse(string fen, string origin)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            IMoveCalculator movesCalculator = new MovesCalculator(match.Chessboard);
            IReadOnlyPiece piece = match.Chessboard.GetPiece(Coordinate.GetInstance(origin))!;

            Assert.IsFalse(movesCalculator.IsHittingTheEnemyKing(piece));
        }

        [DataRow("4K3/4R3/2p5/8/7P/8/8/k3r3 w - - 0 1", "E7")]
        [DataRow("4K3/4P3/8/2p5/7P/8/8/k3r3 w - - 0 1", "E7")]
        [DataRow("8/4K3/4RQ2/6b1/7p/8/8/k3r3 w - - 0 1", "F6")]
        [TestMethod]
        public void IsPinned_ShouldReturnTrue(string fen, string origin)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            IMoveCalculator movesCalculator = new MovesCalculator(match.Chessboard);
            IReadOnlyPiece piece = match.Chessboard.GetPiece(Coordinate.GetInstance(origin))!;

            Assert.IsTrue(movesCalculator.IsPinned(piece));
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
            Match match = FakeMatch.RestoreMatch(fen);
            IMoveCalculator movesCalculator = new MovesCalculator(match.Chessboard);
            IReadOnlyPiece piece = match.Chessboard.GetPiece(Coordinate.GetInstance(origin))!;

            Assert.IsFalse(movesCalculator.IsPinned(piece));
        }

        [DataRow("8/8/8/2k3b1/3R3p/8/KQ6/4r3 b - - 0 1", 1)]
        [DataRow("8/8/8/2k3b1/3R3p/8/K1Q5/4r3 b - - 0 1", 2)]
        [DataRow("8/8/8/2kp2b1/3R3p/8/K1Q5/4r3 b - - 0 1", 3)]
        [DataRow("8/8/8/2kP2b1/3R3p/8/K1Q5/4r3 b - - 0 1", 4)]
        [TestMethod]
        public void CalculateKingMoves_ShouldReturnCorrectMoves(string fen, int testCase)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            IMoveCalculator movesCalculator = new MovesCalculator(match.Chessboard);
            var kingMoves = movesCalculator.CalculateKingMoves(match.CurrentPlayerColor!.Value).SelectMany(m => m.RangeOfAttack).ToList();

            CollectionAssert.AreEquivalent(GetExpectedKingMoves(testCase), kingMoves);
        }

    }
}