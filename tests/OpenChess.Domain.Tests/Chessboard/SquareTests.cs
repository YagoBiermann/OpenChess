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
            Chessboard chessboard = new(FEN.InitialPosition);
            IReadOnlySquare square = chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin));

            Assert.IsNotNull(square.ReadOnlyPiece);
        }

        [TestMethod]
        public void Getter_EmptySquare_ReturnsNull()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            IReadOnlySquare square = chessboard.GetReadOnlySquare(Coordinate.GetInstance("E4"));

            Assert.IsNull(square.ReadOnlyPiece);
        }

        [TestMethod]
        public void HasPiece_SquareWithPiece_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("A1");
            IReadOnlySquare square = chessboard.GetReadOnlySquare(coordinate);

            Assert.IsTrue(square.HasPiece);
        }

        [TestMethod]
        public void HasPiece_EmptySquare_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E4");
            IReadOnlySquare square = chessboard.GetReadOnlySquare(coordinate);

            Assert.IsFalse(square.HasPiece);
        }

        [DataRow("A1", 'R')]
        [DataRow("B1", 'N')]
        [DataRow("C1", 'B')]
        [DataRow("D1", 'Q')]
        [DataRow("E1", 'K')]
        [DataRow("F1", 'B')]
        [DataRow("G1", 'N')]
        [DataRow("H1", 'R')]
        [DataRow("A2", 'P')]
        [DataRow("A8", 'r')]
        [DataRow("B8", 'n')]
        [DataRow("C8", 'b')]
        [DataRow("D8", 'q')]
        [DataRow("E8", 'k')]
        [DataRow("F8", 'b')]
        [DataRow("G8", 'n')]
        [DataRow("H8", 'r')]
        [DataRow("A7", 'p')]
        [TestMethod]
        public void HasTypeOfPiece_HavingTypeOfPiece_ShouldReturnTrue(string origin, char type)
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            IReadOnlySquare square = chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin));

            Type pieceType = Utils.GetPieceType(type);

            Assert.IsTrue(square.HasTypeOfPiece(pieceType));
        }

        [DataRow("E4", 'p')]
        [DataRow("E4", 'q')]
        [DataRow("E4", 'r')]
        [DataRow("E4", 'n')]
        [DataRow("E4", 'b')]
        [TestMethod]
        public void HasTypeOfPiece_NotHavingTypeOfPiece_ShouldReturnFalse(string origin, char type)
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            IReadOnlySquare square = chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin));

            Type pieceType = Utils.GetPieceType(type);

            Assert.IsFalse(square.HasTypeOfPiece(pieceType));
        }

        [TestMethod]
        public void HasEnemyPiece_DifferentColor_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E7");
            IReadOnlySquare square = chessboard.GetReadOnlySquare(coordinate);

            Assert.IsTrue(square.HasEnemyPiece(Color.White));
        }

        [TestMethod]
        public void HasEnemyPiece_SameColor_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E7");
            IReadOnlySquare square = chessboard.GetReadOnlySquare(coordinate);

            Assert.IsFalse(square.HasEnemyPiece(Color.Black));
        }
    }
}