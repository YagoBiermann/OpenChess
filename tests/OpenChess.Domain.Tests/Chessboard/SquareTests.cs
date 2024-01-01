using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class SquareTests
    {
        [DataRow("A1")]
        [DataRow("B1")]
        [DataRow("C1")]
        [DataRow("D1")]
        [DataRow("E1")]
        [DataRow("F1")]
        [DataRow("G1")]
        [DataRow("H1")]
        [DataRow("A8")]
        [DataRow("B8")]
        [DataRow("C8")]
        [DataRow("D8")]
        [DataRow("E8")]
        [DataRow("F8")]
        [DataRow("G8")]
        [DataRow("H8")]
        [DataRow("A2")]
        [DataRow("A7")]
        [TestMethod]
        public void Getter_SquareNotEmpty_ShouldReturnPiece(string origin)
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            IReadOnlySquare square = chessboard.GetSquare(Coordinate.GetInstance(origin));

            Assert.IsNotNull(square.ReadOnlyPiece);
        }

        [TestMethod]
        public void Getter_EmptySquare_ReturnsNull()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Square square = chessboard.GetSquare(Coordinate.GetInstance("E4"));

            Assert.IsNull(square.Piece);
        }

        [TestMethod]
        public void HasPiece_SquareWithPiece_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("A1");
            IReadOnlySquare square = chessboard.GetSquare(coordinate);

            Assert.IsTrue(square.HasPiece);
        }

        [TestMethod]
        public void HasPiece_EmptySquare_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E4");
            IReadOnlySquare square = chessboard.GetSquare(coordinate);

            Assert.IsFalse(square.HasPiece);
        }

        [TestMethod]
        public void HasEnemyPiece_DifferentColor_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E7");
            IReadOnlySquare square = chessboard.GetSquare(coordinate);

            Assert.IsTrue(square.HasEnemyPiece(Color.White));
        }

        [TestMethod]
        public void HasEnemyPiece_SameColor_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E7");
            IReadOnlySquare square = chessboard.GetSquare(coordinate);

            Assert.IsFalse(square.HasEnemyPiece(Color.Black));
        }
    }
}