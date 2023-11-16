using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class QueenTests
    {
        [TestMethod]
        public void NameProperty_BlackQueen_ShouldBeLowercaseQ()
        {
            Queen queen = new(Color.Black, Coordinate.GetInstance("D8"));

            Assert.AreEqual(queen.Name, 'q');
        }

        [TestMethod]
        public void NameProperty_WhiteQueen_ShouldBeUppercaseQ()
        {
            Queen queen = new(Color.White, Coordinate.GetInstance("D1"));

            Assert.AreEqual(queen.Name, 'Q');
        }

        [TestMethod]
        public void IsLongeRangeProperty_ShouldBeTrue()
        {
            Queen queen = new(Color.White, Coordinate.GetInstance("D1"));
            Queen queen2 = new(Color.Black, Coordinate.GetInstance("D1"));

            Assert.IsTrue(queen.IsLongRange);
            Assert.IsTrue(queen2.IsLongRange);
        }

        [TestMethod]
        public void Directions_ShouldReturnAllDirections()
        {
            Queen queen = new(Color.White, Coordinate.GetInstance("D1"));
            Queen queen2 = new(Color.Black, Coordinate.GetInstance("D1"));

            List<Direction> directions = new()
            {
                new Up(),
                new Down(),
                new Left(),
                new Right(),
                new UpperLeft(),
                new UpperRight(),
                new LowerLeft(),
                new LowerRight()
            };

            CollectionAssert.AreEquivalent(directions, queen.Directions);
            CollectionAssert.AreEquivalent(directions, queen2.Directions);
        }
    }
}