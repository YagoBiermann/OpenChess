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
    }
}