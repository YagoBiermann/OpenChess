using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KnightTests
    {
        [TestMethod]
        public void NameProperty_BlackKnight_ShouldBeLowercaseN()
        {
            Knight knight = new(Color.Black, Coordinate.GetInstance("B8"));

            Assert.AreEqual(knight.Name, 'n');
        }
        [TestMethod]
        public void NameProperty_WhiteKnight_ShouldBeUppercaseN()
        {
            Knight knight = new(Color.White, Coordinate.GetInstance("B1"));

            Assert.AreEqual(knight.Name, 'N');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            Knight knight = new(Color.White, Coordinate.GetInstance("B1"));
            Knight knight2 = new(Color.Black, Coordinate.GetInstance("B8"));

            Assert.IsFalse(knight.IsLongRange);
            Assert.IsFalse(knight2.IsLongRange);
        }
    }
}