using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PGNBuilderTests
    {
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "E2", "E4", "1. e4")]
        [DataRow("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1", "E7", "E5", "1. e5")]
        [DataRow("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D2", "D4", "1. d4")]
        [TestMethod]
        public void PawnMove_Default_ShouldAddPGNInCorrectFormat(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("rnbqkbnr/pppp1ppp/8/4p3/3PP3/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "D4", "E5", "1. dxe5")]
        [DataRow("rnbqkbnr/pppp2pp/5p2/4P3/4P3/8/PPP2PPP/RNBQKBNR b KQkq - 0 1", "F6", "E5", "1. fxe5")]
        [DataRow("rnb1kbnr/ppp3pp/8/3qp3/4P3/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "E4", "D5", "1. exd5")]
        [DataRow("rnb1kbnr/ppp3pp/B7/3Pp3/8/8/PPP2PPP/RNBQK1NR b KQkq - 0 1", "B7", "A6", "1. bxa6")]
        [DataRow("rnb1kbnr/p1p3pp/8/8/1pP5/8/PP3PPP/RNBQK1NR b KQkq C3 0 1", "B4", "C3", "1. bxc3")]
        [TestMethod]
        public void PawnMove_WithCapture_ShouldAddPGNInCorrectFormat(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("r1bqkb1r/ppp2ppp/2nP1n2/4p3/8/2N2N2/PPPP1PPP/R1BQKB1R w KQkq - 0 1", "D6", "D7", "1. d7+")]
        [DataRow("r1bqkb1r/ppp2ppp/2n2n2/8/8/2Np4/PPP3PP/R1BQKB1R b KQkq - 0 1", "D3", "D2", "1. d2+")]
        [TestMethod]
        public void PawnMove_WithCheck_ShouldAddPGNInCorrectFormat(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("rnbk1bnr/pp1Ppppp/1qp5/8/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "R", "1. dxc8=R+")]
        [DataRow("rnbk1bnr/pp1Ppppp/1qp5/8/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "Q", "1. dxc8=Q+")]
        [DataRow("rnbk1bnr/pp1Ppppp/8/8/8/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "B", "1. dxc8=B+")]
        [DataRow("rnbk1bnr/pp1Ppppp/8/8/8/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "N", "1. dxc8=N+")]
        [TestMethod]
        public void PawnMove_PromotingPawnWithCaptureAndCheck_ShouldAddPGNInCorrectFormat(string fen, string origin, string destination, string promoting, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination, promoting);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }
    }
}