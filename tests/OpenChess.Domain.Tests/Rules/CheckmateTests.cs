using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CheckmateTests
    {
        [DataRow("2N5/k7/8/2Q5/7p/8/8/4K3 b - - 0 1", "A7", "B7")]
        [DataRow("8/k1R5/8/8/3B3p/8/8/4K3 b - - 0 1", "A7", "B8")]
        [TestMethod]
        public void DoubleCheck_ShouldBeSolvedByMovingTheKing(string fen, string origin, string destination)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, "DoubleCheck");
            Match match = new(matchInfo);
            Assert.AreEqual(CheckState.DoubleCheck, match.CurrentPlayerCheckState);

            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CheckState.NotInCheck, match.CurrentPlayerCheckState);
        }

        [DataRow("8/kR6/8/8/3B3p/8/8/4K3 b - - 0 1", "A7", "B7")]
        [DataRow("8/2k5/2R5/Q7/7p/8/8/4K3 b - - 0 1", "C7", "C6")]
        [TestMethod]
        public void DoubleCheck_ShouldBeSolvedByCapturingAPiece(string fen, string origin, string destination)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, "DoubleCheck");
            Match match = new(matchInfo);
            Assert.AreEqual(CheckState.DoubleCheck, match.CurrentPlayerCheckState);

            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CheckState.NotInCheck, match.CurrentPlayerCheckState);
        }

        [DataRow("8/1r6/k1R5/8/8/3BK3/8/8 b - - 0 1", "B7", "B6")]
        [TestMethod]
        public void DoubleCheck_TryingToSolveByCoveringTheKing_ShouldThrowException(string fen, string origin, string destination)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, "DoubleCheck");
            Match match = new(matchInfo);
            Assert.AreEqual(CheckState.DoubleCheck, match.CurrentPlayerCheckState);
            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("3B4/8/kR6/8/8/3BK3/8/8 b - - 0 1", "A6", "B6")]
        [TestMethod]
        public void DoubleCheck_TryingToSolveByCapturingAProtectedPieceWithTheKing_ShouldThrowException(string fen, string origin, string destination)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, "DoubleCheck");
            Match match = new(matchInfo);
            Assert.AreEqual(CheckState.DoubleCheck, match.CurrentPlayerCheckState);
            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("k7/1R6/1P6/p7/4BB2/8/5K2/8 w - - 0 1", "B7", "B8")]
        [TestMethod]
        public void DoubleCheck_WithoutSolution_ShouldFinishTheMatch(string fen, string origin, string destination)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, "DoubleCheck");
            Match match = new(matchInfo);
            Assert.IsFalse(match.HasFinished());

            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.IsTrue(match.HasFinished());
            Assert.AreEqual(match.Winner.GetValueOrDefault(), match.OpponentPlayer.GetValueOrDefault().Id);
        }
    }
}