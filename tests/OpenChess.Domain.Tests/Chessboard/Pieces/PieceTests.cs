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

        [TestMethod]
        public void Equals_DifferentPieces_ShouldReturnFalse()
        {
            Rook rook = new(Color.Black, Coordinate.GetInstance("B2"));
            Queen queen = new(Color.White, Coordinate.GetInstance("F4"));

            Assert.IsFalse(rook.Equals(queen));
        }

        [TestMethod]
        public void Equals_SamePieceDifferentColors_ShouldReturnFalse()
        {
            King king = new(Color.White, Coordinate.GetInstance("F4"));
            King king2 = new(Color.Black, Coordinate.GetInstance("F4"));

            Assert.IsFalse(king.Equals(king2));
        }

        [TestMethod]
        public void Equals_SamePieceDifferentOrigin_ShouldReturnFalse()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("A1"));
            Bishop bishop2 = new(Color.White, Coordinate.GetInstance("A2"));

            Assert.IsFalse(bishop.Equals(bishop2));
        }
    }
}