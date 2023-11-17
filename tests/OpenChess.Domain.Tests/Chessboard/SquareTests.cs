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
    }
}