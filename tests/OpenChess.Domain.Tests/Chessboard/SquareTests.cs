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
            Square square = chessboard.GetSquare(Coordinate.GetInstance(origin));

            Assert.IsNotNull(square.Piece);
        }

        [TestMethod]
        public void Getter_EmptySquare_ReturnsNull()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Square square = chessboard.GetSquare(Coordinate.GetInstance("E4"));

            Assert.IsNull(square.Piece);
        }

        [TestMethod]
        public void Setter_SettingPieceAsNull_ShouldRemoveThePiece()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate origin = Coordinate.GetInstance("A1");
            Square square = chessboard.GetSquare(origin);

            square.Piece = null;

            Assert.IsNull(square.Piece);
        }

        [TestMethod]
        public void Setter_SettingPiece_ShouldReplaceTheCurrentPiece()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate origin = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A7");
            Square square = chessboard.GetSquare(origin);
            Piece piece = chessboard.GetSquare(coordinate2).Piece!;

            square.Piece = piece;

            Assert.AreEqual(square.Piece, piece);
        }

        [TestMethod]
        public void Setter_SettingPiece_ShouldChangeTheOrigin()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate origin = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A7");
            Square square = chessboard.GetSquare(origin);
            Piece piece = chessboard.GetSquare(coordinate2).Piece!;

            square.Piece = piece;

            Assert.AreEqual(square.Piece.Origin, piece.Origin);
        }

        [TestMethod]
        public void HasPiece_SquareWithPiece_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Square square = chessboard.GetSquare(coordinate);

            Assert.IsTrue(square.HasPiece);
        }

        [TestMethod]
        public void HasPiece_EmptySquare_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E4");
            Square square = chessboard.GetSquare(coordinate);

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
            Square square = chessboard.GetSquare(Coordinate.GetInstance(origin));

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
            Square square = chessboard.GetSquare(Coordinate.GetInstance(origin));

            Type pieceType = Utils.GetPieceType(type);

            Assert.IsFalse(square.HasTypeOfPiece(pieceType));
        }

        [TestMethod]
        public void HasEnemyPiece_DifferentColor_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E7");
            Square square = chessboard.GetSquare(coordinate);

            Assert.IsTrue(square.HasEnemyPiece(Color.White));
        }

        [TestMethod]
        public void HasEnemyPiece_SameColor_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("E7");
            Square square = chessboard.GetSquare(coordinate);

            Assert.IsFalse(square.HasEnemyPiece(Color.Black));
        }
    }
}