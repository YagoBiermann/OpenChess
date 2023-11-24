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
        }
    }
}