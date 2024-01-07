using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class EnPassantTests
    {
        [DataRow("rnbqkbnr/pppp1ppp/8/8/3pP3/8/PPPP1PPP/RNBQKBNR b KQkq E3 0 1", "D4", "E3", "rnbqkbnr/pppp1ppp/8/8/8/4p3/PPPP1PPP/RNBQKBNR w KQkq - 0 2")]
        [DataRow("rnbqkbnr/pppp1ppp/8/3Pp3/8/8/PPP1PPPP/RNBQKBNR w KQkq E6 0 1", "D5", "E6", "rnbqkbnr/pppp1ppp/4P3/8/8/8/PPP1PPPP/RNBQKBNR b KQkq - 0 1")]
        [TestMethod]
        public void Play_PawnVulnerable_ShouldBeCapturedByEnPassant(string fen, string position1, string position2, string expectedFen)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, position1, position2);
            Assert.AreEqual(expectedFen, match.FenString);
        }

        [DataRow("rnbqkbnr/pppp1ppp/8/8/3pP3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D4", "E3")]
        [DataRow("rnbqkbnr/pppp1ppp/8/3Pp3/8/8/PPP1PPPP/RNBQKBNR w KQkq - 0 1", "D5", "E6")]
        [TestMethod]
        public void Play_PawnNotVulnerable_ShouldNotBeCaptured(string fen, string position1, string position2)
        {
            Assert.ThrowsException<ChessboardException>(() => FakeMatch.RestoreAndPlay(fen, position1, position2));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq E3 0 1", "D7", "E4")]
        [DataRow("rnbqkbnr/pppp1ppp/8/4p3/8/8/PPPPPPPP/RNBQKBNR w KQkq E6 0 1", "D2", "E5")]
        [TestMethod]
        public void Play_PawnOutOfRange_ShouldNotBeAbleToCapture(string fen, string position1, string position2)
        {
            Assert.ThrowsException<ChessboardException>(() => FakeMatch.RestoreAndPlay(fen, position1, position2));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "E2", "E4", "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq E3 0 1")]
        [DataRow("rnbqkb1r/1p2pppp/p2p1n2/8/3NP3/2N5/PPP2PPP/R1BQKB1R b KQkq - 0 1", "H7", "H5", "rnbqkb1r/1p2ppp1/p2p1n2/7p/3NP3/2N5/PPP2PPP/R1BQKB1R w KQkq H6 0 2")]
        [DataRow("rnbqkb1r/pppp1ppp/5n2/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq - 0 1", "A7", "A5", "rnbqkb1r/1ppp1ppp/5n2/p3p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq A6 0 2")]
        [TestMethod]
        public void Play_ShouldSetPawnAsVulnerableOnMovingTwoSquaresForward(string fen, string origin, string destination, string expectedFen)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedFen, match.FenString);
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "E2", "E3", "rnbqkbnr/pppppppp/8/8/8/4P3/PPPP1PPP/RNBQKBNR b KQkq - 0 1")]
        [DataRow("rnbqkb1r/ppp1pp1p/3p1np1/8/3PP3/2N2N2/PPP2PPP/R1BQKB1R b KQkq - 0 1", "H7", "H6", "rnbqkb1r/ppp1pp2/3p1npp/8/3PP3/2N2N2/PPP2PPP/R1BQKB1R w KQkq - 0 2")]
        [TestMethod]
        public void Play_ShouldNotSetPawnAsVulnerableOnMovingOneSquareForward(string fen, string origin, string destination, string expectedFen)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedFen, match.FenString);
        }
    }
}