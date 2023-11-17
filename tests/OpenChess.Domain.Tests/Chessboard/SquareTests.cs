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
    }
}