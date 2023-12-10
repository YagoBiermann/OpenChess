using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CastlingTests
    {
        [TestMethod]
        public void NewInstanceWithEmptyConstructor_ShouldBeTrueForAll()
        {
            Castling castling = new();

            Assert.IsTrue(castling.HasWhiteKingSide);
            Assert.IsTrue(castling.HasWhiteQueenSide);
            Assert.IsTrue(castling.HasBlackKingSide);
            Assert.IsTrue(castling.HasBlackQueenSide);
        }

        [TestMethod]
        public void NewInstanceWithOneParameter_ShouldApplyBoolForAll()
        {
            Castling castling = new(false);
            Castling castling2 = new(true);

            Assert.IsFalse(castling.HasWhiteKingSide);
            Assert.IsFalse(castling.HasWhiteQueenSide);
            Assert.IsFalse(castling.HasBlackKingSide);
            Assert.IsFalse(castling.HasBlackQueenSide);
            Assert.IsTrue(castling2.HasWhiteKingSide);
            Assert.IsTrue(castling2.HasWhiteQueenSide);
            Assert.IsTrue(castling2.HasBlackKingSide);
            Assert.IsTrue(castling2.HasBlackQueenSide);
        }

        [TestMethod]
        public void ToString_AllPropertiesFalse_ShouldConvertToHyphen()
        {
            Castling castling = new(false);

            Assert.AreEqual("-", castling.ToString());
        }

        [TestMethod]
        public void ToString_AllPropertiesTrue_ShouldConvertToDefaultCastling()
        {
            Castling castling = new(true);

            Assert.AreEqual("KQkq", castling.ToString());
        }

        [TestMethod]
        public void ToString_ShouldConvertCorrectly()
        {
            Castling castling = new(false, true, true, false);
            Castling castling2 = new(false, true, false, false);
            Castling castling3 = new(false, true, false, true);
            Castling castling4 = new(true, false, true, false);

            Assert.AreEqual("Qk", castling.ToString());
            Assert.AreEqual("Q", castling2.ToString());
            Assert.AreEqual("Qq", castling3.ToString());
            Assert.AreEqual("Kk", castling4.ToString());
        }
    }
}