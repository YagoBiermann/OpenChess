using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PieceTests
    {
        [TestMethod]
        public void Equals_SameObject_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Pawn pawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("A7")).Piece!;
            Pawn pawn2 = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("A7")).Piece!;

            Assert.IsTrue(pawn.Equals(pawn2));
        }

        [TestMethod]
        public void Equals_DifferentPieces_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Rook rook = (Rook)chessboard.GetSquare(Coordinate.GetInstance("A1")).Piece!;
            Queen queen = (Queen)chessboard.GetSquare(Coordinate.GetInstance("D1")).Piece!;

            Assert.IsFalse(rook.Equals(queen));
        }

        [TestMethod]
        public void Equals_SamePieceDifferentColors_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;
            King king2 = (King)chessboard.GetSquare(Coordinate.GetInstance("E8")).Piece!;

            Assert.IsFalse(king.Equals(king2));
        }

        [TestMethod]
        public void Equals_SamePieceDifferentOrigin_ShouldReturnFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Pawn pawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("A7")).Piece!;
            Pawn pawn2 = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("B7")).Piece!;

            Assert.IsFalse(pawn.Equals(pawn2));
        }

        [TestMethod]
        public void Create_UppercaseChar_ShouldCreateWhitePiece()
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

        [TestMethod]
        public void Create_LowercaseChar_ShouldCreateBlackPiece()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");

            Piece king = Piece.Create('k', coordinate);
            Piece queen = Piece.Create('q', coordinate);
            Piece rook = Piece.Create('r', coordinate);
            Piece bishop = Piece.Create('b', coordinate);
            Piece knight = Piece.Create('n', coordinate);
            Piece pawn = Piece.Create('p', coordinate);

            Assert.IsInstanceOfType(king, typeof(King));
            Assert.IsInstanceOfType(queen, typeof(Queen));
            Assert.IsInstanceOfType(rook, typeof(Rook));
            Assert.IsInstanceOfType(bishop, typeof(Bishop));
            Assert.IsInstanceOfType(knight, typeof(Knight));
            Assert.IsInstanceOfType(pawn, typeof(Pawn));

            Assert.AreEqual(king.Color, Color.Black);
            Assert.AreEqual(queen.Color, Color.Black);
            Assert.AreEqual(rook.Color, Color.Black);
            Assert.AreEqual(bishop.Color, Color.Black);
            Assert.AreEqual(knight.Color, Color.Black);
            Assert.AreEqual(pawn.Color, Color.Black);
        }

        [DataRow("E5", "4k3/7R/4P3/2K1r3/8/8/8/8 b - - 0 1")]
        [DataRow("E5", "4k3/7R/4P3/2K1q3/8/8/8/8 b - - 0 1")]
        [DataRow("E5", "4k3/7R/3KP3/4b3/8/8/8/8 b - - 0 1")]
        [DataRow("E5", "4k3/7R/4P3/4n3/2K5/8/8/8 b - - 0 1")]
        [DataRow("E5", "4k3/7R/4P3/4p3/3K4/8/8/8 b - - 0 1")]
        [TestMethod]
        public void IsHittingTheEnemyKing_PieceHittingEnemyKing_ShouldReturnTrue(string coordinate, string fen)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(coordinate);
            Piece piece = chessboard.GetSquare(origin).Piece!;

            Assert.IsTrue(piece.IsHittingTheEnemyKing(chessboard));
        }

        [DataRow("E5", "4k3/7R/4P3/4p3/4K3/8/8/8 b - - 0 1")]
        [DataRow("E5", "4k3/7R/4P3/4b3/4K3/8/8/8 b - - 0 1")]
        [DataRow("E5", "4k3/7R/4P3/4n3/4K3/8/8/8 b - - 0 1")]
        [DataRow("C3", "4k3/7R/4P3/8/4K3/2q5/8/8 b - - 0 1")]
        [DataRow("C3", "4k3/7R/4P3/8/4K3/2r5/8/8 b - - 0 1")]
        [TestMethod]
        public void IsHittingTheEnemyKing_PieceNotHittingEnemyKing_ShouldReturnFalse(string coordinate, string fen)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(coordinate);
            Piece piece = chessboard.GetSquare(origin).Piece!;

            Assert.IsFalse(piece.IsHittingTheEnemyKing(chessboard));
        }
    }
}