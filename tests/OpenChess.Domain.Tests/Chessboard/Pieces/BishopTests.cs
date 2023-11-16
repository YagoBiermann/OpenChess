using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class BishopTests
    {
        [TestMethod]
        public void NameProperty_BlackBishop_ShouldBeLowercaseB()
        {
            Bishop bishop = new(Color.Black, Coordinate.GetInstance("C8"));

            Assert.AreEqual(bishop.Name, 'b');
        }
        [TestMethod]
        public void NameProperty_WhiteBishop_ShouldBeUppercaseB()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("C1"));

            Assert.AreEqual(bishop.Name, 'B');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("C1"));
            Bishop bishop2 = new(Color.Black, Coordinate.GetInstance("C1"));

            Assert.IsTrue(bishop.IsLongRange);
            Assert.IsTrue(bishop2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnDiagonalDirections()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("C1"));
            Bishop bishop2 = new(Color.Black, Coordinate.GetInstance("C1"));

            List<Direction> directions = new()
            {
                new UpperLeft(),
                new UpperRight(),
                new LowerLeft(),
                new LowerRight()
            };

            CollectionAssert.AreEquivalent(directions, bishop.Directions);
            CollectionAssert.AreEquivalent(directions, bishop2.Directions);
        }
    }
}