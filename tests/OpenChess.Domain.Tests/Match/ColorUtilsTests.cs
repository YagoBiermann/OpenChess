using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class ColorUtilsTests
    {
        [TestMethod]
        public void GetRandomColor_ShouldReturnBothColorsAfterMultipleCalls()
        {
            bool foundBlack = false;
            bool foundWhite = false;

            for (int i = 0; i < 100; i++)
            {
                Color result = ColorUtils.GetRandomColor();
                if (result == Color.Black) foundBlack = true;
                if (result == Color.White) foundWhite = true;

                if (foundBlack && foundWhite) break;
            }

            Assert.IsTrue(foundBlack, "Black was never returned after multiple calls.");
            Assert.IsTrue(foundWhite, "White was never returned after multiple calls.");
        }

        [TestMethod]
        public void GetRandomColor_ShouldReturnEitherBlackOrWhite()
        {
            Color result = ColorUtils.GetRandomColor();
            Assert.IsTrue(result == Color.Black || result == Color.White, $"The returned color was {result}, but it should be either Black or White.");
        }

        [TestMethod]
        public void GetOppositeColor_ShouldReturnBlack_WhenWhiteIsPassed()
        {
            Color input = Color.White;
            Color result = ColorUtils.GetOppositeColor(input);
            Assert.AreEqual(Color.Black, result, "The opposite color of White should be Black.");
        }

        [TestMethod]
        public void GetOppositeColor_ShouldReturnWhite_WhenBlackIsPassed()
        {
            Color input = Color.Black;
            Color result = ColorUtils.GetOppositeColor(input);
            Assert.AreEqual(Color.White, result, "The opposite color of Black should be White.");
        }
    }
}