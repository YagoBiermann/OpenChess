using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CastlingAvailabilityTests
    {
        [TestMethod]
        public void NewInstanceWithEmptyConstructor_ShouldBeTrueForAll()
        {
            CastlingAvailability castlingAvailability = new();

            Assert.IsTrue(castlingAvailability.WhiteKingSide);
            Assert.IsTrue(castlingAvailability.WhiteQueenSide);
            Assert.IsTrue(castlingAvailability.BlackKingSide);
            Assert.IsTrue(castlingAvailability.BlackQueenSide);
        }

        [TestMethod]
        public void NewInstanceWithOneParameter_ShouldApplyBoolForAll()
        {
            CastlingAvailability castlingAvailability = new(false);
            CastlingAvailability castlingAvailability2 = new(true);

            Assert.IsFalse(castlingAvailability.WhiteKingSide);
            Assert.IsFalse(castlingAvailability.WhiteQueenSide);
            Assert.IsFalse(castlingAvailability.BlackKingSide);
            Assert.IsFalse(castlingAvailability.BlackQueenSide);
            Assert.IsTrue(castlingAvailability2.WhiteKingSide);
            Assert.IsTrue(castlingAvailability2.WhiteQueenSide);
            Assert.IsTrue(castlingAvailability2.BlackKingSide);
            Assert.IsTrue(castlingAvailability2.BlackQueenSide);
        }

        [TestMethod]
        public void ToString_AllPropertiesFalse_ShouldConvertToHyphen()
        {
            CastlingAvailability castlingAvailability = new(false);

            Assert.AreEqual("-", castlingAvailability.ToString());
        }

        [TestMethod]
        public void ToString_AllPropertiesTrue_ShouldConvertToDefaultCastling()
        {
            CastlingAvailability castlingAvailability = new(true);

            Assert.AreEqual("KQkq", castlingAvailability.ToString());
        }

        [TestMethod]
        public void ToString_ShouldConvertCorrectly()
        {
            CastlingAvailability castlingAvailability = new(false, true, true, false);
            CastlingAvailability castlingAvailability2 = new(false, true, false, false);
            CastlingAvailability castlingAvailability3 = new(false, true, false, true);
            CastlingAvailability castlingAvailability4 = new(true, false, true, false);

            Assert.AreEqual("Qk", castlingAvailability.ToString());
            Assert.AreEqual("Q", castlingAvailability2.ToString());
            Assert.AreEqual("Qq", castlingAvailability3.ToString());
            Assert.AreEqual("Kk", castlingAvailability4.ToString());
        }
    }
}