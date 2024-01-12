using OpenChess.Domain;

namespace OpenChess.Tests
{

    [TestClass]
    public class MatchTests
    {
        private readonly string _fen = "rnbqkbnr/ppp1pppp/8/3p4/4P3/8/PPPP1PPP/RNBQKBNR w KQkq D6 0 1";

        [DataRow(3)]
        [DataRow(5)]
        [DataRow(10)]
        [DataRow(15)]
        [DataRow(30)]
        [TestMethod]
        public void NewInstance_GivenTime_ShouldCreateNewMatch(int time)
        {
            Time timeEnum = (Time)Enum.ToObject(typeof(Time), time);
            Match match = new(timeEnum);

            Assert.IsNotNull(match.Id);
            Assert.IsFalse(match.IsFull());
            Assert.IsNull(match.CurrentPlayerInfo);
            Assert.IsNull(match.Winner);
            Assert.AreEqual(match.FenString, FenInfo.InitialPosition);
            Assert.AreEqual(match.CurrentPositionStatus, CurrentPositionStatus.NotInCheck);
            Assert.AreEqual(time, (int)match.Duration);
        }

        [TestMethod]
        public void NewInstance_ShouldRestoreGameStateCorrectly()
        {
            long timeRemaining = TimeSpan.FromMinutes(5).Ticks;
            string currentTurnStartedAt = DateTime.UtcNow.ToString();
            MatchInfo matchInfo = FakeMatch.RestoreMatch(_fen, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), timeRemaining, timeRemaining, currentTurnStartedAt);
            Match match = new(matchInfo);

            Assert.AreEqual(match.Id, matchInfo.MatchId);
            Assert.AreEqual(match.Duration, Time.Five);
            Assert.AreEqual(match.Status, MatchStatus.InProgress);
            Assert.IsNull(match.CurrentPositionStatus);
            Assert.AreEqual(match.FenString, matchInfo.Fen);
            Assert.IsNull(match.Winner);
            CollectionAssert.AreEquivalent(match.Players, matchInfo.Players);
            CollectionAssert.AreEqual(match.Moves, matchInfo.PgnMoves);
        }

