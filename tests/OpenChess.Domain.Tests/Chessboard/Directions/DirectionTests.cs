using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class DirectionTests
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

        [TestMethod]
        public void Right_X_Returns_1()
        {
            Right right = new();
            Assert.AreEqual(right.X, 1);

        }
        [TestMethod]
        public void Right_Y_Returns_0()
        {
            Right right = new();
            Assert.AreEqual(right.Y, 0);
        }

        [TestMethod]
        public void UpperLeft_X_Returns_negative1()
        {
            UpperLeft upperLeft = new();
            Assert.AreEqual(upperLeft.X, -1);
        }
        [TestMethod]
        public void UpperLeft_Y_Returns_1()
        {
            UpperLeft upperLeft = new();
            Assert.AreEqual(upperLeft.Y, 1);
        }

        [TestMethod]
        public void UpperRight_X_Returns_1()
        {
            UpperRight upperRight = new();
            Assert.AreEqual(upperRight.X, 1);
        }
        [TestMethod]
        public void UpperRight_Y_Returns_1()
        {
            UpperRight upperRight = new();
            Assert.AreEqual(upperRight.Y, 1);
        }

        [TestMethod]
        public void LowerLeft_X_Returns_negative1()
        {
            LowerLeft lowerLeft = new();
            Assert.AreEqual(lowerLeft.X, -1);
        }
        [TestMethod]
        public void LowerLeft_Y_Returns_negative1()
        {
            LowerLeft lowerLeft = new();
            Assert.AreEqual(lowerLeft.Y, -1);
        }

        [TestMethod]
        public void LowerRight_X_Returns_1()
        {
            LowerRight lowerRight = new();
            Assert.AreEqual(lowerRight.X, 1);
        }
        [TestMethod]
        public void LowerRight_Y_Returns_negative1()
        {
            LowerRight lowerRight = new();
            Assert.AreEqual(lowerRight.Y, -1);
        }

    }
}