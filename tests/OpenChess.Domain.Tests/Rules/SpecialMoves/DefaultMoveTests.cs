using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class DefaultMoveTests
    {

        [DataRow("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1", "B7", "B3", 'r', 'B')]
        [DataRow("rnbqkb1r/pp2pppp/5n2/3p4/2PP4/2N5/PP3PPP/R1BQKBNR b KQkq - 0 1", "C3", "D5", 'N', 'p')]
        [DataRow("rnbqk2r/pppp1ppp/4pn2/6B1/1bPP4/2N5/PP2PPPP/R2QKBNR b KQkq - 0 1", "B4", "C3", 'b', 'N')]
        [DataRow("1K6/7r/k3p3/3P4/8/8/8/8 b - - 0 1", "E6", "D5", 'p', 'P')]
        [TestMethod]
        public void MovePiece_ShouldBeAbleToCaptureEnemyPiece(string fen, string position1, string position2, char moved, char captured)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            IReadOnlyPiece? pieceCaptured = chessboard.MovePiece(origin, destination);

            Assert.IsFalse(chessboard.GetReadOnlySquare(origin).HasPiece);
            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetReadOnlySquare(destination).ReadOnlyPiece, Utils.GetPieceType(moved));
            Assert.IsInstanceOfType(pieceCaptured, Utils.GetPieceType(captured));
        }

        [DataRow("rnbqk2r/pppp1ppp/4pn2/6B1/1bPP4/2N5/PP2PPPP/R2QKBNR b KQkq - 0 1", "F6", "E8")]
        [DataRow("rnbqk2r/ppppp1bp/5np1/5p2/2PP4/5NP1/PP2PPBP/RNBQK2R b KQkq - 0 1", "D1", "D4")]
        [DataRow("rnbqk2r/ppppp1bp/5np1/5p2/2PP4/5NP1/PP2PPBP/RNBQK2R b KQkq - 0 1", "D1", "E1")]
        [DataRow("8/2K4r/k7/3P4/8/8/8/8 b - - 0 1", "H7", "C7")]
        [TestMethod]
        public void MovePiece_ShouldNotBeAbleToCaptureAllyPieceOrKing(string fen, string position1, string position2)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
        }

        [TestMethod]
        public void MovePiece_OriginWithEmptySquare_ShouldThrowException()
        {
            Chessboard chessboard = new("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1");
            Coordinate origin = Coordinate.GetInstance("A1");
            Coordinate destination = Coordinate.GetInstance("A2");

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
        }

        [TestMethod]
        public void MovePiece_GivenOriginAndDestinationWithEmptySquare_ShouldChangePiecePosition()
        {
            Chessboard chessboard = new("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1");
            Coordinate origin = Coordinate.GetInstance("B7");
            Coordinate destination = Coordinate.GetInstance("B8");

            IReadOnlyPiece? pieceCaptured = chessboard.MovePiece(origin, destination);

            Assert.IsFalse(chessboard.GetReadOnlySquare(origin).HasPiece);
            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetReadOnlySquare(destination).ReadOnlyPiece, typeof(Rook));
            Assert.IsNull(pieceCaptured);
        }

    }
}