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

        [TestMethod]
        public void Down_X_Returns_0()
        {
            Down down = new();
            Assert.AreEqual(down.X, 0);

        }
        [TestMethod]
        public void Down_Y_Returns_negative1()
        {
            Down down = new();
            Assert.AreEqual(down.Y, -1);
        }

        [TestMethod]
        public void Left_X_Returns_negative1()
        {
            Left left = new();
            Assert.AreEqual(left.X, -1);

        }
        [TestMethod]
        public void Left_Y_Returns_0()
        {
            Left left = new();
            Assert.AreEqual(left.Y, 0);
        }

    }
}