using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PGNBuilderTests
    {
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 w - - 0 1", "F7", "F8", "Q", "1. f8=Q")]
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 b - - 0 1", "B2", "B1", "Q", "1. b1=Q")]
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 w - - 0 1", "F7", "F8", "R", "1. f8=R")]
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 b - - 0 1", "B2", "B1", "R", "1. b1=R")]
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 w - - 0 1", "F7", "F8", "B", "1. f8=B")]
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 b - - 0 1", "B2", "B1", "B", "1. b1=B")]
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 w - - 0 1", "F7", "F8", "N", "1. f8=N")]
        [DataRow("8/5P2/8/4k3/8/3Q4/1p2K3/8 b - - 0 1", "B2", "B1", "N", "1. b1=N")]
        [TestMethod]
        public void Play_Promoting_ShouldAddPromotingSign(string fen, string origin, string destination, string promoting, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination, promoting);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 w - - 0 1", "F7", "G8", "Q", "1. fxg8=Q")]
        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 b - - 0 1", "B2", "A1", "Q", "1. bxa1=Q")]
        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 w - - 0 1", "F7", "G8", "R", "1. fxg8=R")]
        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 b - - 0 1", "B2", "A1", "R", "1. bxa1=R")]
        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 w - - 0 1", "F7", "G8", "B", "1. fxg8=B")]
        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 b - - 0 1", "B2", "A1", "B", "1. bxa1=B")]
        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 w - - 0 1", "F7", "G8", "N", "1. fxg8=N")]
        [DataRow("6n1/5P2/8/4k3/8/3Q4/1p2K3/R7 b - - 0 1", "B2", "A1", "N", "1. bxa1=N")]
        [TestMethod]
        public void Play_PromotingWithCapture_ShouldAddCaptureAndPromotionSign(string fen, string origin, string destination, string promoting, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination, promoting);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("rnbk1bnr/pp1Ppppp/1qp5/8/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "R", "1. dxc8=R+")]
        [DataRow("rnbk1bnr/pp1Ppppp/1qp5/8/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "Q", "1. dxc8=Q+")]
        [DataRow("rnbk1bnr/pp1Ppppp/8/8/8/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "B", "1. dxc8=B+")]
        [DataRow("rnbk1bnr/pp1Ppppp/8/8/8/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "D7", "C8", "N", "1. dxc8=N+")]
        [TestMethod]
        public void Play_PromotingPawnWithCaptureAndCheck_ShouldAddCapturePromotionAndCheckSign(string fen, string origin, string destination, string promoting, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination, promoting);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1", "E2", "E4", "1. e4", DisplayName = "Pawn move")]
        [DataRow("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR b KQkq - 0 1", "E7", "E5", "1. e5", DisplayName = "Pawn move")]
        [DataRow("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D2", "D4", "1. d4", DisplayName = "Pawn move")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR w KQkq - 0 1", "B1", "C3", "1. Nc3")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR w KQkq - 0 1", "A1", "A2", "1. Ra2")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR w KQkq - 0 1", "F1", "B5", "1. Bb5")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR w KQkq - 0 1", "D1", "G4", "1. Qg4")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR w KQkq - 0 1", "E1", "E2", "1. Ke2")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR b KQkq - 0 1", "E8", "E7", "1. Ke7")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR b KQkq - 0 1", "A8", "A7", "1. Ra7")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR b KQkq - 0 1", "B8", "C6", "1. Nc6")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR b KQkq - 0 1", "D8", "H4", "1. Qh4")]
        [DataRow("rnbqkbnr/1ppp1pp1/p6p/4p3/4P3/P6P/1PPP1PP1/RNBQKBNR b KQkq - 0 1", "F8", "B4", "1. Bb4")]
        [TestMethod]
        public void Play_Default_ShouldAddPGNInCorrectFormat(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("rnbqkbnr/1pp2pp1/p6p/8/8/P6P/1PP2PP1/RNBQKBNR w KQkq - 0 1", "D1", "E2", "1. Qe2+")]
        [DataRow("rnbqkbnr/1pp2pp1/p6p/8/8/P6P/1PP2PP1/RNBQKBNR w KQkq - 0 1", "F1", "B5", "1. Bb5+")]
        [DataRow("rnbqkbnr/1pp2pp1/p6p/8/8/P6P/1PP2PP1/RNBQKBNR b KQkq - 0 1", "F8", "B4", "1. Bb4+")]
        [DataRow("rnbqkbnr/1pp2pp1/p6p/8/8/P6P/1PP2PP1/RNBQKBNR b KQkq - 0 1", "D8", "E7", "1. Qe7+")]
        [DataRow("r1bqkb1r/ppp2ppp/2nP1n2/4p3/8/2N2N2/PPPP1PPP/R1BQKB1R w KQkq - 0 1", "D6", "D7", "1. d7+")]
        [DataRow("r1bqkb1r/ppp2ppp/2n2n2/8/8/2Np4/PPP3PP/R1BQKB1R b KQkq - 0 1", "D3", "D2", "1. d2+")]
        [TestMethod]
        public void Play_WithCheck_ShouldAddCheckSign(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("rnbqkbnr/pppp1ppp/8/4p3/3PP3/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "D4", "E5", "1. dxe5", DisplayName = "Pawn move")]
        [DataRow("rnbqkbnr/pppp2pp/5p2/4P3/4P3/8/PPP2PPP/RNBQKBNR b KQkq - 0 1", "F6", "E5", "1. fxe5", DisplayName = "Pawn move")]
        [DataRow("rnb1kbnr/ppp3pp/8/3qp3/4P3/8/PPP2PPP/RNBQKBNR w KQkq - 0 1", "E4", "D5", "1. exd5", DisplayName = "Pawn move")]
        [DataRow("rnb1kbnr/ppp3pp/B7/3Pp3/8/8/PPP2PPP/RNBQK1NR b KQkq - 0 1", "B7", "A6", "1. bxa6", DisplayName = "Pawn move")]
        [DataRow("rnb1kbnr/p1p3pp/8/8/1pP5/8/PP3PPP/RNBQK1NR b KQkq C3 0 1", "B4", "C3", "1. bxc3", DisplayName = "Pawn move")]
        [DataRow("r1bq1rk1/pppp1ppp/2n2n2/1B6/1b1QP3/2N2N2/PPP2PPP/R1B2RK1 w - - 0 1", "B5", "C6", "1. Bxc6")]
        [DataRow("r1bq1rk1/pppp1ppp/2n2n2/1B6/1b1QP3/2N2N2/PPP2PPP/R1B2RK1 w - - 0 1", "D4", "D7", "1. Qxd7")]
        [DataRow("r1bq1rk1/pppp1ppp/2n2n2/1B6/1b1QP3/2N2N2/PPP2PPP/R1B2RK1 b - - 0 1", "B4", "C3", "1. Bxc3")]
        [DataRow("r1bq1rk1/pppp1ppp/2n2n2/1B6/1b1QP3/2N2N2/PPP2PPP/R1B2RK1 b - - 0 1", "C6", "D4", "1. Nxd4")]
        [TestMethod]
        public void Play_WithCapture_ShouldAddCaptureSign(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("8/8/8/4k3/p4r2/1R4P1/2K5/8 w - - 0 1", "G3", "F4", "1. gxf4+", DisplayName = "Pawn move")]
        [DataRow("8/8/8/4k3/p4r2/1R4P1/2K5/8 b - - 0 1", "A4", "B3", "1. axb3+", DisplayName = "Pawn move")]
        [DataRow("8/3k4/8/4pP2/pP6/8/2K5/8 w - E6 0 1", "F5", "E6", "1. fxe6+", DisplayName = "Pawn move")]
        [DataRow("8/3k4/8/4pP2/pP6/8/2K5/8 b - B3 0 1", "A4", "B3", "1. axb3+", DisplayName = "Pawn move")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R w - - 0 1", "H1", "H8", "1. Rxh8+")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R w - - 0 1", "C2", "G6", "1. Bxg6+")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R w - - 0 1", "D5", "C7", "1. Nxc7+")]
        [DataRow("r3k2r/2p1q1b1/6b1/8/1n6/2B5/2B1Q3/R3K2R w - - 0 1", "E2", "E7", "1. Qxe7+")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "A8", "A1", "1. Rxa1+")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "B4", "C2", "1. Nxc2+")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "G7", "C3", "1. Bxc3+")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R b - - 0 1", "E7", "E2", "1. Qxe2+")]
        [TestMethod]
        public void Play_CaptureWithCheck_ShouldAddCaptureAndCheckSign(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("k7/1R6/1P6/p7/4BB2/8/5K2/8 w - - 0 1", "B7", "B8", "1. Rb8#")]
        [DataRow("r3k2r/2p1q1b1/6b1/3N4/1n6/2B5/2B1Q3/R3K2R w - - 0 1", "E2", "E7", "1. Qxe7#")]
        [DataRow("6k1/6Pp/8/5N2/8/rnB5/P7/4KR2 w - - 0 1", "F5", "H6", "1. Nh6#")]
        [DataRow("7k/6pp/8/8/8/rnB5/P7/4KR2 w - - 0 1", "F1", "F8", "1. Rf8#")]
        [DataRow("7k/6pp/8/8/8/rnB5/P7/4KR2 w - - 0 1", "F1", "F8", "1. Rf8#")]
        [DataRow("rnbqkbnr/ppppp2p/8/5Pp1/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D1", "H5", "1. Qh5#")]
        [DataRow("k7/3P3R/8/8/p7/1p2K3/8/8 w - - 0 1", "D7", "D8", "1. d8=Q#")]
        [DataRow("k1r5/3P3R/8/8/p7/1p2K3/8/8 w - - 0 1", "D7", "C8", "1. dxc8=Q#")]
        [DataRow("4k3/b7/1q6/8/8/5b2/6R1/7K b - - 0 1", "B6", "G1", "1. Qg1#")]
        [DataRow("6k1/5rp1/8/8/8/8/B5Q1/4K1R1 w - - 0 1", "G2", "G7", "1. Qxg7#")]
        [DataRow("7k/n7/p5PP/1p6/8/8/Q1B5/1R2K3 w - - 0 1", "G6", "G7", "1. g7#")]
        [DataRow("7k/n5p1/p4P1P/1p6/8/8/Q1B5/1R2K3 w - - 0 1", "F6", "G7", "1. fxg7#")]
        [TestMethod]
        public void Play_MoveResultingInCheckmate_ShouldAddCheckmateSign(string fen, string origin, string destination, string expectedPGNMove)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.AreEqual(expectedPGNMove, match.Moves.Peek());
        }

        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R b KQkq - 0 1", "E8", "G8")]
        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R w KQkq - 0 1", "E1", "G1")]
        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R b KQkq - 0 1", "E8", "C8")]
        [DataRow("r3k2r/pppq1ppp/2np1n2/1Bb1p1B1/4P1b1/2NP1N2/PPPQ1PPP/R3K2R w KQkq - 0 1", "E1", "C1")]
        [TestMethod]
        public void Play_Castling_ShouldAddCastlingSign(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            string castlingPgn = destination[0] == 'G' ? "O-O" : "O-O-O";

            Assert.AreEqual(castlingPgn, match.Moves.Peek());
        }

        [TestMethod]
        public void Play_PgnMoveList_ShouldAddPGNInCorrectFormatAndSequence()
        {
            Match match = new(Time.Ten);
            Guid player1Id = match.CurrentPlayerInfo!.Value.Id;
            Guid player2Id = match.OpponentPlayerInfo!.Value.Id;

            List<Move> moves = new()
            {
                new(player1Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4")),
                new(player2Id, Coordinate.GetInstance("D7"), Coordinate.GetInstance("D5")),
                new(player1Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("D5")),
                new(player2Id, Coordinate.GetInstance("D8"), Coordinate.GetInstance("D5")),
                new(player1Id, Coordinate.GetInstance("F1"), Coordinate.GetInstance("C4")),
                new(player2Id, Coordinate.GetInstance("D5"), Coordinate.GetInstance("C4")),
                new(player1Id, Coordinate.GetInstance("D2"), Coordinate.GetInstance("D3")),
                new(player2Id, Coordinate.GetInstance("C4"), Coordinate.GetInstance("D3")),
                new(player1Id, Coordinate.GetInstance("C2"), Coordinate.GetInstance("D3")),
            };
            foreach (Move move in moves)
            {
                match.Play(move);
            }

            List<string> expectedMoveList = new()
            {
                "9. cxd3",
                "8. Qxd3",
                "7. d3",
                "6. Qxc4",
                "5. Bc4",
                "4. Qxd5",
                "3. exd5",
                "2. d5",
                "1. e4",
            };

            CollectionAssert.AreEqual(expectedMoveList, match.Moves);
        }
    }
}