        [TestMethod]
        public void NewInstance_RestoringGameState_InvalidProperties_ShouldThrowException()
        {
            string matchId = Guid.NewGuid().ToString();
            string player1 = Guid.NewGuid().ToString();
            string player2 = Guid.NewGuid().ToString();
            long timeRemaining = TimeSpan.FromMinutes(5).Ticks;
            string currentTurnStartedAt = DateTime.UtcNow.ToString();
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, player2, timeRemaining, timeRemaining, currentTurnStartedAt, 6));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, player2, timeRemaining, timeRemaining, currentTurnStartedAt, 5, "imProges"));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, "invalidId", timeRemaining, timeRemaining, currentTurnStartedAt));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, "invalidId", player2, timeRemaining, timeRemaining, currentTurnStartedAt));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, "invalidId", player1, player2, timeRemaining, timeRemaining, currentTurnStartedAt));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, player2, timeRemaining, timeRemaining, currentTurnStartedAt, 5, "InProgress", "invalidId"));
        }

        [TestMethod]
        public void Join_MatchNotFull_ShouldAssignPlayerToMatch()
        {
            Match match = new(Time.Ten);
            PlayerInfo player = match.CreateNewPlayer(Color.White);

            match.Join(player);

            Assert.IsTrue(match.HasPlayer());
            Assert.AreEqual(MatchStatus.NotStarted, match.Status);
            Assert.IsFalse(match.IsFull());
            Assert.IsNull(match.CurrentPlayerInfo);
        }

        [TestMethod]
        public void Join_WhenAllPlayersJoined_ShouldChangeStatusAndCurrentPlayer()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = match.CreateNewPlayer(Color.White);
            PlayerInfo blackPlayer = match.CreateNewPlayer(Color.Black);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.AreEqual(MatchStatus.InProgress, match.Status);
            Assert.AreEqual(whitePlayer.Id, match.CurrentPlayerInfo!.Value.Id);
        }

        [TestMethod]
        public void Join_AddingSamePlayerTwice_ShouldThrowException()
        {
            TimeSpan timeRemaining = TimeSpan.FromMinutes(9);
            Match match = new(Time.Ten);
            PlayerInfo player = match.CreateNewPlayer(Color.White);
            PlayerInfo fakePlayer = new(player.Id, Color.Black, timeRemaining.Ticks);

            match.Join(player);

            Assert.ThrowsException<MatchException>(() => match.Join(player));
            Assert.ThrowsException<MatchException>(() => match.Join(fakePlayer));
        }

        [TestMethod]
        public void Join_AddingPlayerThatIsAlreadyInAnotherMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            TimeSpan timeRemaining = TimeSpan.FromMinutes(9);
            PlayerInfo player = new(Guid.NewGuid(), Color.White, timeRemaining.Ticks, null);
            match.Join(player);
            Match match2 = new(Time.Ten);

            Assert.ThrowsException<MatchException>(() => match2.Join(match.Players.FirstOrDefault()));
        }

        [TestMethod]
        public void Join_FullMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = match.CreateNewPlayer(Color.White);
            PlayerInfo blackPlayer = match.CreateNewPlayer(Color.Black);

            PlayerInfo otherPlayer = match.CreateNewPlayer(Color.White);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.ThrowsException<MatchException>(() => match.Join(otherPlayer));
        }

        [TestMethod]
        public void IsFull_ShouldReturnTrue()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = match.CreateNewPlayer(Color.White);
            PlayerInfo blackPlayer = match.CreateNewPlayer(Color.Black);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.IsTrue(match.IsFull());
        }

        [TestMethod]
        public void IsFull_ShouldReturnFalse()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = match.CreateNewPlayer(Color.White);

            match.Join(whitePlayer);

            Assert.IsFalse(match.IsFull());
        }

        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R3K2R w KQkq - 0 1", "G2", "G3", "r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1NPP/PPP1QP2/R3K2R b KQkq - 0 1", DisplayName = "Pawn move")]
        [DataRow("rnbqk1nr/p4pp1/8/2bp2Pp/pP1PPp2/5N2/4B2P/RNBQK2R b KQkq B3 0 1", "A4", "B3", "rnbqk1nr/p4pp1/8/2bp2Pp/3PPp2/1p3N2/4B2P/RNBQK2R w KQkq - 0 2", DisplayName = "Pawn enPassant capture")]
        [DataRow("rnbqk1nr/p4pp1/8/2bp2Pp/3PPp2/p4N2/1P2B2P/RNBQK2R w KQkq H6 0 1", "G5", "H6", "rnbqk1nr/p4pp1/7P/2bp4/3PPp2/p4N2/1P2B2P/RNBQK2R b KQkq - 0 1", DisplayName = "Pawn enPassant capture")]
        [DataRow("rnbqk1nr/p4pp1/7P/2bp4/3PPp2/p4N2/1P2B2P/RNBQK2R w KQkq - 0 1", "B2", "A3", "rnbqk1nr/p4pp1/7P/2bp4/3PPp2/P4N2/4B2P/RNBQK2R b KQkq - 0 1", DisplayName = "Pawn move with capture")]
        [DataRow("rnbqk1nr/p4pp1/7P/2bp4/3PPp2/P4N2/4B2P/RNBQK2R b KQkq - 0 1", "G7", "H6", "rnbqk1nr/p4p2/7p/2bp4/3PPp2/P4N2/4B2P/RNBQK2R w KQkq - 0 2", DisplayName = "Pawn move with capture")]
        [DataRow("rnbqkbnr/ppp2ppp/8/3p4/4Pp2/5N2/PPPP2PP/RNBQKB1R w KQkq - 0 1", "D2", "D4", "rnbqkbnr/ppp2ppp/8/3p4/3PPp2/5N2/PPP3PP/RNBQKB1R b KQkq D3 0 1", DisplayName = "Pawn forward move")]
        [DataRow("rnbqkbnr/ppp2ppp/8/3p4/3PPp2/5N2/PPP3PP/RNBQKB1R b KQkq D3 0 1", "C7", "C5", "rnbqkbnr/pp3ppp/8/2pp4/3PPp2/5N2/PPP3PP/RNBQKB1R w KQkq C6 0 2", DisplayName = "Pawn forward move")]
        [DataRow("rnbqkbnr/pp3ppp/8/2pp4/3PPp2/5N2/PPP3PP/RNBQKB1R w KQkq C6 0 1", "C2", "C3", "rnbqkbnr/pp3ppp/8/2pp4/3PPp2/2P2N2/PP4PP/RNBQKB1R b KQkq - 0 1", DisplayName = "Pawn forward move")]
        [DataRow("rnbqkbnr/pp3ppp/8/2pp4/3PPp2/2P2N2/PP4PP/RNBQKB1R b KQkq - 0 1", "C5", "D4", "rnbqkbnr/pp3ppp/8/3p4/3pPp2/2P2N2/PP4PP/RNBQKB1R w KQkq - 0 2", DisplayName = "Pawn diagonal move")]
        [DataRow("rnbqkbnr/pp3ppp/8/3p4/3pPp2/2P2N2/PP4PP/RNBQKB1R w KQkq - 0 1", "C3", "D4", "rnbqkbnr/pp3ppp/8/3p4/3PPp2/5N2/PP4PP/RNBQKB1R b KQkq - 0 1", DisplayName = "Pawn diagonal move")]
        [DataRow("r1b1k1nr/ppp2pp1/2n4p/2bp3q/3NP3/2N2Q1P/PPP2PP1/R1B1KB1R w KQkq - 0 1", "F3", "H5", "r1b1k1nr/ppp2pp1/2n4p/2bp3Q/3NP3/2N4P/PPP2PP1/R1B1KB1R b KQkq - 0 1", DisplayName = "Queen Move with capture")]
        [DataRow("6k1/5rp1/6p1/8/8/8/B5Q1/4K1R1 w - - 0 1", "G2", "G6", "6k1/5rp1/6Q1/8/8/8/B7/4K1R1 b - - 0 1", DisplayName = "Queen Move with capture")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPbN1/P2P1B2/b1NB2PP/1PP1QP2/R3K2R w KQkq - 0 1", "A1", "A3", "r3k2r/pp3ppp/n1pqpn2/3pPbN1/P2P1B2/R1NB2PP/1PP1QP2/4K2R b Kkq - 0 1", DisplayName = "Rook Move with capture")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPbN1/1b1P1B2/2NB2PP/PPP1QP2/R3K2R b KQkq - 0 1", "F5", "D3", "r3k2r/pp3ppp/n1pqpn2/3pP1N1/1b1P1B2/2Nb2PP/PPP1QP2/R3K2R w KQkq - 0 2", DisplayName = "Bishop Move with capture")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPbN1/1b1PQB2/2NB2PP/PPP2P2/R3K2R b KQkq - 0 1", "F6", "E4", "r3k2r/pp3ppp/n1pqp3/3pPbN1/1b1PnB2/2NB2PP/PPP2P2/R3K2R w KQkq - 0 2", DisplayName = "Knight Move with capture")]
        [DataRow("8/4k3/3q1R2/6np/7K/p7/8/8 w - - 0 1", "H4", "H5", "8/4k3/3q1R2/6nK/8/p7/8/8 b - - 0 1", DisplayName = "King Move with capture")]
        [DataRow("8/4k3/3q1R2/6np/7K/p7/8/8 b - - 0 1", "E7", "F6", "8/8/3q1k2/6np/7K/p7/8/8 w - - 0 2", DisplayName = "King Move with capture")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPbN1/1b1P1B2/2NB2PP/PPP1QP2/R3K2R w KQkq - 0 1", "E2", "E4", "r3k2r/pp3ppp/n1pqpn2/3pPbN1/1b1PQB2/2NB2PP/PPP2P2/R3K2R b KQkq - 1 1", DisplayName = "Queen Move")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPbN1/1b1P1B2/2NB2PP/PPP1QP2/R3K2R b KQkq - 0 1", "A8", "C8", "2r1k2r/pp3ppp/n1pqpn2/3pPbN1/1b1P1B2/2NB2PP/PPP1QP2/R3K2R w KQk - 1 2", DisplayName = "Rook Move")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPbN1/1b1P1B2/2NB2PP/PPP1QP2/R3K2R b KQkq - 0 1", "B4", "A5", "r3k2r/pp3ppp/n1pqpn2/b2pPbN1/3P1B2/2NB2PP/PPP1QP2/R3K2R w KQkq - 1 2", DisplayName = "Bishop Move")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1NPP/PPP1QP2/R3K2R w KQkq - 0 1", "F3", "G5", "r3k2r/pp3ppp/n1pqpn2/3pPbN1/1b1P1B2/2NB2PP/PPP1QP2/R3K2R b KQkq - 1 1", DisplayName = "Knight Move")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1NPP/PPP1QP2/R3K2R w KQkq - 0 1", "E1", "D1", "r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1NPP/PPP1QP2/R2K3R b kq - 1 1", DisplayName = "King Move")]
        [DataRow("8/4k3/4n3/q5R1/7K/8/8/4R3 w - - 0 1", "G5", "G7", "8/4k1R1/4n3/q7/7K/8/8/4R3 b - - 1 1", DisplayName = "Move resulting in check")]
        [DataRow("8/4k3/4n3/q5R1/7K/8/8/4R3 b - - 0 1", "A5", "A4", "8/4k3/4n3/6R1/q6K/8/8/4R3 w - - 1 2", DisplayName = "Move resulting in check")]
        [DataRow("r1b1k1nr/ppp2pp1/2n4p/2bp3q/3NP3/2N2Q1P/PPP2PP1/R1B1KB1R w KQkq - 0 1", "F3", "F7", "r1b1k1nr/ppp2Qp1/2n4p/2bp3q/3NP3/2N4P/PPP2PP1/R1B1KB1R b KQkq - 0 1", DisplayName = "Move resulting in capture with check")]
        [DataRow("8/4k3/4n3/q5R1/7K/8/8/4R3 b - - 0 1", "A5", "G5", "8/4k3/4n3/6q1/7K/8/8/4R3 w - - 0 2", DisplayName = "Move resulting in capture with check")]
        [DataRow("8/4k3/4n3/q5R1/7K/8/8/4R3 w - - 0 1", "E1", "E6", "8/4k3/4R3/q5R1/7K/8/8/8 b - - 0 1", DisplayName = "Move resulting in capture with check")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R3K2R w KQkq - 0 1", "E1", "G1", "r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R4RK1 b kq - 1 1", DisplayName = "white player castling kingside")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R3K2R w KQkq - 0 1", "E1", "C1", "r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/2KR3R b kq - 1 1", DisplayName = "white player castling queenside")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R3K2R b KQkq - 0 1", "E8", "G8", "r4rk1/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R3K2R w KQ - 1 2", DisplayName = "black player castling kingside")]
        [DataRow("r3k2r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R3K2R b KQkq - 0 1", "E8", "C8", "2kr3r/pp3ppp/n1pqpn2/3pPb2/1b1P1B2/2NB1N1P/PPP1QPP1/R3K2R w KQ - 1 2", DisplayName = "black player castling queenside")]
        [TestMethod]
        public void Play_ValidMove_ShouldBeHandled(string fen, string origin, string destination, string expectedFen)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Guid currentPlayer = match.CurrentPlayerInfo!.Value.Id;
            Move move = new(currentPlayer, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            match.Play(move);

            Assert.AreNotEqual(match.CurrentPlayerInfo, move.PlayerId);
            Assert.AreEqual(match.FenString, expectedFen);
            Assert.IsTrue(match.Moves.Any());
        }

        [DataRow("6b1/7P/8/6K1/8/k7/1p6/3r4 w - - 0 1", "H7", "G8", "6Q1/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "Q", DisplayName = "Pawn promotion with capture")]
        [DataRow("8/7P/8/6K1/8/k7/1p6/3r4 w - - 0 1", "H7", "H8", "7Q/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "Q", DisplayName = "Pawn promotion to queen")]
        [DataRow("7Q/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "B2", "B1", "7Q/8/8/6K1/8/k7/8/1q1r4 w - - 0 2", "Q", DisplayName = "Pawn promotion to queen")]
        [DataRow("8/7P/8/6K1/8/k7/1p6/3r4 w - - 0 1", "H7", "H8", "7R/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "R", DisplayName = "Pawn promotion to rook")]
        [DataRow("7Q/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "B2", "B1", "7Q/8/8/6K1/8/k7/8/1r1r4 w - - 0 2", "R", DisplayName = "Pawn promotion to rook")]
        [DataRow("8/7P/8/6K1/8/k7/1p6/3r4 w - - 0 1", "H7", "H8", "7B/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "B", DisplayName = "Pawn promotion to bishop")]
        [DataRow("7Q/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "B2", "B1", "7Q/8/8/6K1/8/k7/8/1b1r4 w - - 0 2", "B", DisplayName = "Pawn promotion to bishop")]
        [DataRow("8/7P/8/6K1/8/k7/1p6/3r4 w - - 0 1", "H7", "H8", "7N/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "N", DisplayName = "Pawn promotion to knight")]
        [DataRow("7Q/8/8/6K1/8/k7/1p6/3r4 b - - 0 1", "B2", "B1", "7Q/8/8/6K1/8/k7/8/1n1r4 w - - 0 2", "N", DisplayName = "Pawn promotion to knight")]
        [TestMethod]
        public void Play_PawnPromotion_ShouldBeHandledCorrectly(string fen, string origin, string destination, string expectedFen, string promoting)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Guid currentPlayer = match.CurrentPlayerInfo!.Value.Id;
            Move move = new(currentPlayer, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination), promoting);

            match.Play(move);

            Assert.AreNotEqual(match.CurrentPlayerInfo, move.PlayerId);
            Assert.AreEqual(match.FenString, expectedFen);
            Assert.IsTrue(match.Moves.Any());
        }

        [DataRow("r3k2r/1pp1qpp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP1QPP1/R3K2R w - - 0 1", "E1", "G1", DisplayName = "Castling when its not available")]
        [DataRow("r3k2r/1pp1qpp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP1QPP1/R3K2R w - - 0 1", "E1", "C1", DisplayName = "Castling when its not available")]
        [DataRow("r3k2r/1pp1qpp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP2PP1/R2QK2R b - - 0 1", "E8", "G8", DisplayName = "Castling when its not available")]
        [DataRow("r3k2r/1pp1qpp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP2PP1/R2QK2R b - - 0 1", "E8", "C8", DisplayName = "Castling when its not available")]
        [DataRow("rnbqkb1r/p1pp1ppp/4pn2/8/1pPP4/6P1/PP2PP1P/RNBQKBNR b KQkq - 0 1", "B4", "C3", DisplayName = "Capturing by en passant when its not available")]
        [DataRow("rnbqkb1r/p1pp1pp1/4pn1p/1pP5/3P4/6PP/PP2PP2/RNBQKBNR b KQkq - 0 1", "C5", "B6", DisplayName = "Capturing by en passant when its not available")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR b KQkq - 0 1", "B5", "B4", DisplayName = "Capturing piece in front of pawn")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR w KQkq - 0 1", "B4", "B5", DisplayName = "Capturing piece in front of pawn")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR w KQkq - 0 1", "D4", "E5", DisplayName = "Moving pawn to empty diagonal")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR b KQkq - 0 1", "H6", "G5", DisplayName = "Moving pawn to empty diagonal")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR w KQkq - 0 1", "D4", "C5", DisplayName = "Capturing ally piece")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR w KQkq - 0 1", "D1", "D4", DisplayName = "Capturing ally piece")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR b KQkq - 0 1", "F6", "E8", DisplayName = "Capturing ally piece")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR w KQkq - 0 1", "D1", "E1", DisplayName = "Capturing the king")]
        [DataRow("rnbqkb1r/p2p1pp1/2p1pn1p/1pP5/1P1P4/6PP/P3PP2/RNBQKBNR b KQkq - 0 1", "D8", "E8", DisplayName = "Capturing the king")]
        [DataRow("8/1K4kP/8/4q3/8/8/8/7R b - - 0 1", "G7", "H7", DisplayName = "Capturing with the king a protected piece")]
        [DataRow("8/1K4kP/2r5/3q4/8/8/8/7R w - - 0 1", "B7", "C6", DisplayName = "Capturing with the king a protected piece")]
        [DataRow("5k2/8/3K4/3Q1p2/5P2/8/8/3r4 w - - 0 1", "D5", "F5", DisplayName = "Moving a pinned piece and exposing king to a check")]
        [DataRow("5k2/8/3K4/3Q1p2/5P2/8/8/3r4 w - - 0 1", "D5", "A2", DisplayName = "Moving a pinned piece and exposing king to a check")]
        [DataRow("8/5k2/3K4/1p6/8/2Q1rP2/8/8 w - - 0 1", "F3", "F6", DisplayName = "Moving pawn more than one square")]
        [DataRow("8/5k2/3K4/1p6/8/2Q1rP2/8/8 w - - 0 1", "F3", "H5", DisplayName = "Moving pawn more than one square")]
        [DataRow("8/5k2/3K4/1p6/8/2Q1rP2/8/8 w - - 0 1", "F3", "B7", DisplayName = "Moving pawn more than one square")]
        [DataRow("8/5k2/3K4/1p6/8/2Q1rP2/8/8 w - - 0 1", "F3", "F2", DisplayName = "Moving pawn to invalid direction")]
        [DataRow("8/5k2/3K4/1p6/8/2Q1rP2/8/8 b - - 0 1", "B5", "B6", DisplayName = "Moving pawn to invalid direction")]
        [DataRow("8/5k2/Q2K4/1p6/8/5P2/6r1/8 w - - 0 1", "F3", "G2", DisplayName = "Moving pawn to invalid direction")]
        [DataRow("8/5k2/Q2K4/1p6/8/5P2/6r1/8 b - - 0 1", "B5", "A6", DisplayName = "Moving pawn to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/5r2/8/8 b - - 0 1", "F3", "G4", DisplayName = "Moving rook to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/5r2/8/8 b - - 0 1", "F3", "G2", DisplayName = "Moving rook to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/5r2/8/8 b - - 0 1", "F3", "E2", DisplayName = "Moving rook to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/5r2/8/8 b - - 0 1", "F3", "E4", DisplayName = "Moving rook to invalid direction")]
        [DataRow("r1bq1rk1/2p1bppp/p1n2n2/1p1pp1B1/3PP3/1BP2N2/PP3PPP/RN1QR1K1 w - - 0 1", "G5", "G6", DisplayName = "Moving bishop to invalid direction")]
        [DataRow("r1bq1rk1/2p1bppp/p1n2n2/1p1pp1B1/3PP3/1BP2N2/PP3PPP/RN1QR1K1 w - - 0 1", "G5", "G4", DisplayName = "Moving bishop to invalid direction")]
        [DataRow("r1bq1rk1/2p1bppp/p1n2n2/1p1pp1B1/3PP3/1BP2N2/PP3PPP/RN1QR1K1 w - - 0 1", "G5", "F5", DisplayName = "Moving bishop to invalid direction")]
        [DataRow("r1bq1rk1/2p1bppp/p1n2n2/1p1pp1B1/3PP3/1BP2N2/PP3PPP/RN1QR1K1 w - - 0 1", "G5", "H5", DisplayName = "Moving bishop to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/2Q2r2/8/8 w - - 0 1", "C3", "D5", DisplayName = "Moving queen to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/2Q2r2/8/8 w - - 0 1", "C3", "E2", DisplayName = "Moving queen to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/2Q2r2/8/8 w - - 0 1", "C3", "A2", DisplayName = "Moving queen to invalid direction")]
        [DataRow("8/2P2k2/3K4/8/8/2Q2r2/8/8 w - - 0 1", "C3", "B5", DisplayName = "Moving queen to invalid direction")]
        [DataRow("6k1/5rp1/6p1/8/8/8/B5Q1/4K1R1 w - - 0 1", "G2", "G7", DisplayName = "Queen Move with capture")]
        [DataRow("8/5k2/Q2K4/1p6/8/5P2/6r1/8 b - - 0 1", "F7", "F4", DisplayName = "Moving king more than one square")]
        [DataRow("8/5k2/Q2K4/1p6/8/5P2/6r1/8 b - - 0 1", "F7", "H7", DisplayName = "Moving king more than one square")]
        [DataRow("8/5k2/Q2K4/1p6/8/5P2/6r1/8 w - - 0 1", "D6", "H6", DisplayName = "Moving king more than one square")]
        [DataRow("8/5k2/Q2K4/1p6/8/5P2/6r1/8 w - - 0 1", "D6", "D1", DisplayName = "Moving king more than one square")]
        [TestMethod]
        public void Play_InvalidMove_ShouldThrowException(string fen, string origin, string destination)
        {
            Assert.ThrowsException<ChessboardException>(() => FakeMatch.RestoreAndPlay(fen, origin, destination));
        }

        [TestMethod]
        public void Play_MatchNotStarted_ShouldThrowException()
        {
            Match match = new(Time.Three);
            PlayerInfo player = match.CreateNewPlayer(Color.White);
            match.Join(player);
            Move move = new(player.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerNotInMatch_ShouldThrowException()
        {
            Match match = FakeMatch.RestoreMatch(FenInfo.InitialPosition);
            Move move = new(Guid.NewGuid(), Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerMakingMoveWhenItsNotTheTurn_ShouldThrowException()
        {
            Match match = FakeMatch.RestoreMatch(FenInfo.InitialPosition);

            Move move = new(match.OpponentPlayerInfo!.Value.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_EmptyOrigin_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = match.CreateNewPlayer(Color.White);
            PlayerInfo player2 = match.CreateNewPlayer(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("E6"));
            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerTryingToMoveOpponentPiece_ShouldThrowException()
        {
            Match match = FakeMatch.RestoreMatch(FenInfo.InitialPosition);

            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance("E7"), Coordinate.GetInstance("E5"));
            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [DataRow("8/8/1kr1QK2/7p/7P/8/8/2R5 b - - 0 1", "C6", "C1")]
        [DataRow("8/8/3Q1K2/1k5p/7P/8/8/3r4 b - - 0 1", "B5", "C5")]
        [DataRow("8/8/5K2/8/1k1pP2Q/8/8/8 b - E3 0 1", "D4", "E3")]
        [TestMethod]
        public void Play_PlayerInSelfCheckAfterMove_ShouldThrowException(string fen, string origin, string destination)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Guid currentPlayer = match.CurrentPlayerInfo!.Value.Id;
            Move move = new(currentPlayer, Coordinate.GetInstance(origin), Coordinate.GetInstance(destination));

            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
            Assert.AreEqual(match.FenString, fen);
        }

        [TestMethod]
        public void Play_ShouldAddPGNMove()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = match.CreateNewPlayer(Color.White);
            PlayerInfo player2 = match.CreateNewPlayer(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            match.Play(move);

            Assert.IsTrue(match.Moves.Any());
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1")]
        [DataRow("rnbqkb1r/pppppp1p/5np1/8/2PP4/8/PP2PPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("8/5P2/3K4/8/8/1k6/8/q7 w - - 0 1")]
        [DataRow("3k4/1K6/2PbP3/3B4/8/8/8/8 w - - 0 1")]
        [DataRow("r1bqk2r/p2p1ppp/1pnbpn2/2p5/2PP4/3BPN1P/PP3PP1/RNBQ1RK1 b kq - 0 1")]
        [TestMethod]
        public void FenString_ShouldReturnCorrectFenString(string fen)
        {
            Match match = FakeMatch.RestoreMatch(fen);
            Assert.AreEqual(match.FenString, fen);
        }

        [TestMethod]
        public void Play_ShouldSwitchTurns()
        {
            Match match = FakeMatch.RestoreMatch(FenInfo.InitialPosition);

            Assert.AreEqual(Color.White, match.CurrentPlayerColor);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            match.Play(move);
            Assert.AreEqual(Color.Black, match.CurrentPlayerColor);
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", "D6", "B6")]
        [TestMethod]
        public void Play_InvalidMove_ShouldThrowExceptionAndRestoreChessboardToLastPosition(string position, string orig, string dest)
        {
            Coordinate origin = Coordinate.GetInstance(orig);
            Coordinate destination = Coordinate.GetInstance(dest);
            Match match = FakeMatch.RestoreMatch(position);

            string currentPosition = match.FenString;

            Move move = new(match.CurrentPlayerInfo!.Value.Id, origin, destination);
            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
            Assert.AreEqual(currentPosition, match.FenString);
        }

        [TestMethod]
        public void Play_NonCaptureOrNonPawnMove_ShouldIncrementHalfMoveCounter()
        {
            Match match = FakeMatch.RestoreMatch(FenInfo.InitialPosition);

            Assert.AreEqual(0, match.HalfMove);
            Move move = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance("G1"), Coordinate.GetInstance("F3"));
            match.Play(move);
            Assert.AreEqual(1, match.HalfMove);
            Move move2 = new(match.CurrentPlayerInfo!.Value.Id, Coordinate.GetInstance("B8"), Coordinate.GetInstance("C6"));
            match.Play(move2);
            Assert.AreEqual(2, match.HalfMove);
        }

        [DataRow("r1bqkbnr/1ppp1ppp/p7/4p3/4P3/5N2/PPPP1PPP/RNBQK2R b KQkq - 8 1", "D7", "D6")]
        [DataRow("r1bqkbnr/1ppp1ppp/p1B5/4p3/4P3/N4N2/PPPP1PPP/R1BQK2R b KQkq - 8 1", "F8", "A3")]
        [TestMethod]
        public void Play_CaptureOrPawnMove_ShouldResetHalfMoveCounter(string position, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(position, origin, destination);
            Assert.AreEqual(0, match.HalfMove);
        }

        [DataRow("r1bqkbnr/1ppp1ppp/p7/4p3/4P3/5N2/PPPP1PPP/RNBQK2R b KQkq - 8 4", "D7", "D6")]
        [DataRow("r1bqkbnr/1ppp1ppp/p1B5/4p3/4P3/N4N2/PPPP1PPP/R1BQK2R b KQkq - 8 4", "F8", "A3")]
        [TestMethod]
        public void Play_BlackMoves_ShouldIncrementFullMoveCounter(string position, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(position, origin, destination);
            Assert.AreEqual(5, match.FullMove);
        }

        [DataRow("r1bqkbnr/1ppp1ppp/p1B5/4p3/4P3/N4N2/PPPP1PPP/R1BQK2R w KQkq - 0 1", "F3", "E5")]
        [TestMethod]
        public void Play_WhiteMoves_ShouldNotIncrementFullMoveCounter(string position, string origin, string destination)
        {
            Match match = FakeMatch.RestoreAndPlay(position, origin, destination);
            Assert.AreEqual(1, match.FullMove);
        }

        [TestMethod]
        public void Play_PlayerWithoutTimeEnough_ShouldFinishTheMatch()
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(FenInfo.InitialPosition, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), TimeSpan.Zero.Ticks, TimeSpan.FromMinutes(4).Ticks, DateTime.UtcNow.ToString(), 5);
            Match match = new(matchInfo);
            Assert.IsFalse(match.HasFinished());
            Assert.AreEqual(match.CurrentPlayerInfo.Value.Color, Color.White);

            Move move = new(match.CurrentPlayerInfo.Value.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            match.Play(move);

            Assert.IsTrue(match.HasFinished());
            Assert.AreEqual(match.Winner.Value, match.Players.Where(p => p.Color == Color.Black).First().Id);
            Assert.AreEqual(match.CurrentPositionStatus, CurrentPositionStatus.Timeout);
        }

        [TestMethod]
        public void Play_ShouldDecreaseTheTimeOfCurrentPlayer()
        {
            Match match = FakeMatch.RestoreMatch(FenInfo.InitialPosition);
            TimeSpan expectedTime = TimeSpan.FromMinutes((int)match.Duration) - TimeSpan.FromMilliseconds(100);
            Move move = new(match.CurrentPlayerInfo.Value.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Thread.Sleep(110);

            match.Play(move);

            Assert.IsTrue(match.OpponentPlayerInfo.Value.TimeRemaining <= expectedTime);
            Assert.AreEqual(TimeSpan.FromMinutes((int)match.Duration), match.CurrentPlayerInfo.Value.TimeRemaining);
        }


        [TestMethod]
        public void Play_ShouldNotDecreaseTheTimeOfOpponentPlayer()
        {
            Match match = FakeMatch.RestoreMatch(FenInfo.InitialPosition);
            long opponentPlayerTime = match.OpponentPlayerInfo.Value.TimeRemaining.Ticks;

            Move move = new(match.CurrentPlayerInfo.Value.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            match.Play(move);

            Assert.AreEqual(opponentPlayerTime, match.CurrentPlayerInfo.Value.TimeRemaining.Ticks);
        }

        [TestMethod]
        public void Play_TimeRemaining_ShouldNotBeGreaterThanBefore()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White, Time.Ten);
            PlayerInfo player2 = new(Color.Black, Time.Ten);
            match.Join(player1);
            match.Join(player2);

            List<Move> moves = new()
            {
                new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4")),
                new(player2.Id, Coordinate.GetInstance("D7"), Coordinate.GetInstance("D5")),
                new(player1.Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("D5")),
                new(player2.Id, Coordinate.GetInstance("D8"), Coordinate.GetInstance("D5")),
                new(player1.Id, Coordinate.GetInstance("F1"), Coordinate.GetInstance("C4")),
                new(player2.Id, Coordinate.GetInstance("D5"), Coordinate.GetInstance("C4")),
                new(player1.Id, Coordinate.GetInstance("D2"), Coordinate.GetInstance("D3")),
                new(player2.Id, Coordinate.GetInstance("C4"), Coordinate.GetInstance("D3")),
                new(player1.Id, Coordinate.GetInstance("C2"), Coordinate.GetInstance("D3")),
            };

            foreach (Move move in moves)
            {
                var currentPlayerTimeRemainingBeforePlayingMove = match.CurrentPlayerInfo.Value.TimeRemaining;
                var opponentPlayerTimeRemainingBeforePlayingMove = match.OpponentPlayerInfo.Value.TimeRemaining;
                Thread.Sleep(100);
                match.Play(move);
                Thread.Sleep(100);
                var currentPlayerTimeRemainingAfterPlayingMove = match.CurrentPlayerInfo.Value.TimeRemaining;
                var opponentPlayerTimeRemainingAfterPlayingMove = match.OpponentPlayerInfo.Value.TimeRemaining;

                Assert.IsTrue(opponentPlayerTimeRemainingAfterPlayingMove <= opponentPlayerTimeRemainingBeforePlayingMove);
                Assert.IsTrue(currentPlayerTimeRemainingAfterPlayingMove <= currentPlayerTimeRemainingBeforePlayingMove);
            }
        }
    }
}