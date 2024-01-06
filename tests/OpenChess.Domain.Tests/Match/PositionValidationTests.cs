using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PositionValidationTests
    {
        [DataRow("8/8/8/p7/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "Stalemate")]
        [DataRow("8/8/2b5/p2R4/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "Stalemate/pinned piece without solution")]
        [DataRow("8/8/8/8/8/8/3Kp1pp/5brk w - - 0 1", "D2", "E1", DisplayName = "Stalemate")]
        [DataRow("k7/P7/K7/8/8/8/8/4B3 w - - 0 1", "E1", "G3", DisplayName = "Stalemate")]
        [DataRow("8/8/8/8/2k5/rRn5/p7/K7 b - - 0 1", "C4", "B3", DisplayName = "Stalemate")]
        [DataRow("5r2/8/8/8/8/3kb3/3R4/3K4 b - - 0 1", "E3", "D2", DisplayName = "Stalemate")]
        [DataRow("8/8/3k4/3P4/8/4K3/8/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/Only kings left")]
        [DataRow("8/8/3k4/3Q4/8/4K3/8/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/Only kings left")]
        [DataRow("8/8/3k4/3B4/8/4K3/3B4/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/king + bishop left")]
        [DataRow("7B/8/3k4/3B4/8/4K3/3B4/8 b - - 0 1", "D6", "D5", DisplayName = "Dead Position/king + bishop left")]
        [DataRow("8/8/3k4/3b4/8/2bK4/8/8 w - - 0 1", "D3", "C3", DisplayName = "Dead Position/king + bishop left")]
        [DataRow("8/8/3k4/1B6/8/5p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Dead Position/bishops in same tile")]
        [DataRow("8/8/3k4/1B6/8/5p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Dead Position/bishops in same tile")]
        [DataRow("8/1B1B1b2/3k4/1B6/2b5/5p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Dead Position/bishops in same tile")]
        [DataRow("8/8/2k1n3/2P5/8/4K3/8/8 b - - 0 1", "C6", "C5", DisplayName = "Dead Position/king + knight left")]
        [DataRow("8/8/2k2N2/2P5/8/4K3/8/8 b - - 0 1", "C6", "C5", DisplayName = "Dead Position/king + knight left")]
        [TestMethod]
        public void Play_MoveResultingInDraw_ShouldFinishMatchWithoutWinner(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);

            Assert.IsNull(match.Winner);
            Assert.AreEqual(CurrentPositionStatus.Draw, match.CurrentPositionStatus);
            Assert.IsTrue(match.HasFinished());
        }

        [DataRow("8/8/2k2N2/2P5/6N1/4K3/8/8 b - - 0 1", "C6", "C5", DisplayName = "More than one knight left")]
        [DataRow("8/8/3k4/1B6/8/p4p2/2b2K2/8 w - - 0 1", "F2", "F3", DisplayName = "Not only bishops left")]
        [DataRow("8/8/3k4/1B6/8/2b2p2/5K2/8 w - - 0 1", "F2", "F3", DisplayName = "only bishops left but in different tiles")]
        [DataRow("8/8/8/8/2k5/rR6/p7/K7 b - - 0 1", "A3", "B3", DisplayName = "not stalemate")]
        [DataRow("8/8/2b5/p2B4/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "not stalemate/pinned piece with solution")]
        [DataRow("8/8/2b5/p2P4/P7/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "not stalemate/pinned piece with solution")]
        [DataRow("7r/8/8/p7/P6P/4k3/3q4/7K b - - 0 1", "D2", "F2", DisplayName = "not stalemate/pinned piece with solution")]
        [TestMethod]
        public void Play_MoveNotResultingInDraw_ShouldKeepMatchGoing(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);
            Assert.IsFalse(match.HasFinished());
        }

        [DataRow("r7/3R2k1/4P3/4K3/8/8/8/8 w - - 0 1", 'b')]
        [DataRow("rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1", 'b')]
        [DataRow("rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1", 'b')]
        [DataRow("3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1", 'b')]
        [DataRow("3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1", 'w')]
        [DataRow("r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1", 'w')]
        [DataRow("rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1", 'w')]
        [DataRow("rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1", 'w')]
        [TestMethod]
        public void IsInCheck_PlayerInCheck_ShouldReturnTrue(string fen, char color)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(match.Chessboard);
            CheckValidation checkValidation = new(match, moveCalculator);

            Assert.IsTrue(checkValidation.IsInCheck(player, out CurrentPositionStatus checkStatus));
        }

        [DataRow("r7/3R2k1/4P3/4K3/8/8/8/8 w - - 0 1", 'w')]
        [DataRow("rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1", 'w')]
        [DataRow("rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1", 'w')]
        [DataRow("3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1", 'w')]
        [DataRow("rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", 'w')]
        [DataRow("8/8/8/5PK1/8/k7/8/3r4 w - - 0 1", 'w')]
        [DataRow("3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1", 'b')]
        [DataRow("r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1", 'b')]
        [DataRow("rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1", 'b')]
        [DataRow("rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1", 'b')]
        [DataRow("rnbqkbnr/pp2pppp/2p5/3P4/3P4/8/PPP2PPP/RNBQKBNR b KQkq - 0 1", 'b')]
        [DataRow("rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", 'b')]
        [DataRow("8/8/8/5PK1/8/k7/8/3r4 w - - 0 1", 'b')]
        [TestMethod]
        public void IsInCheck_PlayerNotInCheck_ShouldReturnFalse(string fen, char color)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(match.Chessboard);
            CheckValidation checkValidation = new(match, moveCalculator);

            Assert.IsFalse(checkValidation.IsInCheck(player, out CurrentPositionStatus checkStatus));
        }

        [DataRow("3b4/8/4n3/5PK1/8/k4r2/8/3r4 w - - 0 1", 'w')]
        [DataRow("r2qkb1r/1pp2ppp/p2p4/4p3/7b/1B1n1P2/PPPP2PP/RNBQK2R b KQkq - 0 1", 'w')]
        [DataRow("3b4/4n3/8/5PK1/2N5/k4R2/8/3r4 w - - 0 1", 'b')]
        [DataRow("r1bqkbnr/1pp2ppp/p1Bp1N2/4p3/4P3/8/PPPP1PPP/RNBQK2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void CheckState_DoubleCheck_ShouldReturnCorrectEnum(string fen, char color)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(match.Chessboard);
            CheckValidation checkValidation = new(match, moveCalculator);
            checkValidation.IsInCheck(player, out CurrentPositionStatus checkStatus);

            Assert.AreEqual(CurrentPositionStatus.DoubleCheck, checkStatus);
        }

        [DataRow("rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1", 'b')]
        [DataRow("rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1", 'w')]
        [DataRow("rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1", 'b')]
        [DataRow("3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1", 'b')]
        [TestMethod]
        public void CheckState_Check_ShouldReturnCorrectEnum(string fen, char color)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(match.Chessboard);
            CheckValidation checkValidation = new(match, moveCalculator);
            checkValidation.IsInCheck(player, out CurrentPositionStatus checkStatus);

            Assert.AreEqual(CurrentPositionStatus.Check, checkStatus);
        }

        [DataRow("3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1", 'w')]
        [DataRow("rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", 'w')]
        [DataRow("8/8/8/5PK1/8/k7/8/3r4 w - - 0 1", 'w')]
        [DataRow("3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1", 'b')]
        [DataRow("r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1", 'b')]
        [DataRow("rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1", 'b')]
        [DataRow("rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void CheckState_NotInCheck_ShouldReturnCorrectEnum(string fen, char color)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(match.Chessboard);
            CheckValidation checkValidation = new(match, moveCalculator);
            checkValidation.IsInCheck(player, out CurrentPositionStatus checkStatus);

            Assert.AreEqual(CurrentPositionStatus.NotInCheck, checkStatus);
        }

        [DataRow("2N5/k7/8/2Q5/7p/8/8/4K3 b - - 0 1", "A7", "B7")]
        [DataRow("8/k1R5/8/8/3B3p/8/8/4K3 b - - 0 1", "A7", "B8")]
        [DataRow("8/8/kP6/8/8/3BK3/7p/8 b - - 0 1", "A6", "A5")]
        [DataRow("8/8/kP6/8/8/3BK3/7p/8 b - - 0 1", "A6", "B7")]
        [TestMethod]
        public void Play_ShouldSolveCheckByMovingTheKing(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CurrentPositionStatus.NotInCheck, match.CurrentPositionStatus);
        }

        [DataRow("8/kR6/8/8/3B3p/8/8/4K3 b - - 0 1", "A7", "B7")]
        [DataRow("8/2k5/2R5/Q7/7p/8/8/4K3 b - - 0 1", "C7", "C6")]
        [DataRow("8/8/kP6/8/8/3BK3/7p/8 b - - 0 1", "A6", "B6")]
        [TestMethod]
        public void Play_ShouldSolveDoubleCheckByCapturingAPieceWithTheKing(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CurrentPositionStatus.NotInCheck, match.CurrentPositionStatus);
        }

        [DataRow("8/8/2k1P3/8/q7/8/2Q4p/4K3 b - - 0 1", "A4", "C2")]
        [DataRow("R6k/6pp/4P3/8/4b3/2K5/2Q4p/8 b - - 0 1", "E4", "A8")]
        [DataRow("6k1/6pp/4P2N/8/8/2K1b3/2Q4p/5R2 b - - 0 1", "E3", "H6")]
        [DataRow("8/5kpp/4P3/8/8/2K1b3/2Q4p/6R1 b - - 0 1", "F7", "E6")]
        [DataRow("k7/1B4pp/1P6/8/8/2K1b1B1/2Q4p/8 b - - 0 1", "A8", "B7")]
        [TestMethod]
        public void Play_ShouldSolveCheckByCapturingTheEnemyPiece(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CurrentPositionStatus.NotInCheck, match.CurrentPositionStatus);
        }

        [DataRow("8/1r6/k1R5/8/8/3BK3/8/8 b - - 0 1", "B7", "B6")]
        [TestMethod]
        public void Play_TryingToSolveDoubleCheckByCoveringTheKing_ShouldThrowException(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Assert.IsNull(match.CurrentPositionStatus);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("3B4/8/kR6/8/8/3BK3/8/8 b - - 0 1", "A6", "B6")]
        [TestMethod]
        public void Play_TryingToSolveDoubleCheckByCapturingAProtectedPieceWithTheKing_ShouldThrowException(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Assert.IsNull(match.CurrentPositionStatus);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("8/8/2k5/7R/8/2Q1K3/7p/8 b - - 0 1", "C6", "C5")]
        [DataRow("8/8/2k5/7R/8/2Q1K3/7p/8 b - - 0 1", "C6", "D5")]
        [DataRow("8/8/2k5/7R/8/2Q1K3/7p/8 b - - 0 1", "C6", "C7")]
        [DataRow("8/8/2k1P3/8/8/2Q1K3/7p/8 b - - 0 1", "C6", "D7")]
        [TestMethod]
        public void Play_TryingToSolveCheckByMovingTheKingToAttackRangeOfEnemyPiece_ShouldThrowException(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("k7/1R6/1P6/p7/4BB2/8/5K2/8 w - - 0 1", "B7", "B8")]
        [DataRow("6k1/6Pp/8/5N2/8/rnB5/P7/4KR2 w - - 0 1", "F5", "H6")]
        [DataRow("7k/6pp/8/8/8/rnB5/P7/4KR2 w - - 0 1", "F1", "F8")]
        [DataRow("7k/6pp/8/8/8/rnB5/P7/4KR2 w - - 0 1", "F1", "F8")]
        [DataRow("rnbqkbnr/ppppp2p/8/5Pp1/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D1", "H5")]
        [DataRow("k7/3P3R/8/8/p7/1p2K3/8/8 w - - 0 1", "D7", "D8")]
        [DataRow("4k3/b7/1q6/8/8/5b2/6R1/7K b - - 0 1", "B6", "G1", DisplayName = "Pinned piece")]
        [DataRow("6k1/5rp1/8/8/8/8/B5Q1/4K1R1 w - - 0 1", "G2", "G7", DisplayName = "Pinned piece")]
        [DataRow("7k/n7/p5PP/1p6/8/8/Q1B5/1R2K3 w - - 0 1", "G6", "G7")]
        [DataRow("7k/n5p1/p4P1P/1p6/8/8/Q1B5/1R2K3 w - - 0 1", "F6", "G7")]
        [DataRow("k1r5/3P3R/8/8/p7/1p2K3/8/8 w - - 0 1", "D7", "C8")]
        [TestMethod]
        public void Play_MoveResultingInCheckmate_ShouldEndTheMatchAndDeclareWinner(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Guid currentPlayer = match.CurrentPlayerInfo!.Value.Id;
            Move move = new(currentPlayer, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.IsTrue(match.HasFinished());
            Assert.AreEqual(match.Winner.GetValueOrDefault(), currentPlayer);
        }

        [DataRow("8/8/2k1P3/8/8/1Q2K3/7p/8 w - - 0 1", "B3", "C3")]
        [DataRow("4k3/3b4/8/1B5R/8/8/8/4K3 w - - 0 1", "H5", "E5")]
        [DataRow("r6k/6pp/8/3R4/8/1n6/P7/4K3 w - - 0 1", "D5", "D8")]
        [DataRow("6k1/6Pp/8/5N2/8/rn6/P7/4KR2 w - - 0 1", "F5", "H6")]
        [DataRow("rnbqkbnr/ppppp1pp/8/5p2/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D1", "H5")]
        [DataRow("k7/6pp/BP6/8/8/2K1b1B1/2Q4p/8 w - - 0 1", "A6", "B7")]
        [DataRow("6k1/5r2/8/8/8/8/6Q1/4K1R1 w - - 0 1", "G2", "G7")]
        [DataRow("6k1/p7/1p3Q2/8/7P/8/8/4K3 w - - 0 1", "F6", "G7")]
        [DataRow("4k3/3b4/8/1B1R4/8/8/8/4K3 w - - 0 1", "D5", "E5", DisplayName = "Pinned piece")]
        [TestMethod]
        public void Play_MoveResultingInCheckWithSolution_ShouldKeepMatchInProgress(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);

            Assert.IsFalse(match.HasFinished());
            Assert.IsNull(match.Winner);
            Assert.AreNotEqual(CurrentPositionStatus.NotInCheck, match.CurrentPositionStatus);
            Assert.AreNotEqual(CurrentPositionStatus.Checkmate, match.CurrentPositionStatus);
        }

        [DataRow("4k3/8/8/8/7b/2q3R1/8/4K3 w - - 0 1", "G3", "C3")]
        [DataRow("4k3/3b4/8/1B2R3/8/8/8/4K3 b - - 0 1", "D7", "E6")]
        [TestMethod]
        public void Play_TryingToSolveCheckByMovingPinnedPiece_ShouldThrowException(string fen, string origin, string destination)
        {
            Assert.ThrowsException<ChessboardException>(() => FakeMatch.RestoreAndPlay(fen, origin, destination));
        }
    }
}