using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class TimeTests
    {
        [TestMethod]
        [DataRow(5)]
        [DataRow(10)]
        [DataRow(15)]
        [DataRow(30)]
        public void Constructor_ValidTime_SetsValue(int validTime)
        {
            Time time = new(validTime);
            Assert.AreEqual(validTime, time.Value);
        }

        [TestMethod]
        [DataRow(7)]
        [DataRow(20)]
        [DataRow(25)]
        [DataRow(24)]
        public void Constructor_InvalidTime_ThrowsMatchException(int invalidTime)
        {
            Assert.ThrowsException<MatchException>(() => new Time(invalidTime));
        }

        [TestMethod]
        [DataRow(3, true)]
        [DataRow(5, true)]
        [DataRow(30, true)]
        [DataRow(7, false)]
        [DataRow(20, false)]
        public void IsValid_Time_ReturnsCorrectResult(int timeValue, bool expectedResult)
        {
            bool result = Time.IsValid(timeValue);
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        [DataRow(15, 15)]
        [DataRow(5, 5)]
        [DataRow(30, 30)]
        public void ImplicitConversion_ToInt_ReturnsCorrectValue(int inputTime, int expectedValue)
        {
            Time time = new(inputTime);
            int value = time;
            Assert.AreEqual(expectedValue, value);
        }

        [TestMethod]
        [DataRow(30, "30")]
        [DataRow(5, "5")]
        public void ToString_ReturnsCorrectString(int inputTime, string expectedString)
        {
            Time time = new(inputTime);
            string result = time.ToString();
            Assert.AreEqual(expectedString, result);
        }
    }
}