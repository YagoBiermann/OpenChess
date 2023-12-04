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
    }
}