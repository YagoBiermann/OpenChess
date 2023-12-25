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
            Assert.IsNull(match.CurrentPlayer);
            Assert.IsNull(match.Winner);
            Assert.AreEqual(match.Chessboard, FenInfo.InitialPosition);
            Assert.AreEqual(match.CurrentPlayerCheckState, CheckState.NotInCheck);
            Assert.AreEqual(time, (int)match.Time);
        }

        [TestMethod]
        public void NewInstance_ShouldRestoreGameStateCorrectly()
        {
            MatchInfo matchInfo = FakeMatch.RestoreMatch(_fen, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
            Match match = new(matchInfo);

            Assert.AreEqual(match.Id, matchInfo.MatchId);
            Assert.AreEqual(match.Time, Time.Five);
            Assert.AreEqual(match.Status, MatchStatus.InProgress);
            Assert.AreEqual(match.CurrentPlayerCheckState, CheckState.NotInCheck);
            Assert.AreEqual(match.Chessboard, matchInfo.Fen);
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
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, player2, 6));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, player2, 5, "imProges"));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, "invalidId"));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, "invalidId", player2));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, "invalidId", player1, player2));
            Assert.ThrowsException<MatchException>(() => FakeMatch.RestoreMatch(_fen, matchId, player1, player2, 5, "InProgress", "invalidId"));
        }

        [TestMethod]
        public void Join_MatchNotFull_ShouldAssignPlayerToMatch()
        {
            Match match = new(Time.Ten);
            PlayerInfo player = new(Color.White);

            match.Join(player);

            Assert.IsTrue(match.HasPlayer());
            Assert.AreEqual(MatchStatus.NotStarted, match.Status);
            Assert.IsFalse(match.IsFull());
            Assert.IsNull(match.CurrentPlayer);
        }

        [TestMethod]
        public void Join_WhenAllPlayersJoined_ShouldChangeStatusAndCurrentPlayer()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);
            PlayerInfo blackPlayer = new(Color.Black);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.AreEqual(MatchStatus.InProgress, match.Status);
            Assert.AreEqual(whitePlayer.Id, match.CurrentPlayer!.Value.Id);
        }

        [TestMethod]
        public void Join_AddingSamePlayerTwice_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player = new(Color.White);
            PlayerInfo fakePlayer = new(player.Id, Color.Black);

            match.Join(player);

            Assert.ThrowsException<MatchException>(() => match.Join(player));
            Assert.ThrowsException<MatchException>(() => match.Join(fakePlayer));
        }

        [TestMethod]
        public void Join_AddingPlayerThatIsAlreadyInAnotherMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player = new(Guid.NewGuid(), Color.White, null);
            match.Join(player);
            Match match2 = new(Time.Ten);

            Assert.ThrowsException<MatchException>(() => match2.Join(match.Players.FirstOrDefault()));
        }

        [TestMethod]
        public void Join_FullMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);
            PlayerInfo blackPlayer = new(Color.Black);

            PlayerInfo otherPlayer = new(Color.White);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.ThrowsException<MatchException>(() => match.Join(otherPlayer));
        }

        [TestMethod]
        public void IsFull_ShouldReturnTrue()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);
            PlayerInfo blackPlayer = new(Color.Black);

            match.Join(whitePlayer);
            match.Join(blackPlayer);

            Assert.IsTrue(match.IsFull());
        }

        [TestMethod]
        public void IsFull_ShouldReturnFalse()
        {
            Match match = new(Time.Ten);
            PlayerInfo whitePlayer = new(Color.White);

            match.Join(whitePlayer);

            Assert.IsFalse(match.IsFull());
        }

        [TestMethod]
        public void Play_ValidMove_ShouldBeHandled()
        {
            Match match = new(Time.Ten);
            match.Join(new(Color.White));
            match.Join(new(Color.Black));
            string initialPosition = match.Chessboard;
            Move move = new(match.CurrentPlayer!.Value.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));

            match.Play(move);

            Assert.AreNotEqual(match.Chessboard, initialPosition);
            Assert.AreNotEqual(match.CurrentPlayer, move.PlayerId);
        }

        [TestMethod]
        public void Play_MatchNotStarted_ShouldThrowException()
        {
            Match match = new(Time.Three);
            PlayerInfo player = new(Color.White);
            match.Join(player);
            Move move = new(player.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerNotInMatch_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(Guid.NewGuid(), Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerMakingMoveWhenItsNotTheTurn_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player2.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.ThrowsException<MatchException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_EmptyOrigin_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("E6"));
            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerTryingToMoveOpponentPiece_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E7"), Coordinate.GetInstance("E5"));
            Assert.ThrowsException<ChessboardException>(() => match.Play(move));
        }

        [TestMethod]
        public void Play_PlayerInSelfCheckAfterMove_ShouldThrowException()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            List<Move> moves = new()
            {
                new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4")),
                new(player2.Id, Coordinate.GetInstance("E7"), Coordinate.GetInstance("E5")),
                new(player1.Id, Coordinate.GetInstance("D1"), Coordinate.GetInstance("H5")),
                new(player2.Id, Coordinate.GetInstance("F7"), Coordinate.GetInstance("F5")),
            };
            string lastPosition = match.Chessboard;

            foreach (Move move in moves)
            {
                bool isLastMove = moves.Last() == move;
                if (isLastMove)
                {
                    Assert.ThrowsException<ChessboardException>(() => match.Play(move));
                }
                else
                {
                    match.Play(move);
                }
            }
            string currentPosition = match.Chessboard;

            Assert.AreEqual(lastPosition, currentPosition);
        }

        [TestMethod]
        public void Play_ShouldAddPGNMove()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            Move move = new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            match.Play(move);

            Assert.IsTrue(match.Moves.Any());
        }

        [TestMethod]
        public void Play_ShouldHandleEnPassantMoves()
        {
            Match match = new(Time.Ten);
            PlayerInfo player1 = new(Color.White);
            PlayerInfo player2 = new(Color.Black);
            match.Join(player1);
            match.Join(player2);

            List<Move> moves = new()
            {
                new(player1.Id, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4")),
                new(player2.Id, Coordinate.GetInstance("D7"), Coordinate.GetInstance("D5")),
                new(player1.Id, Coordinate.GetInstance("E4"), Coordinate.GetInstance("E5")),
                new(player2.Id, Coordinate.GetInstance("F7"), Coordinate.GetInstance("F5")),
                new(player1.Id, Coordinate.GetInstance("E5"), Coordinate.GetInstance("F6")),
            };
            foreach (Move move in moves)
            {
                match.Play(move);
            }
        }
    }
}