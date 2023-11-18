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

        [TestMethod]
        public void Create_UpperChar_ShouldCreateWhitePiece()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");

            Piece king = Piece.Create('K', coordinate);
            Piece queen = Piece.Create('Q', coordinate);
            Piece rook = Piece.Create('R', coordinate);
            Piece bishop = Piece.Create('B', coordinate);
            Piece knight = Piece.Create('N', coordinate);
            Piece pawn = Piece.Create('P', coordinate);

            Assert.IsInstanceOfType(king, typeof(King));
            Assert.IsInstanceOfType(queen, typeof(Queen));
            Assert.IsInstanceOfType(rook, typeof(Rook));
            Assert.IsInstanceOfType(bishop, typeof(Bishop));
            Assert.IsInstanceOfType(knight, typeof(Knight));
            Assert.IsInstanceOfType(pawn, typeof(Pawn));

            Assert.AreEqual(king.Color, Color.White);
            Assert.AreEqual(queen.Color, Color.White);
            Assert.AreEqual(rook.Color, Color.White);
            Assert.AreEqual(bishop.Color, Color.White);
            Assert.AreEqual(knight.Color, Color.White);
            Assert.AreEqual(pawn.Color, Color.White);
        }
    }
}