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

            Assert.IsTrue(FEN.IsValid(position));
            Assert.IsTrue(FEN.IsValid(position2));
            Assert.IsTrue(FEN.IsValid(position3));
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
    }
}