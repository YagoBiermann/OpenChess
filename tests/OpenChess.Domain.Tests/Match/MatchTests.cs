using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class MatchTests
    {
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
            Assert.AreEqual(time, (int)match.Time);
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
            Assert.AreEqual(whitePlayer.Id, match.CurrentPlayer);
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
            PlayerInfo player = new(Guid.NewGuid(), Color.White, match.Id);
            match.Join(player);
            Match match2 = new(Time.Ten);

            Assert.ThrowsException<MatchException>(() => match2.Join(player));
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
            Move move = new(match.CurrentPlayer!.Value, Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));

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
                    Assert.ThrowsException<MatchException>(() => match.Play(move));
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
        public void Play_PgnMoveList_ShouldBeInRightPgnFormat()
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
                match.Play(move);
            }

            List<string> expectedMoveList = new()
            {
            "1. e4",
            "2. d5",
            "3. exd5",
            "4. Qxd5",
            "5. Bc4",
            "6. Qxc4",
            "7. d3",
            "8. Qxd3",
            "9. cxd3",
            };

            CollectionAssert.AreEqual(expectedMoveList, match.Moves);
        }
    }
}