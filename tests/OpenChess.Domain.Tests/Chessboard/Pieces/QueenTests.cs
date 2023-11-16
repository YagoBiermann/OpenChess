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

    }
}