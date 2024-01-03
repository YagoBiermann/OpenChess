using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class FENTests
    {
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("4r1k1/3b1ppp/rq1p1b2/1p2p3/1P2P3/4QN1P/5PP1/RN2K2R w KQ - 0 19")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 w - - 11 63")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 w - F3 11 63")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 w - F6 11 63")]
        [TestMethod]
        public void IsValid_GivenValidFEN_ShouldReturnTrue(string position)
        {
            Assert.IsTrue(FenInfo.IsValid(position));
        }

        [TestMethod]
        public void InitialPosition_ShouldBeCorrect()
        {
            Assert.AreEqual("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", FenInfo.InitialPosition);
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 InvalidField")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR Test w KQkq - 0 1")]
        [TestMethod]
        public void IsValid_MoreThanSixFields_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR KQkq - 0 1")]
        [TestMethod]
        public void IsValid_LessThanSixFields_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/PPPPPPPP w KQkq - 0 1")]
        [DataRow("rnbqkbnr")]
        [DataRow("")]
        [TestMethod]
        public void IsValid_LessThanEightColumns_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [TestMethod]
        public void IsValid_MoreThanEightColumns_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/123456 w KQkq - 0 1";

            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR// w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP//RNBQKBNR w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8//8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("//rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [TestMethod]
        public void IsValid_DuplicatedSlashes_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/ppppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/9/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQ2BNR w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/P/RNBQKBNR w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/P8/RNBQKBNR w KQkq - 0 1")]
        [TestMethod]
        public void IsValid_InvalidRow_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR x KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR 0 KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR B KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR W KQkq - 0 1")]
        [TestMethod]
        public void IsValid_InvalidActiveColor_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 22 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 1 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq FW 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq E4 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq E5 0 1")]
        [TestMethod]
        public void IsValid_InvalidEnPassant_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQ-q - 0 1")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 w PWXQ - 11 63")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 b -- - 11 63")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 b 1234 - 11 63")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w kqKQ - 0 1")]
        [TestMethod]
        public void IsValidString_InvalidCastling_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQ-q - 0 1")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 b - - TEST 63")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 b - - 101 63")]
        [DataRow("8/6b1/4k1P1/1q6/q7/K7/8/8 b - - -1 63")]
        [TestMethod]
        public void IsValidString_InvalidHalfMove_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 -1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1999")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1201")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 00")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 FF")]
        [TestMethod]
        public void IsValidString_InvalidFullMove_ShouldReturnFalse(string position)
        {
            Assert.IsFalse(FenInfo.IsValid(position));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b Qkq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b kq - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b q - 0 1")]
        [DataRow("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq E3 0 1")]
        [DataRow("rnbqk2r/pppp1ppp/5n2/2b1p1B1/4P3/3P1N2/PPP2PPP/RN1QKB1R w KQkq - 0 1")]
        [TestMethod]
        public void BuildFenString_ShouldBeInCorrectFormat(string fen)
        {
            Chessboard chessboard = new(fen);
            Assert.AreEqual(fen, FenInfo.BuildFenString(chessboard));
        }
    }
}