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

        [DataRow("r3k2r/pppppppp/2N3N1/8/8/2n3n1/PPPPPPPP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp1p1pp/3pBp2/8/8/3PbP2/PPP1P1PP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp1p1pp/3pNp2/8/8/3PnP2/PPP1P1PP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppP1p1Pp/3p1p2/8/8/3P1P2/PPp1P1pP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp1p1pp/3pBp2/8/8/3PbP2/PPP1P1PP/R3K2R b KQkq - 0 1", 'b')]
        [DataRow("r3k2r/pppppppp/2N3N1/8/8/2n3n1/PPPPPPPP/R3K2R b KQkq - 0 1", 'b')]
        [DataRow("r3k2r/ppp1p1pp/3pNp2/8/8/3PnP2/PPP1P1PP/R3K2R b KQkq - 0 1", 'b')]
        [DataRow("r3k2r/ppP1p1Pp/3p1p2/8/8/3P1P2/PPp1P1pP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_AnyEnemyPieceHittingSquaresWhereKingPassThroughDuringTheCastling_ShouldNotBeAbleToCastle(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate queenSideDestination = Coordinate.GetInstance($"C{row}");
            Coordinate kingSideDestination = Coordinate.GetInstance($"C{row}");
            Chessboard chessboard = new(position);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, queenSideDestination));
            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, kingSideDestination));
        }

        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R b KQkq - 0 1", 'b', true)]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R w KQkq - 0 1", 'w', true)]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R b KQkq - 0 1", 'b', false)]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R w KQkq - 0 1", 'w', false)]
        [TestMethod]
        public void MovePiece_KingMoved_ShouldNotBeAbleToCastle(string position, char color, bool isCastlingKingSide)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            string column = isCastlingKingSide ? "G" : "C";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate destination = Coordinate.GetInstance($"{column}{row}");
            Chessboard chessboard = new(position);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
        }

        [DataRow("1r2k2r/pp5p/6p1/8/8/1P4P1/P6P/1R2K2R w KQkq - 0 1", 'w')]
        [DataRow("1r2k2r/pp5p/6p1/8/8/1P4P1/P6P/1R2K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_QueenSideRookMoved_ShouldNotBeAbleToCastleOnlyInQueenSide(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate queenSideDestination = Coordinate.GetInstance($"C{row}");
            Coordinate kingSideDestination = Coordinate.GetInstance($"G{row}");
            Chessboard chessboard = new(position);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, queenSideDestination));
            chessboard.MovePiece(origin, kingSideDestination);
            Assert.IsTrue(chessboard.GetReadOnlySquare(kingSideDestination).HasPiece);
        }

        [DataRow("r3k1r1/pp5p/6p1/8/8/1P4P1/P6P/R3K1R1 w KQkq - 0 1", 'w')]
        [DataRow("r3k1r1/pp5p/6p1/8/8/1P4P1/P6P/R3K1R1 b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_KingSideRookMoved_ShouldNotBeAbleToCastleOnlyInKingSide(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate queenSideDestination = Coordinate.GetInstance($"C{row}");
            Coordinate kingSideDestination = Coordinate.GetInstance($"G{row}");
            Chessboard chessboard = new(position);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, kingSideDestination));
            chessboard.MovePiece(origin, queenSideDestination);
            Assert.IsTrue(chessboard.GetReadOnlySquare(queenSideDestination).HasPiece);
        }

        [DataRow("r3k2r/pp5p/6p1/8/8/1Pb3P1/P6P/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/pp5p/2B3p1/8/8/1P4P1/P6P/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_KingInCheck_ShouldNotBeAbleToCastle(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate queenSideDestination = Coordinate.GetInstance($"C{row}");
            Coordinate kingSideDestination = Coordinate.GetInstance($"G{row}");
            Chessboard chessboard = new(position);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, kingSideDestination));
            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, queenSideDestination));
        }

        [DataRow("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w - - 0 1", 'w')]
        [DataRow("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R b - - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_CastlingNotAvailable_ShouldNotBeAbleToCastle(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate queenSideDestination = Coordinate.GetInstance($"C{row}");
            Coordinate kingSideDestination = Coordinate.GetInstance($"G{row}");
            Chessboard chessboard = new(position);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, kingSideDestination));
            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, queenSideDestination));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 'w')]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 'b')]
        [DataRow("r1b1kb1r/ppp1pppp/2nq1n2/3p4/3P4/2NQ1N2/PPP1PPPP/R1B1KB1R w KQkq - 0 1", 'w')]
        [DataRow("r1b1kb1r/ppp1pppp/2nq1n2/3p4/3P4/2NQ1N2/PPP1PPPP/R1B1KB1R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_AnyPiecesInBetweenKingAndRook_ShouldNotBeAbleToCastle(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate queenSideDestination = Coordinate.GetInstance($"C{row}");
            Coordinate kingSideDestination = Coordinate.GetInstance($"G{row}");
            Chessboard chessboard = new(position);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, kingSideDestination));
            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, queenSideDestination));
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_MovingQueenSideRook_ShouldLoseQueenSideCastling(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"A{row}");
            Coordinate destination = Coordinate.GetInstance($"B{row}");
            Chessboard chessboard = new(position);

            bool isAvailableBefore = color == 'w' ? chessboard.CastlingAvailability.IsWhiteQueenSideAvailable : chessboard.CastlingAvailability.IsBlackQueenSideAvailable;
            Assert.IsTrue(isAvailableBefore);
            chessboard.MovePiece(origin, destination);
            bool isAvailableAfter = color == 'w' ? chessboard.CastlingAvailability.IsWhiteQueenSideAvailable : chessboard.CastlingAvailability.IsBlackQueenSideAvailable;
            Assert.IsFalse(isAvailableAfter);
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_MovingKingSideRook_ShouldLoseKingSideCastling(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"H{row}");
            Coordinate destination = Coordinate.GetInstance($"G{row}");
            Chessboard chessboard = new(position);

            bool isAvailableBefore = color == 'w' ? chessboard.CastlingAvailability.IsWhiteKingSideAvailable : chessboard.CastlingAvailability.IsBlackKingSideAvailable;
            Assert.IsTrue(isAvailableBefore);
            chessboard.MovePiece(origin, destination);
            bool isAvailableAfter = color == 'w' ? chessboard.CastlingAvailability.IsWhiteKingSideAvailable : chessboard.CastlingAvailability.IsBlackKingSideAvailable;
            Assert.IsFalse(isAvailableAfter);
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_MovingKing_ShouldLoseCastling(string position, char color)
        {
            Color player = Utils.ColorFromChar(color);
            string row = player == Color.White ? "1" : "8";
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate destination = Coordinate.GetInstance($"D{row}");
            Chessboard chessboard = new(position);

            chessboard.MovePiece(origin, destination);
            bool isKingSideAvailable = color == 'w' ? chessboard.CastlingAvailability.IsWhiteKingSideAvailable : chessboard.CastlingAvailability.IsBlackKingSideAvailable;
            bool isQueenSideAvailable = color == 'w' ? chessboard.CastlingAvailability.IsWhiteQueenSideAvailable : chessboard.CastlingAvailability.IsBlackQueenSideAvailable;

            Assert.IsFalse(isKingSideAvailable);
            Assert.IsFalse(isQueenSideAvailable);
        }
    }
}