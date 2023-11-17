using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PieceTests
    {
        [TestMethod]
        public void Equals_SameObject_ShouldReturnTrue()
        {
            Pawn pawn = new(Color.Black, Coordinate.GetInstance("A1"));
            Pawn pawn2 = new(Color.Black, Coordinate.GetInstance("A1"));

            Assert.IsTrue(pawn.Equals(pawn2));
        }
    }
}