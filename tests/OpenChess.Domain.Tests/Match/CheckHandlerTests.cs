using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CheckHandlerTests
    {
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
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            Assert.IsTrue(new CheckHandler(chessboard, moveCalculator).IsInCheck(player, out CheckState checkAmount));
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
            Chessboard chessboard = new(fen);
            Color player = color == 'w' ? Color.White : Color.Black;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            Assert.IsFalse(new CheckHandler(chessboard, moveCalculator).IsInCheck(player, out CheckState checkAmount));
        }

        [DataRow("3b4/8/4n3/5PK1/8/k4r2/8/3r4 w - - 0 1", 'w')]
        [DataRow("r2qkb1r/1pp2ppp/p2p4/4p3/7b/1B1n1P2/PPPP2PP/RNBQK2R b KQkq - 0 1", 'w')]
        [DataRow("3b4/4n3/8/5PK1/2N5/k4R2/8/3r4 w - - 0 1", 'b')]
        [DataRow("r1bqkbnr/1pp2ppp/p1Bp1N2/4p3/4P3/8/PPPP1PPP/RNBQK2R b KQkq - 0 1", 'b')]
        [TestMethod]
        public void CheckState_DoubleCheck_ShouldReturnCorrectEnum(string fen, char color)
        {
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            new CheckHandler(chessboard, moveCalculator).IsInCheck(player, out CheckState checkState);

            Assert.AreEqual(CheckState.DoubleCheck, checkState);
        }

        [DataRow("rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1", 'b')]
        [DataRow("rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1", 'w')]
        [DataRow("rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1", 'b')]
        [DataRow("3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1", 'b')]
        [TestMethod]
        public void CheckState_Check_ShouldReturnCorrectEnum(string fen, char color)
        {
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            new CheckHandler(chessboard, moveCalculator).IsInCheck(player, out CheckState checkState);

            Assert.AreEqual(CheckState.Check, checkState);
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
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            new CheckHandler(chessboard, moveCalculator).IsInCheck(player, out CheckState checkState);

            Assert.AreEqual(CheckState.NotInCheck, checkState);
        }

        [DataRow("2N5/k7/8/2Q5/7p/8/8/4K3 b - - 0 1", "A7", "B7", "DoubleCheck")]
        [DataRow("8/k1R5/8/8/3B3p/8/8/4K3 b - - 0 1", "A7", "B8", "DoubleCheck")]
        [DataRow("8/8/kP6/8/8/3BK3/7p/8 b - - 0 1", "A6", "A5", "Check")]
        [DataRow("8/8/kP6/8/8/3BK3/7p/8 b - - 0 1", "A6", "B7", "Check")]
        [TestMethod]
        public void Play_ShouldSolveCheckByMovingTheKing(string fen, string origin, string destination, string checkState)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, checkState);
            Match match = new(matchInfo);

            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CheckState.NotInCheck, match.CurrentPlayerCheckState);
        }

        [DataRow("8/kR6/8/8/3B3p/8/8/4K3 b - - 0 1", "A7", "B7", "DoubleCheck")]
        [DataRow("8/2k5/2R5/Q7/7p/8/8/4K3 b - - 0 1", "C7", "C6", "DoubleCheck")]
        [DataRow("8/8/kP6/8/8/3BK3/7p/8 b - - 0 1", "A6", "B6", "DoubleCheck")]
        [TestMethod]
        public void Play_ShouldSolveDoubleCheckByCapturingAPieceWithTheKing(string fen, string origin, string destination, string checkState)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, checkState);
            Match match = new(matchInfo);

            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CheckState.NotInCheck, match.CurrentPlayerCheckState);
        }

        [DataRow("8/8/2k1P3/8/q7/8/2Q1K2p/8 b - - 0 1", "A4", "C2", "Check")]
        [DataRow("R6k/6pp/4P3/8/4b3/2K5/2Q4p/8 w - - 0 1", "E4", "A8", "Check")]
        [DataRow("6k1/6pp/4P2N/8/8/2K1b3/2Q4p/5R2 b - - 0 1", "E3", "H6", "Check")]
        [DataRow("8/5kpp/4P3/8/8/2K1b3/2Q4p/6R1 w - - 0 1", "F7", "E6", "Check")]
        [DataRow("k7/1B4pp/1P6/8/8/2K1b1B1/2Q4p/8 w - - 0 1", "A8", "B7", "Check")]
        [TestMethod]
        public void Play_ShouldSolveCheckByCapturingTheEnemyPiece(string fen, string origin, string destination, string checkState)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, checkState);
            Match match = new(matchInfo);

            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));
            match.Play(move);

            Assert.AreEqual(CheckState.NotInCheck, match.CurrentPlayerCheckState);
        }

        [DataRow("8/1r6/k1R5/8/8/3BK3/8/8 b - - 0 1", "B7", "B6")]
        [TestMethod]
        public void Play_TryingToSolveDoubleCheckByCoveringTheKing_ShouldThrowException(string fen, string origin, string destination)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, "DoubleCheck");
            Match match = new(matchInfo);
            Assert.AreEqual(CheckState.DoubleCheck, match.CurrentPlayerCheckState);
            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("3B4/8/kR6/8/8/3BK3/8/8 b - - 0 1", "A6", "B6")]
        [TestMethod]
        public void Play_TryingToSolveDoubleCheckByCapturingAProtectedPieceWithTheKing_ShouldThrowException(string fen, string origin, string destination)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, "DoubleCheck");
            Match match = new(matchInfo);
            Assert.AreEqual(CheckState.DoubleCheck, match.CurrentPlayerCheckState);
            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("8/8/2k5/7R/8/2Q1K3/7p/8 b - - 0 1", "C6", "C5", "DoubleCheck")]
        [DataRow("8/8/2k5/7R/8/2Q1K3/7p/8 b - - 0 1", "C6", "D5", "DoubleCheck")]
        [DataRow("8/8/2k5/7R/8/2Q1K3/7p/8 b - - 0 1", "C6", "C7", "DoubleCheck")]
        [DataRow("8/8/2k1P3/8/8/2Q1K3/7p/8 b - - 0 1", "C6", "D7", "DoubleCheck")]
        [TestMethod]
        public void Play_TryingToSolveCheckByMovingTheKingToAttackRangeOfEnemyPiece_ShouldThrowException(string fen, string origin, string destination, string checkState)
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(fen, checkState);
            Match match = new(matchInfo);
            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("k7/1R6/1P6/p7/4BB2/8/5K2/8 w - - 0 1", "B7", "B8")]
        [DataRow("6k1/6Pp/8/5N2/8/rnB5/P7/4KR2 w - - 0 1", "F5", "H6")]
        [DataRow("7k/6pp/8/8/8/rnB5/P7/4KR2 w - - 0 1", "F1", "F8")]
        [DataRow("7k/6pp/8/8/8/rnB5/P7/4KR2 w - - 0 1", "F1", "F8")]
        [DataRow("rnbqkbnr/ppppp2p/8/5Pp1/8/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D1", "H5")]
        [DataRow("k7/3P3R/8/8/p7/1p2K3/8/8 w - - 0 1", "D7", "D8")]
        [TestMethod]
        public void Play_MoveResultingInCheckmate_ShouldEndTheMatchAndDeclareWinner(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);

            Assert.IsTrue(match.HasFinished());
            Assert.AreEqual(match.Winner.GetValueOrDefault(), match.OpponentPlayer.GetValueOrDefault().Id);
        }

        [DataRow("8/8/2k1P3/8/8/1Q2K3/7p/8 w - - 0 1", "B3", "C3")]
        [DataRow("4k3/3b4/8/1B5R/8/8/8/4K3 w - - 0 1", "H5", "E5")]
        [DataRow("r6k/6pp/8/3R4/8/1n6/P7/4K3 w - - 0 1", "D5", "D8")]
        [DataRow("6k1/6Pp/8/5N2/8/rn6/P7/4KR2 w - - 0 1", "F5", "H6")]
        [DataRow("rnbqkbnr/ppppp1pp/8/5p2/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D1", "H5")]
        [DataRow("k7/6pp/BP6/8/8/2K1b1B1/2Q4p/8 w - - 0 1", "A6", "B7")]
        [TestMethod]
        public void Play_MoveResultingInCheckWithSolution_ShouldKeepMatchInProgress(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(fen, origin, destination);

            Assert.IsFalse(match.HasFinished());
            Assert.IsNull(match.Winner);
            Assert.AreNotEqual(CheckState.NotInCheck, match.CurrentPlayerCheckState);
            Assert.AreNotEqual(CheckState.Checkmate, match.CurrentPlayerCheckState);
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