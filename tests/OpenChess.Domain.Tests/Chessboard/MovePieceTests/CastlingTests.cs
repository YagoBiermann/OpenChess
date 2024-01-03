using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CastlingTests
    {
        private static Chessboard MovePiece(string position, char color, bool castlingKingSide)
        {
            string row = GetRow(color);
            string column = GetColumn(castlingKingSide);
            Coordinate origin = Coordinate.GetInstance($"E{row}");
            Coordinate destination = Coordinate.GetInstance($"{column}{row}");
            Chessboard chessboard = new(position);

            chessboard.MovePiece(origin, destination);
            return chessboard;
        }

        private static Chessboard MovePiece(string position, string origin, string destination)
        {
            Chessboard chessboard = new(position);
            chessboard.MovePiece(Coordinate.GetInstance($"{origin}"), Coordinate.GetInstance($"{destination}"));
            return chessboard;
        }

        private static bool GetQueenCastling(char color, Chessboard chessboard)
        {
            return color == 'w' ? chessboard.CastlingAvailability.IsAvailableAt['Q'] : chessboard.CastlingAvailability.IsAvailableAt['q'];
        }

        private static bool GetKingCastling(char color, Chessboard chessboard)
        {
            return color == 'w' ? chessboard.CastlingAvailability.IsAvailableAt['K'] : chessboard.CastlingAvailability.IsAvailableAt['k'];
        }

        private static string GetColumn(bool castlingKingSide)
        {
            return castlingKingSide ? "G" : "C";
        }

        private static string GetRow(char color)
        {
            return color == 'w' ? "1" : "8";
        }

        [TestMethod]
        public void NewInstanceWithEmptyConstructor_ShouldBeTrueForAll()
        {
            CastlingAvailability castling = new();

            Assert.IsTrue(castling.IsAvailableAt['K']);
            Assert.IsTrue(castling.IsAvailableAt['Q']);
            Assert.IsTrue(castling.IsAvailableAt['k']);
            Assert.IsTrue(castling.IsAvailableAt['q']);
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
            Chessboard chessboard = MovePiece(position, color, true);
            string row = GetRow(color);
            string column = GetColumn(true);
            Coordinate destination = Coordinate.GetInstance($"{column}{row}");
            Coordinate rookPosition = Coordinate.GetInstance($"F{row}");

            Assert.IsFalse(chessboard.GetSquare(Coordinate.GetInstance($"H{row}")).HasPiece);
            Assert.IsFalse(chessboard.GetSquare(Coordinate.GetInstance($"E{row}")).HasPiece);
            Assert.IsTrue(chessboard.GetSquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetPiece(destination), typeof(King));
            Assert.IsInstanceOfType(chessboard.GetPiece(rookPosition), typeof(Rook));
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
            string row = GetRow(color);
            string column = GetColumn(false);
            Coordinate destination = Coordinate.GetInstance($"{column}{row}");
            Coordinate rookPosition = Coordinate.GetInstance($"D{row}");

            Chessboard chessboard = MovePiece(position, color, false);

            Assert.IsFalse(chessboard.GetSquare(Coordinate.GetInstance($"A{row}")).HasPiece);
            Assert.IsFalse(chessboard.GetSquare(Coordinate.GetInstance($"E{row}")).HasPiece);
            Assert.IsTrue(chessboard.GetSquare(rookPosition).HasPiece);
            Assert.IsTrue(chessboard.GetSquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetPiece(destination), typeof(King));
            Assert.IsInstanceOfType(chessboard.GetPiece(rookPosition), typeof(Rook));
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
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, false));
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, true));
        }

        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R b KQkq - 0 1", 'b', true)]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R w KQkq - 0 1", 'w', true)]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R b KQkq - 0 1", 'b', false)]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R w KQkq - 0 1", 'w', false)]
        [TestMethod]
        public void MovePiece_KingMoved_ShouldNotBeAbleToCastle(string position, char color, bool isCastlingKingSide)
        {
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, isCastlingKingSide));
        }

        [DataRow("1r2k2r/pp5p/6p1/8/8/1P4P1/P6P/1R2K2R w KQkq - 0 1", 'w')]
        [DataRow("1r2k2r/pp5p/6p1/8/8/1P4P1/P6P/1R2K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_QueenSideRookMoved_ShouldBeAbleToCastleOnlyInQueenSide(string position, char color)
        {
            Coordinate kingSideDestination = Coordinate.GetInstance($"G{GetRow(color)}");

            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, false));
            Chessboard chessboard = MovePiece(position, color, true);
            Assert.IsTrue(chessboard.GetSquare(kingSideDestination).HasPiece);
        }

        [DataRow("r3k1r1/pp5p/6p1/8/8/1P4P1/P6P/R3K1R1 w KQkq - 0 1", 'w')]
        [DataRow("r3k1r1/pp5p/6p1/8/8/1P4P1/P6P/R3K1R1 b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_KingSideRookMoved_ShouldBeAbleToCastleOnlyInKingSide(string position, char color)
        {
            Coordinate queenSideDestination = Coordinate.GetInstance($"C{GetRow(color)}");

            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, true));
            Chessboard chessboard = MovePiece(position, color, false);
            Assert.IsTrue(chessboard.GetSquare(queenSideDestination).HasPiece);
        }

        [DataRow("r3k2r/pp5p/6p1/8/8/1Pb3P1/P6P/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/pp5p/2B3p1/8/8/1P4P1/P6P/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_KingInCheck_ShouldNotBeAbleToCastle(string position, char color)
        {
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, true));
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, false));
        }

        [DataRow("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w - - 0 1", 'w')]
        [DataRow("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R b - - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_CastlingNotAvailable_ShouldNotBeAbleToCastle(string position, char color)
        {
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, true));
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, false));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 'w')]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", 'b')]
        [DataRow("r1b1kb1r/ppp1pppp/2nq1n2/3p4/3P4/2NQ1N2/PPP1PPPP/R1B1KB1R w KQkq - 0 1", 'w')]
        [DataRow("r1b1kb1r/ppp1pppp/2nq1n2/3p4/3P4/2NQ1N2/PPP1PPPP/R1B1KB1R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_AnyPiecesInBetweenKingAndRook_ShouldNotBeAbleToCastle(string position, char color)
        {
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, true));
            Assert.ThrowsException<ChessboardException>(() => MovePiece(position, color, false));
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_MovingQueenSideRook_ShouldLoseQueenSideCastling(string position, char color)
        {
            Chessboard chessboard = MovePiece(position, $"E{GetRow(color)}", $"C{GetRow(color)}");
            bool isAvailable = GetQueenCastling(color, chessboard);
            Assert.IsFalse(isAvailable);
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_MovingKingSideRook_ShouldLoseKingSideCastling(string position, char color)
        {
            Chessboard chessboard = MovePiece(position, $"E{GetRow(color)}", $"G{GetRow(color)}");
            bool isAvailable = GetKingCastling(color, chessboard);
            Assert.IsFalse(isAvailable);
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", 'w')]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void MovePiece_MovingKing_ShouldLoseCastling(string position, char color)
        {
            Chessboard chessboard = MovePiece(position, $"E{GetRow(color)}", $"F{GetRow(color)}");
            bool isKingSideAvailable = GetKingCastling(color, chessboard);
            bool isQueenSideAvailable = GetQueenCastling(color, chessboard);

            Assert.IsFalse(isKingSideAvailable);
            Assert.IsFalse(isQueenSideAvailable);
        }
    }
}