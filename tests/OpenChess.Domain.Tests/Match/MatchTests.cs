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
        }

        [TestMethod]
        public void Join_MatchNotFull_ShouldAssignPlayerToMatch()
        {
            Match match = new(Time.Ten);
            Player player = new(Color.White);

            match.Join(player);

            Assert.IsNotNull(match.GetPlayerBy(Color.White));
            Assert.IsNull(match.GetPlayerBy(Color.Black));
        }
    }
}