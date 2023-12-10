using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CastlingTests
    {
        [TestMethod]
        public void NewInstanceWithEmptyConstructor_ShouldBeTrueForAll()
        {
            Castling castlingAvailability = new();

            Assert.IsTrue(castlingAvailability.HasWhiteKingSide);
            Assert.IsTrue(castlingAvailability.HasWhiteQueenSide);
            Assert.IsTrue(castlingAvailability.HasBlackKingSide);
            Assert.IsTrue(castlingAvailability.HasBlackQueenSide);
        }

        [TestMethod]
        public void NewInstanceWithOneParameter_ShouldApplyBoolForAll()
        {
            Castling castlingAvailability = new(false);
            Castling castlingAvailability2 = new(true);

            Assert.IsFalse(castlingAvailability.HasWhiteKingSide);
            Assert.IsFalse(castlingAvailability.HasWhiteQueenSide);
            Assert.IsFalse(castlingAvailability.HasBlackKingSide);
            Assert.IsFalse(castlingAvailability.HasBlackQueenSide);
            Assert.IsTrue(castlingAvailability2.HasWhiteKingSide);
            Assert.IsTrue(castlingAvailability2.HasWhiteQueenSide);
            Assert.IsTrue(castlingAvailability2.HasBlackKingSide);
            Assert.IsTrue(castlingAvailability2.HasBlackQueenSide);
        }

        [TestMethod]
        public void ToString_AllPropertiesFalse_ShouldConvertToHyphen()
        {
            Castling castlingAvailability = new(false);

            Assert.AreEqual("-", castlingAvailability.ToString());
        }

        [TestMethod]
        public void ToString_AllPropertiesTrue_ShouldConvertToDefaultCastling()
        {
            Castling castlingAvailability = new(true);

            Assert.AreEqual("KQkq", castlingAvailability.ToString());
        }

        [TestMethod]
        public void ToString_ShouldConvertCorrectly()
        {
            Castling castlingAvailability = new(false, true, true, false);
            Castling castlingAvailability2 = new(false, true, false, false);
            Castling castlingAvailability3 = new(false, true, false, true);
            Castling castlingAvailability4 = new(true, false, true, false);

            Assert.AreEqual("Qk", castlingAvailability.ToString());
            Assert.AreEqual("Q", castlingAvailability2.ToString());
            Assert.AreEqual("Qq", castlingAvailability3.ToString());
            Assert.AreEqual("Kk", castlingAvailability4.ToString());
        }
    }
}