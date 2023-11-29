using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CheckTests
    {
        [DataRow('b', "r7/3R2k1/4P3/4K3/8/8/8/8 w - - 0 1")]
        [DataRow('b', "rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1")]
        [DataRow('b', "rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1")]
        [DataRow('b', "3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('w', "3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('w', "r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1")]
        [DataRow('w', "rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1")]
        [DataRow('w', "rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1")]
        [TestMethod]
        public void IsInCheck_PlayerInCheck_ShouldReturnTrue(char color, string fen)
        {
            Chessboard chessboard = new(fen);
            Color player = color == 'w' ? Color.White : Color.Black;

            Assert.IsTrue(Check.IsInCheck(player, chessboard));
        }

        [DataRow('w', "r7/3R2k1/4P3/4K3/8/8/8/8 w - - 0 1")]
        [DataRow('w', "rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1")]
        [DataRow('w', "rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1")]
        [DataRow('w', "3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('w', "rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow('w', "8/8/8/5PK1/8/k7/8/3r4 w - - 0 1")]
        [DataRow('b', "3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('b', "r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1")]
        [DataRow('b', "rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1")]
        [DataRow('b', "rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1")]
        [DataRow('b', "rnbqkbnr/pp2pppp/2p5/3P4/3P4/8/PPP2PPP/RNBQKBNR b KQkq - 0 1")]
        [DataRow('b', "rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow('b', "8/8/8/5PK1/8/k7/8/3r4 w - - 0 1")]
        [TestMethod]
        public void IsInCheck_PlayerNotInCheck_ShouldReturnFalse(char color, string fen)
        {
            Chessboard chessboard = new(fen);
            Color player = color == 'w' ? Color.White : Color.Black;

            Assert.IsFalse(Check.IsInCheck(player, chessboard));
        }
    }
}