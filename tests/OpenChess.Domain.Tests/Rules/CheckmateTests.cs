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
        
    }
}