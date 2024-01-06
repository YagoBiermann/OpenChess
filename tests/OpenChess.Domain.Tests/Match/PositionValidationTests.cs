using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PositionValidationTests
    {
        [DataRow("8/8/8/p7/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "Stalemate")]
        [DataRow("8/8/2b5/p2R4/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "Stalemate/pinned piece without solution")]
        [DataRow("8/8/8/8/8/8/3Kp1pp/5brk w - - 0 1", "D2", "E1", DisplayName = "Stalemate")]
        [DataRow("k7/P7/K7/8/8/8/8/4B3 w - - 0 1", "E1", "G3", DisplayName = "Stalemate")]
        [DataRow("8/8/8/8/2k5/rRn5/p7/K7 b - - 0 1", "C4", "B3", DisplayName = "Stalemate")]
        [DataRow("8/8/3k4/3P4/8/4K3/8/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/Only kings left")]
        [DataRow("8/8/3k4/3Q4/8/4K3/8/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/Only kings left")]
        [DataRow("8/8/3k4/3B4/8/4K3/3B4/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/king + bishop left")]
        [DataRow("7B/8/3k4/3B4/8/4K3/3B4/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/king + bishop left")]
        [DataRow("8/8/3k4/3b4/8/2bK4/8/8 w - - 0 1", "D3", "C3", DisplayName = "Dead Position/king + bishop left")]
        [DataRow("8/8/3k4/1B6/8/5p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Dead Position/bishops in same tile")]
        [DataRow("8/8/3k4/1B6/8/5p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Dead Position/bishops in same tile")]
        [DataRow("8/1B1B1b2/3k4/1B6/2b5/5p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Dead Position/bishops in same tile")]
        [DataRow("8/8/2k1n3/2P5/8/4K3/8/8 b - - 0 1", "C6", "C5", DisplayName = "Dead Position/king + knight left")]
        [DataRow("8/8/2k2N2/2P5/8/4K3/8/8 b - - 0 1", "C6", "C5", DisplayName = "Dead Position/king + knight left")]
        [TestMethod]
        public void Play_MoveResultingInDraw_ShouldFinishMatchWithoutWinner(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);

            Assert.IsNull(match.Winner);
            Assert.AreEqual(CheckState.Draw, match.CurrentPlayerCheckState);
            Assert.IsTrue(match.HasFinished());
        }

        [DataRow("8/8/2k2N2/2P5/6N1/4K3/8/8 b - - 0 1", "C6", "C5", DisplayName = "More than one knight left")]
        [DataRow("8/8/3k4/1B6/8/p4p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Not only bishops left")]
        [DataRow("8/8/3k4/1B6/8/2b2p2/5K2/8 w - - 0 1", "F2", "F3", DisplayName = "only bishops left but in different tiles")]
        [DataRow("8/8/8/8/2k5/rR6/p7/K7 b - - 0 1", "A3", "B3", DisplayName = "not stalemate")]
        [DataRow("8/8/2b5/p2B4/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "not stalemate/pinned piece with solution")]
        [DataRow("8/8/2b5/p2P4/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "not stalemate/pinned piece with solution")]
        [DataRow("7r/8/8/p7/P6P/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "not stalemate/pinned piece with solution")]
        [TestMethod]
        public void Play_MoveNotResultingInDraw_ShouldKeepMatchGoing(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.IsFalse(match.HasFinished());
        }
    }
}