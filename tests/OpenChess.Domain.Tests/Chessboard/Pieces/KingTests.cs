using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        public void NameProperty_BlackKing_ShouldBeLowercaseK()
        {
            King king = new(Color.Black, Coordinate.GetInstance("E8"));

            Assert.AreEqual(king.Name, 'k');
        }
        [TestMethod]
        public void NameProperty_WhiteKing_ShouldBeUppercaseK()
        {
            King king = new(Color.White, Coordinate.GetInstance("E1"));

            Assert.AreEqual(king.Name, 'K');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            King king = new(Color.White, Coordinate.GetInstance("E1"));
            King king2 = new(Color.Black, Coordinate.GetInstance("E8"));

            Assert.IsFalse(king.IsLongRange);
            Assert.IsFalse(king2.IsLongRange);
        }
    }
}