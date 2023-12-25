using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CheckmateTests
    {
        [DataRow("k7/1R6/1P6/p7/4BB2/8/5K2/8 w - - 0 1", "B7", "B8")]
        [TestMethod]
        public void Play_MoveResultingInCheckmate_ShouldEndTheMatchAndDeclareWinner(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);

            Assert.IsTrue(match.HasFinished());
            Assert.AreEqual(match.Winner.GetValueOrDefault(), match.OpponentPlayer.GetValueOrDefault().Id);
        }

        [DataRow("8/8/2k1P3/8/8/1Q2K3/7p/8 w - - 0 1", "B3", "C3")]
        [TestMethod]
        public void Play_MoveResultingInCheckWithSolution_ShouldKeepMatchInProgress(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);

            Assert.IsFalse(match.HasFinished());
            Assert.IsNull(match.Winner);
            Assert.AreNotEqual(CheckState.NotInCheck, match.CurrentPlayerCheckState);
            Assert.AreNotEqual(CheckState.Checkmate, match.CurrentPlayerCheckState);
        }
    }
}