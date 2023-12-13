using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CastlingTests
    {
        [TestMethod]
        public void NewInstanceWithEmptyConstructor_ShouldBeTrueForAll()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Castling castling = new(chessboard);

            Assert.IsTrue(castling.HasWhiteKingSide);
            Assert.IsTrue(castling.HasWhiteQueenSide);
            Assert.IsTrue(castling.HasBlackKingSide);
            Assert.IsTrue(castling.HasBlackQueenSide);
        }

        [TestMethod]
        public void ToString_AllPropertiesFalse_ShouldConvertToHyphen()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Castling castling = new(false, false, false, false, chessboard);

            Assert.AreEqual("-", castling.ToString());
        }

        [TestMethod]
        public void ToString_AllPropertiesTrue_ShouldConvertToDefaultCastling()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Castling castling = new(chessboard);

            Assert.AreEqual("KQkq", castling.ToString());
        }

        [TestMethod]
        public void ToString_ShouldConvertCorrectly()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Castling castling = new(false, true, true, false, chessboard);
            Castling castling2 = new(false, true, false, false, chessboard);
            Castling castling3 = new(false, true, false, true, chessboard);
            Castling castling4 = new(true, false, true, false, chessboard);

            Assert.AreEqual("Qk", castling.ToString());
            Assert.AreEqual("Q", castling2.ToString());
            Assert.AreEqual("Qq", castling3.ToString());
            Assert.AreEqual("Kk", castling4.ToString());
        }
    }
}