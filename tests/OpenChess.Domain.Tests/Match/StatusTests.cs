using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class StatusTests
    {
        [DataRow("012", 0, 1, 2)]
        [DataRow("111", 1, 1, 1)]
        [DataRow("222", 2, 2, 2)]
        [DataRow("333", 3, 3, 3)]
        [DataRow("123", 1, 2, 3)]
        [TestMethod]
        public void Status_ExplicitCast_ShouldReturnValidStatus_WhenInputIsValid(string value, int gameStatus, int gameResult, int checkStatus)
        {
            var status = (Status)value;

            Assert.AreEqual((GameStatus)gameStatus, status.GameStatus);
            Assert.AreEqual((GameResult)gameResult, status.GameResult);
            Assert.AreEqual((CheckStatus)checkStatus, status.CheckStatus);
        }

        [TestMethod]
        [ExpectedException(typeof(MatchException), "Couldn't parse the string to Status")]
        public void Status_ExplicitCast_ShouldThrowException_WhenInputIsTooLong()
        {
            string input = "01234";
            var status = (Status)input;
        }

        [TestMethod]
        [ExpectedException(typeof(MatchException), "Couldn't parse the string to Status")]
        public void Status_ExplicitCast_ShouldThrowException_WhenInputIsInvalidFormat()
        {
            string input = "XYZ";  // Invalid characters that won't match enum values
            var status = (Status)input;
        }

        [TestMethod]
        [ExpectedException(typeof(MatchException), "Couldn't parse the string to Status")]
        public void Status_ExplicitCast_ShouldThrowException_WhenInputIsNullOrEmpty()
        {
            string input = "";
            var status = (Status)input;
        }
    }
}