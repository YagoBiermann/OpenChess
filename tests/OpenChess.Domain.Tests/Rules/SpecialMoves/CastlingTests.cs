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

        [DataRow("E1", "G1")]
        [DataRow("E1", "C1")]
        [DataRow("E8", "G8")]
        [DataRow("E8", "C8")]
        [TestMethod]
        public void IsCastling_ShouldReturnTrue(string position1, string position2)
        {
            Chessboard chessboard = new("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1");
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.IsTrue(Castling.IsCastling(origin, destination, chessboard));
        }

        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "E1", "D1")]
        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "E1", "F1")]
        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "E1", "C8")]
        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "F1", "C1")]
        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "E8", "F8")]
        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "E8", "D8")]
        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "E8", "C1")]
        [DataRow("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1", "F8", "E8")]
        [DataRow("r3Q2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3Q2R w - - 0 1", "E1", "G1")]
        [DataRow("r3Q2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3Q2R w - - 0 1", "E1", "C1")]
        [DataRow("r3Q2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3Q2R w - - 0 1", "E8", "C8")]
        [DataRow("r3Q2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3Q2R w - - 0 1", "E8", "G8")]
        [TestMethod]
        public void IsCastling_ShouldReturnFalse(string fen, string position1, string position2)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.IsFalse(Castling.IsCastling(origin, destination, chessboard));
        }
    }
}