using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PromotionTests
    {
        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/8 b - - 0 1", "F2", "F1", "K")]
        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/8 b - - 0 1", "F2", "F1", "P")]
        [TestMethod]
        public void Play_PromotingPawnToAnInvalidPiece_ShouldThrowException(string fen, string position1, string position2, string promotingPiece)
        {
            Assert.ThrowsException<ChessboardException>(() => FakeMatch.RestoreAndPlay(fen, position1, position2, promotingPiece));
        }

        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/4R3 b - - 0 1", "F2", "E1", "", "8/8/8/3r4/2KP4/8/2k5/4q3 w - - 0 1")]
        [DataRow("4r3/5P2/8/8/2K5/8/2kR4/8 w - - 0 1", "F7", "E8", "", "4Q3/8/8/8/2K5/8/2kR4/8 b - - 0 1")]
        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/8 b - - 0 1", "F2", "F1", "Q", "8/8/8/3r4/2KP4/8/2k5/5q2 w - - 0 1")]
        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/4R3 b - - 0 1", "F2", "E1", "N", "8/8/8/3r4/2KP4/8/2k5/4n3 w - - 0 1")]
        [DataRow("4r3/5P2/8/8/2K5/8/2kR4/8 w - - 0 1", "F7", "F8", "B", "4rB2/8/8/8/2K5/8/2kR4/8 b - - 0 1")]
        [DataRow("4r3/5P2/8/8/2K5/8/2kR4/8 w - - 0 1", "F7", "F8", "R", "4rR2/8/8/8/2K5/8/2kR4/8 b - - 0 1")]
        [TestMethod]
        public void Play_ShouldHandlePawnPromotion(string fen, string origin, string destination, string promotingPiece, string expectedFen)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination, string.IsNullOrEmpty(promotingPiece) ? null : promotingPiece);
            Assert.AreEqual(expectedFen, match.FenString);
        }
    }
}