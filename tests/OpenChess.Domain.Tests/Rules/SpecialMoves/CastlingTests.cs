using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CastlingTests
    {
        [TestMethod]
        public void NewInstanceWithEmptyConstructor_ShouldBeTrueForAll()
        {
            CastlingAvailability castling = new();

            Assert.IsTrue(castling.IsWhiteKingSideAvailable);
            Assert.IsTrue(castling.IsWhiteQueenSideAvailable);
            Assert.IsTrue(castling.IsBlackKingSideAvailable);
            Assert.IsTrue(castling.IsBlackQueenSideAvailable);
        }

        [TestMethod]
        public void ToString_AllPropertiesFalse_ShouldConvertToHyphen()
        {
            CastlingAvailability castling = new(false, false, false, false);

            Assert.AreEqual("-", castling.ToString());
        }

        [TestMethod]
        public void ToString_AllPropertiesTrue_ShouldConvertToDefaultCastling()
        {
            CastlingAvailability castling = new();

            Assert.AreEqual("KQkq", castling.ToString());
        }

        [TestMethod]
        public void ToString_ShouldConvertCorrectly()
        {
            CastlingAvailability castling = new(false, true, true, false);
            CastlingAvailability castling2 = new(false, true, false, false);
            CastlingAvailability castling3 = new(false, true, false, true);
            CastlingAvailability castling4 = new(true, false, true, false);

            Assert.AreEqual("Qk", castling.ToString());
            Assert.AreEqual("Q", castling2.ToString());
            Assert.AreEqual("Qq", castling3.ToString());
            Assert.AreEqual("Kk", castling4.ToString());
        }

        [DataRow("rnbqk2r/pppp1ppp/5n2/1B2p3/1b2P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 1", 'w')]
        [DataRow("1nb1k1n1/ppp2p1r/8/2bp4/1r1pP3/3NBN2/P3QP2/R3K2R w KQ - 0 1", 'w')]
        [DataRow("r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pp1/2n2n1p/2bppbq1/2BPP3/2N1BN1P/PPP1QPP1/R3K2R b KQkq - 0 1", 'b')]
        [DataRow("r3k2r/p1pp1pp1/5n2/5q2/1R1PP3/2N2N1Q/PPP2P2/R3K2R b KQkq - 0 1", 'b')]
        [DataRow("r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_CastlingToKingSide_ShouldBeHandledCorrectly(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate destination = Coordinate.GetInstance($"G{row}");
            Coordinate rookPosition = Coordinate.GetInstance($"F{row}");
            Chessboard chessboard = new(position);

            chessboard.MovePiece(origin, destination);

            Assert.IsFalse(chessboard.GetReadOnlySquare(Coordinate.GetInstance($"H{row}")).HasPiece);
            Assert.IsFalse(chessboard.GetReadOnlySquare(Coordinate.GetInstance($"E{row}")).HasPiece);
            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasPiece);
            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetReadOnlySquare(destination).ReadOnlyPiece, typeof(King));
            Assert.IsInstanceOfType(chessboard.GetReadOnlySquare(rookPosition).ReadOnlyPiece, typeof(Rook));
        }

        [DataRow("r2qkbnr/pp1nppp1/2p4p/5bB1/3PN2Q/8/PPP2PPP/R3KBNR w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("1nb1k1n1/ppp2p1r/8/2bp4/1r1pP3/3NBN2/P3QP2/R3K2R w KQ - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pp1/2n2n1p/2bppbq1/2BPP3/2N1BN1P/PPP1QPP1/R3K2R b KQkq - 0 1", 'b')]
        [DataRow("r3k2r/p1pp1pp1/5n2/5q2/1R1PP3/2N2N1Q/PPP2P2/R3K2R b KQkq - 0 1", 'b')]
        [DataRow("r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_CastlingToQueenSide_ShouldBeHandledCorrectly(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate destination = Coordinate.GetInstance($"C{row}");
            Coordinate rookPosition = Coordinate.GetInstance($"D{row}");
            Chessboard chessboard = new(position);

            chessboard.MovePiece(origin, destination);

            Assert.IsFalse(chessboard.GetReadOnlySquare(Coordinate.GetInstance($"A{row}")).HasPiece);
            Assert.IsFalse(chessboard.GetReadOnlySquare(Coordinate.GetInstance($"E{row}")).HasPiece);
            Assert.IsTrue(chessboard.GetReadOnlySquare(rookPosition).HasPiece);
            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetReadOnlySquare(destination).ReadOnlyPiece, typeof(King));
            Assert.IsInstanceOfType(chessboard.GetReadOnlySquare(rookPosition).ReadOnlyPiece, typeof(Rook));
        }
    }
}