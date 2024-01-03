using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class DefaultMoveTests
    {

        [DataRow("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1", "B7", "B3", 'r', 'B')]
        [DataRow("rnbqkb1r/pp2pppp/5n2/3p4/2PP4/2N5/PP3PPP/R1BQKBNR w KQkq - 0 1", "C3", "D5", 'N', 'p')]
        [DataRow("rnbqk2r/pppp1ppp/4pn2/6B1/1bPP4/2N5/PP2PPPP/R2QKBNR b KQkq - 0 1", "B4", "C3", 'b', 'N')]
        [DataRow("1K6/7r/k3p3/3P4/8/8/8/8 b - - 0 1", "E6", "D5", 'p', 'P')]
        [TestMethod]
        public void MovePiece_ShouldBeAbleToCaptureEnemyPiece(string fen, string position1, string position2, char moved, char captured)
        {
            Chessboard chessboard = new(new FenInfo(fen));
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            MovePlayed movePlayed = chessboard.MovePiece(origin, destination);

            Assert.IsFalse(chessboard.GetSquare(origin).HasPiece);
            Assert.IsTrue(chessboard.GetSquare(destination).HasPiece);
            Assert.IsInstanceOfType(movePlayed.PieceMoved, Utils.GetPieceType(moved));
            Assert.IsInstanceOfType(movePlayed.PieceCaptured, Utils.GetPieceType(captured));
        }

        [DataRow("rnbqk2r/pppp1ppp/4pn2/6B1/1bPP4/2N5/PP2PPPP/R2QKBNR b KQkq - 0 1", "F6", "E8")]
        [DataRow("rnbqk2r/ppppp1bp/5np1/5p2/2PP4/5NP1/PP2PPBP/RNBQK2R b KQkq - 0 1", "D1", "D4")]
        [DataRow("rnbqk2r/ppppp1bp/5np1/5p2/2PP4/5NP1/PP2PPBP/RNBQK2R b KQkq - 0 1", "D1", "E1")]
        [TestMethod]
        public void MovePiece_ShouldNotBeAbleToCaptureAllyPiece(string fen, string position1, string position2)
        {
            Chessboard chessboard = new(new FenInfo(fen));
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
        }

        [TestMethod]
        public void MovePiece_OriginWithEmptySquare_ShouldThrowException()
        {
            Chessboard chessboard = new(new FenInfo("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1"));
            Coordinate origin = Coordinate.GetInstance("A1");
            Coordinate destination = Coordinate.GetInstance("A2");

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
        }

        [TestMethod]
        public void MovePiece_GivenOriginAndDestinationWithEmptySquare_ShouldChangePiecePosition()
        {
            Chessboard chessboard = new(new FenInfo("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1"));
            Coordinate origin = Coordinate.GetInstance("B7");
            Coordinate destination = Coordinate.GetInstance("B8");

            MovePlayed movePlayed = chessboard.MovePiece(origin, destination);

            Assert.IsFalse(chessboard.GetSquare(origin).HasPiece);
            Assert.IsTrue(chessboard.GetSquare(destination).HasPiece);
            Assert.IsInstanceOfType(movePlayed.PieceMoved, typeof(Rook));
            Assert.IsNull(movePlayed.PieceCaptured);
        }

    }
}