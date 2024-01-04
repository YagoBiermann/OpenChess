using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PieceTests
    {
        [DataRow("A1", 'R')]
        [DataRow("B1", 'N')]
        [DataRow("C1", 'B')]
        [DataRow("D1", 'Q')]
        [DataRow("E1", 'K')]
        [DataRow("A2", 'P')]
        [DataRow("A8", 'r')]
        [DataRow("B8", 'n')]
        [DataRow("C8", 'b')]
        [DataRow("D8", 'q')]
        [DataRow("E8", 'k')]
        [DataRow("A7", 'p')]
        [TestMethod]
        public void Name_ShouldBeInCorrectFormat(string origin, char name)
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            IReadOnlyPiece piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece;

            Assert.AreEqual(name, piece.Name);
        }

        [TestMethod]
        public void Equals_SameObject_ShouldReturnTrue()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare("A7").ReadOnlyPiece!;
            Pawn pawn2 = (Pawn)chessboard.GetReadOnlySquare("A7").ReadOnlyPiece!;

            Assert.IsTrue(pawn.Equals(pawn2));
        }

        [TestMethod]
        public void Equals_DifferentPieces_ShouldReturnFalse()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            Rook rook = (Rook)chessboard.GetReadOnlySquare("A1").ReadOnlyPiece!;
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D1").ReadOnlyPiece!;

            Assert.IsFalse(rook.Equals(queen));
        }

        [TestMethod]
        public void Equals_SamePieceDifferentColors_ShouldReturnFalse()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            King king = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;
            King king2 = (King)chessboard.GetReadOnlySquare("E8").ReadOnlyPiece!;

            Assert.IsFalse(king.Equals(king2));
        }

        [TestMethod]
        public void Equals_SamePieceDifferentOrigin_ShouldReturnFalse()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare("A7").ReadOnlyPiece!;
            Pawn pawn2 = (Pawn)chessboard.GetReadOnlySquare("B7").ReadOnlyPiece!;

            Assert.IsFalse(pawn.Equals(pawn2));
        }
    }
}