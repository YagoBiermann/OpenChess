using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class FENTests
    {
        [TestMethod]
        public void IsValid_GivenValidFEN_ShouldReturnTrue()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            string position2 = "4r1k1/3b1ppp/rq1p1b2/1p2p3/1P2P3/4QN1P/5PP1/RN2K2R w KQ - 0 19";
            string position3 = "8/6b1/4k1P1/1q6/q7/K7/8/8 w - - 11 63";
            string position4 = "8/6b1/4k1P1/1q6/q7/K7/8/8 w - F3 11 63";
            string position5 = "8/6b1/4k1P1/1q6/q7/K7/8/8 w - F6 11 63";

            Assert.IsTrue(FEN.IsValid(position));
            Assert.IsTrue(FEN.IsValid(position2));
            Assert.IsTrue(FEN.IsValid(position3));
            Assert.IsTrue(FEN.IsValid(position4));
            Assert.IsTrue(FEN.IsValid(position5));
        }

        [TestMethod]
        public void IsValid_MoreThanSixFields_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 InvalidField";
            string position2 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1 1";
            string position3 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR Test w KQkq - 0 1";

            Assert.IsFalse(FEN.IsValid(position));
            Assert.IsFalse(FEN.IsValid(position2));
            Assert.IsFalse(FEN.IsValid(position3));
        }

        [TestMethod]
        public void IsValid_LessThanSixFields_ShouldReturnFalse()
        {
            string position = "w KQkq - 0 1";
            string position2 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w - 0 1";
            string position3 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR KQkq - 0 1";

            Assert.IsFalse(FEN.IsValid(position));
            Assert.IsFalse(FEN.IsValid(position2));
            Assert.IsFalse(FEN.IsValid(position3));
        }

        [TestMethod]
        public void IsValid_LessThanEightColumns_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP w KQkq - 0 1";
            string position2 = "rnbqkbnr/pppppppp/8/8/8/PPPPPPPP w KQkq - 0 1";
            string position3 = "rnbqkbnr";
            string position4 = "";

            Assert.IsFalse(FEN.IsValid(position));
            Assert.IsFalse(FEN.IsValid(position2));
            Assert.IsFalse(FEN.IsValid(position3));
            Assert.IsFalse(FEN.IsValid(position4));
        }

        [TestMethod]
        public void IsValid_MoreThanEightColumns_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR/123456 w KQkq - 0 1";

            Assert.IsFalse(FEN.IsValid(position));
        }

        [TestMethod]
        public void IsValid_DuplicatedSlashes_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR// w KQkq - 0 1";
            string position2 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP//RNBQKBNR w KQkq - 0 1";
            string position3 = "rnbqkbnr/pppppppp/8/8/8//8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            string position4 = "//rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

            Assert.IsFalse(FEN.IsValid(position));
            Assert.IsFalse(FEN.IsValid(position2));
            Assert.IsFalse(FEN.IsValid(position3));
            Assert.IsFalse(FEN.IsValid(position4));
        }

        [TestMethod]
        public void IsValid_InvalidRow_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/ppppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            string position2 = "rnbqkbnr/pppppppp/9/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            string position3 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQ2BNR w KQkq - 0 1";
            string position4 = "rnbqkbnr/pppppppp/8/8/8/8/P/RNBQKBNR w KQkq - 0 1";
            string position5 = "rnbqkbnr/pppppppp/8/8/8/8/P8/RNBQKBNR w KQkq - 0 1";

            Assert.IsFalse(FEN.IsValid(position));
            Assert.IsFalse(FEN.IsValid(position2));
            Assert.IsFalse(FEN.IsValid(position3));
            Assert.IsFalse(FEN.IsValid(position4));
            Assert.IsFalse(FEN.IsValid(position5));
        }

        [TestMethod]
        public void IsValid_InvalidActiveColor_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR x KQkq - 0 1";
            string position2 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR 0 KQkq - 0 1";
            string position3 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR B KQkq - 0 1";
            string position4 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR W KQkq - 0 1";

            Assert.IsFalse(FEN.IsValid(position));
            Assert.IsFalse(FEN.IsValid(position2));
            Assert.IsFalse(FEN.IsValid(position3));
            Assert.IsFalse(FEN.IsValid(position4));
        }

        [TestMethod]
        public void IsValid_InvalidEnPassant_ShouldReturnFalse()
        {
            string position = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 22 0 1";
            string position2 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq 1 0 1";
            string position3 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq FW 0 1";
            string position4 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq E4 0 1";
            string position5 = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq E5 0 1";

            Assert.IsFalse(FEN.IsValid(position));
            Assert.IsFalse(FEN.IsValid(position2));
            Assert.IsFalse(FEN.IsValid(position3));
            Assert.IsFalse(FEN.IsValid(position4));
            Assert.IsFalse(FEN.IsValid(position5));
        }
    }
}