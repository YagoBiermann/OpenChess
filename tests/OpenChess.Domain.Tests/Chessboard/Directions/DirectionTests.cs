using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    class DirectionTests
    {
        [TestMethod]
        public void Up_X_Returns_0()
        {
            Up up = new();
            Assert.AreEqual(up.X, 0);

        }

        [TestMethod]
        public void Up_Y_Returns_1()
        {
            Up up = new();
            Assert.AreEqual(up.Y, 1);
        }
    }
}