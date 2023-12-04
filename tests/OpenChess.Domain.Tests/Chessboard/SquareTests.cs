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
        public void Getter_PieceNotNull_ShouldReturnPiece(string origin)
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Square square = chessboard.GetSquare(Coordinate.GetInstance(origin));

            Assert.IsNotNull(square.Piece);
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

        [DataRow("A1", 'K')]
        [DataRow("A1", 'P')]
        [DataRow("A1", 'N')]
        [DataRow("A1", 'B')]
        [DataRow("A1", 'R')]
        [DataRow("A1", 'Q')]
        [TestMethod]
        public void HasTypeOfPiece_HavingTypeOfPiece_ShouldReturnTrue(string origin, char type)
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance(origin);
            Piece piece = Piece.Create(type, coordinate, chessboard);
            Square square = new(coordinate, piece);

            Type? pieceType;

            if (char.ToLower(type) == 'k') { pieceType = typeof(King); }
            else if (char.ToLower(type) == 'p') { pieceType = typeof(Pawn); }
            else if (char.ToLower(type) == 'n') { pieceType = typeof(Knight); }
            else if (char.ToLower(type) == 'b') { pieceType = typeof(Bishop); }
            else if (char.ToLower(type) == 'r') { pieceType = typeof(Rook); }
            else if (char.ToLower(type) == 'q') { pieceType = typeof(Queen); }
            else { pieceType = null; }

            Assert.IsTrue(square.HasTypeOfPiece(pieceType));
        }

        [TestMethod]
        public void HasTypeOfPiece_NotHavingTypeOfPiece_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Piece piece = Piece.Create('k', coordinate, chessboard);
            Square square = new(coordinate, piece);

            Assert.IsFalse(square.HasTypeOfPiece(typeof(Queen)));
            Assert.IsFalse(square.HasTypeOfPiece(typeof(Pawn)));
            Assert.IsFalse(square.HasTypeOfPiece(typeof(Rook)));
            Assert.IsFalse(square.HasTypeOfPiece(typeof(Bishop)));
            Assert.IsFalse(square.HasTypeOfPiece(typeof(Knight)));
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