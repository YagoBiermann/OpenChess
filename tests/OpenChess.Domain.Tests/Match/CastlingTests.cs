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

            Assert.IsTrue(castling.IsAvailableAt['K']);
            Assert.IsTrue(castling.IsAvailableAt['Q']);
            Assert.IsTrue(castling.IsAvailableAt['k']);
            Assert.IsTrue(castling.IsAvailableAt['q']);
        }

        [DataRow("rnbqk2r/pppp1ppp/5n2/1B2p3/1b2P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 0 1", "E1", "G1", "rnbqk2r/pppp1ppp/5n2/1B2p3/1b2P3/5N2/PPPP1PPP/RNBQ1RK1 b kq - 1 1")]
        [DataRow("1nb1k1n1/ppp2p1r/8/2bp4/1r1pP3/3NBN2/P3QP2/R3K2R w KQ - 0 1", "E1", "G1", "1nb1k1n1/ppp2p1r/8/2bp4/1r1pP3/3NBN2/P3QP2/R4RK1 b - - 1 1")]
        [DataRow("r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R w KQkq - 0 1", "E1", "G1", "r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R4RK1 b kq - 1 1")]
        [DataRow("r3k2r/ppp2pp1/2n2n1p/2bppbq1/2BPP3/2N1BN1P/PPP1QPP1/R3K2R b KQkq - 0 1", "E8", "G8", "r4rk1/ppp2pp1/2n2n1p/2bppbq1/2BPP3/2N1BN1P/PPP1QPP1/R3K2R w KQ - 1 2")]
        [DataRow("r3k2r/p1pp1pp1/5n2/5q2/1R1PP3/2N2N1Q/PPP2P2/R3K2R b KQkq - 0 1", "E8", "G8", "r4rk1/p1pp1pp1/5n2/5q2/1R1PP3/2N2N1Q/PPP2P2/R3K2R w KQ - 1 2")]
        [DataRow("rnbqk2r/pppp1ppp/5n2/4p3/1b1PP3/2P2N2/PP3PPP/RNBQKB1R b KQkq - 0 1", "E8", "G8", "rnbq1rk1/pppp1ppp/5n2/4p3/1b1PP3/2P2N2/PP3PPP/RNBQKB1R w KQ - 1 2")]
        [TestMethod]
        public void Play_CastlingToKingSide_ShouldBeHandledCorrectly(string fen, string origin, string destination, string expectedFen)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedFen, match.FenString);
        }

        [DataRow("r2qkbnr/pp1nppp1/2p4p/5bB1/3PN2Q/8/PPP2PPP/R3KBNR w KQkq - 0 1", "E1", "C1", "r2qkbnr/pp1nppp1/2p4p/5bB1/3PN2Q/8/PPP2PPP/2KR1BNR b kq - 1 1")]
        [DataRow("r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R w KQkq - 0 1", "E1", "C1", "r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/2KR3R b kq - 1 1")]
        [DataRow("1nb1k1n1/ppp2p1r/8/2bp4/1r1pP3/3NBN2/P3QP2/R3K2R w KQ - 0 1", "E1", "C1", "1nb1k1n1/ppp2p1r/8/2bp4/1r1pP3/3NBN2/P3QP2/2KR3R b - - 1 1")]
        [DataRow("r3k2r/ppp2pp1/2n2n1p/2bppbq1/2BPP3/2N1BN1P/PPP1QPP1/R3K2R b KQkq - 0 1", "E8", "C8", "2kr3r/ppp2pp1/2n2n1p/2bppbq1/2BPP3/2N1BN1P/PPP1QPP1/R3K2R w KQ - 1 2")]
        [DataRow("r3k2r/p1pp1pp1/5n2/5q2/1R1PP3/2N2N1Q/PPP2P2/R3K2R b KQkq - 0 1", "E8", "C8", "2kr3r/p1pp1pp1/5n2/5q2/1R1PP3/2N2N1Q/PPP2P2/R3K2R w KQ - 1 2")]
        [DataRow("r3k2r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R b KQkq - 0 1", "E8", "C8", "2kr3r/pppppppp/4B3/8/8/4b3/PPPPPPPP/R3K2R w KQ - 1 2")]
        [TestMethod]
        public void Play_CastlingToQueenSide_ShouldBeHandledCorrectly(string fen, string origin, string destination, string expectedFen)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedFen, match.FenString);
        }

        [DataRow("r3k2r/pppppppp/2N3N1/8/8/2n3n1/PPPPPPPP/R3K2R w KQkq - 0 1", "E1", "G1", DisplayName = "King passing through check when castling")]
        [DataRow("r3k2r/ppp1p1pp/3pBp2/8/8/3PbP2/PPP1P1PP/R3K2R w KQkq - 0 1", "E1", "C1", DisplayName = "King passing through check when castling")]
        [DataRow("r3k2r/ppp1p1pp/3pNp2/8/8/3PnP2/PPP1P1PP/R3K2R w KQkq - 0 1", "E1", "G1", DisplayName = "King passing through check when castling")]
        [DataRow("r3k2r/ppP1p1Pp/3p1p2/8/8/3P1P2/PPp1P1pP/R3K2R w KQkq - 0 1", "E1", "C1", DisplayName = "King passing through check when castling")]
        [DataRow("r3k2r/ppp1p1pp/3pBp2/8/8/3PbP2/PPP1P1PP/R3K2R b KQkq - 0 1", "E8", "C8", DisplayName = "King passing through check when castling")]
        [DataRow("r3k2r/pppppppp/2N3N1/8/8/2n3n1/PPPPPPPP/R3K2R b KQkq - 0 1", "E8", "G8", DisplayName = "King passing through check when castling")]
        [DataRow("r3k2r/ppp1p1pp/3pNp2/8/8/3PnP2/PPP1P1PP/R3K2R b KQkq - 0 1", "E8", "C8", DisplayName = "King passing through check when castling")]
        [DataRow("r3k2r/ppP1p1Pp/3p1p2/8/8/3P1P2/PPp1P1pP/R3K2R b KQkq - 0 1", "E8", "G8", DisplayName = "King passing through check when castling")]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R b KQkq - 0 1", "E8", "C8", DisplayName = "King has moved")]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R b KQkq - 0 1", "E8", "G8", DisplayName = "King has moved")]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R w KQkq - 0 1", "E1", "C1", DisplayName = "King has moved")]
        [DataRow("r6r/pp2k2p/6p1/8/8/1P4P1/P3K2P/R6R w KQkq - 0 1", "E1", "G1", DisplayName = "King has moved")]
        [DataRow("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R w - - 0 1", "E1", "G1", DisplayName = "Castling not available")]
        [DataRow("r3k2r/pppppppp/8/8/8/8/PPPPPPPP/R3K2R b - - 0 1", "E8", "G8", DisplayName = "Castling not available")]
        [DataRow("1r2k2r/pp5p/6p1/8/8/1P4P1/P6P/1R2K2R w KQkq - 0 1", "E1", "C1", DisplayName = "QueenSide rook has moved")]
        [DataRow("1r2k2r/pp5p/6p1/8/8/1P4P1/P6P/1R2K2R b KQkq - 0 1", "E8", "C8", DisplayName = "QueenSide rook has moved")]
        [DataRow("r3k1r1/pp5p/6p1/8/8/1P4P1/P6P/R3K1R1 w KQkq - 0 1", "E1", "G1", DisplayName = "KingSide rook has moved")]
        [DataRow("r3k1r1/pp5p/6p1/8/8/1P4P1/P6P/R3K1R1 b KQkq - 0 1", "E8", "G8", DisplayName = "KingSide rook has moved")]
        [DataRow("r3k2r/pp5p/6p1/8/8/1Pb3P1/P6P/R3K2R w KQkq - 0 1", "E1", "G1", DisplayName = "King in check")]
        [DataRow("r3k2r/pp5p/2B3p1/8/8/1P4P1/P6P/R3K2R b KQkq - 0 1", "E8", "G8", DisplayName = "King in check")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "E1", "G1", DisplayName = "Piece in between king and rook")]
        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "E1", "C1", DisplayName = "Piece in between king and rook")]
        [DataRow("r1b1kb1r/ppp1pppp/2nq1n2/3p4/3P4/2NQ1N2/PPP1PPPP/R1B1KB1R b KQkq - 0 1", "E8", "G8", DisplayName = "Piece in between king and rook")]
        [DataRow("r1b1kb1r/ppp1pppp/2nq1n2/3p4/3P4/2NQ1N2/PPP1PPPP/R1B1KB1R b KQkq - 0 1", "E8", "C8", DisplayName = "Piece in between king and rook")]
        [TestMethod]
        public void Play_TryingToMakeCastlingMoveWhenItsNotAble_ShouldThrowException(string position, string origin, string destination)
        {
            Assert.ThrowsException<ChessboardException>(() => FakeMatch.RestoreAndPlay(position, origin, destination));
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", "H1", "G1", "r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K1R1 b Qkq - 1 1")]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", "A1", "B1", "r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/1R2K2R b Kkq - 1 1")]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", "E1", "D1", "r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R2K3R b kq - 1 1")]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", "E8", "D8", "r2k3r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQ - 1 2")]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", "H8", "G8", "r3k1r1/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQq - 1 2")]
        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R b KQkq - 0 1", "A8", "B8", "1r2k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQk - 1 2")]
        [TestMethod]
        public void Play_MovingKingOrRook_ShouldLoseQueenSideCastling(string fen, string origin, string destination, string expectedFen)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedFen, match.FenString);
        }
    }
}