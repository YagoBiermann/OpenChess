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
    }
}