using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class ColorTests
    {
        [TestMethod]
        public void GetRandomColor_ShouldReturnBothColorsAfterMultipleCalls()
        {
            bool foundBlack = false;
            bool foundWhite = false;

            for (int i = 0; i < 100; i++)
            {
                Color result = Color.GetRandomColor();
                if (result.Value == Color.Black) foundBlack = true;
                if (result.Value == Color.White) foundWhite = true;

                if (foundBlack && foundWhite) break;
            }

            Assert.IsTrue(foundBlack, "Black was never returned after multiple calls.");
            Assert.IsTrue(foundWhite, "White was never returned after multiple calls.");
        }

        [TestMethod]
        public void GetRandomColor_ShouldReturnEitherBlackOrWhite()
        {
            Color result = Color.GetRandomColor();
            Assert.IsTrue(result.Value == Color.Black || result.Value == Color.White, $"The returned color was {result}, but it should be either Black or White.");
        }

        [TestMethod]
        public void GetOppositeColor_ShouldReturnBlack_WhenWhiteIsPassed()
        {
            Color input = new(Color.White);
            Color result = Color.GetOppositeColor(input);
            Assert.AreEqual(Color.Black, result.Value, "The opposite color of White should be Black.");
        }

        [TestMethod]
        public void GetOppositeColor_ShouldReturnWhite_WhenBlackIsPassed()
        {
            Color input = new(Color.Black);
            Color result = Color.GetOppositeColor(input);
            Assert.AreEqual(Color.White, result.Value, "The opposite color of Black should be White.");
        }
    }
}