using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class RookTests
    {
        [TestMethod]
        public void NameProperty_BlackRook_ShouldBeLowercaseR()
        {
            Rook rook = new(Color.Black, Coordinate.GetInstance("A1"));

            Assert.AreEqual(rook.Name, 'r');
        }

        [TestMethod]
        public void NameProperty_WhiteRook_ShouldBeUppercaseR()
        {
            Rook rook = new(Color.White, Coordinate.GetInstance("A1"));

            Assert.AreEqual(rook.Name, 'R');
        }
    }
}