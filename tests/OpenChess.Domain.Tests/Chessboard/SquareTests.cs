using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class SquareTests
    {
        [TestMethod]
        public void Getter_PieceNotNull_ShouldReturnPiece()
        {
            Coordinate origin = Coordinate.GetInstance("A1");
            Pawn pawn = new(Color.Black, origin);
            Square square = new(origin, pawn);

            Assert.IsNotNull(square.Piece);
            Assert.IsInstanceOfType(pawn, typeof(Pawn));
        }

        [TestMethod]
        public void Getter_PieceNull_ReturnsNull()
        {
            Coordinate origin = Coordinate.GetInstance("A1");
            Square square = new(origin, null);

            Assert.IsNull(square.Piece);
        }

        [TestMethod]
        public void Setter_SettingPieceAsNull_ShouldRemoveThePiece()
        {
            Coordinate origin = Coordinate.GetInstance("A1");
            Pawn pawn = new(Color.Black, origin);
            Square square = new(origin, pawn);

            square.Piece = null;

            Assert.IsNull(square.Piece);
        }

        [TestMethod]
        public void Setter_SettingPiece_ShouldReplaceTheCurrentPiece()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A4");
            Pawn pawn = new(Color.Black, coordinate);
            Rook rook = new(Color.Black, coordinate2);
            Square square = new(coordinate, pawn);

            square.Piece = rook;

            Assert.AreEqual(square.Piece, rook);
        }

        [TestMethod]
        public void Setter_SettingPiece_ShouldChangeTheOrigin()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A5");
            Pawn pawn = new(Color.Black, coordinate);
            Square square = new(coordinate2, null);

            square.Piece = pawn;

            Assert.AreEqual(square.Piece.Origin, coordinate2);
        }

        [TestMethod]
        public void NewInstance_ShouldChangeTheOriginOfThePiece()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A5");
            Pawn pawn = new(Color.Black, coordinate);
            Square square = new(coordinate2, pawn);

            Assert.AreEqual(square.Piece.Origin, coordinate2);
        }

        [TestMethod]
        public void HasPiece_SquareWithPiece_ShouldReturnTrue()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Pawn pawn = new(Color.Black, coordinate);
            Square square = new(coordinate, pawn);

            Assert.IsTrue(square.HasPiece);
        }

        [TestMethod]
        public void HasPiece_EmptySquare_ShouldReturnFalse()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Square square = new(coordinate, null);

            Assert.IsFalse(square.HasPiece);
        }
    }
}