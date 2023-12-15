using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class EnPassantTests
    {
        [DataRow("rnbqkbnr/pppp1ppp/8/8/3pP3/8/PPPP1PPP/RNBQKBNR b KQkq E3 0 1", "D4", "E3", 'w', 'b')]
        [DataRow("rnbqkbnr/pppp1ppp/8/3Pp3/8/8/PPP1PPPP/RNBQKBNR w KQkq E6 0 1", "D5", "E6", 'b', 'w')]
        [TestMethod]
        public void MovePiece_PawnVulnerable_ShouldBeCapturedByEnPassant(string fen, string position1, string position2, char color1, char color2)
        {
            Chessboard chessboard = new(fen);
            var origin = Coordinate.GetInstance(position1);
            var destination = Coordinate.GetInstance(position2);

            IReadOnlyPiece? pieceCaptured = chessboard.MovePiece(origin, destination).PieceCaptured;

            Assert.IsInstanceOfType(pieceCaptured, typeof(Pawn));
            Assert.AreEqual(pieceCaptured!.Color, Utils.ColorFromChar(color1));
            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasPiece);
            Assert.AreEqual(chessboard.GetReadOnlySquare(destination).ReadOnlyPiece!.Color, Utils.ColorFromChar(color2));
        }

        [DataRow("rnbqkbnr/pppp1ppp/8/8/3pP3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1", "D4", "E3")]
        [DataRow("rnbqkbnr/pppp1ppp/8/3Pp3/8/8/PPP1PPPP/RNBQKBNR w KQkq - 0 1", "D5", "E6")]
        [TestMethod]
        public void MovePiece_PawnNotVulnerable_ShouldNotBeCaptured(string fen, string position1, string position2)
        {
            Chessboard chessboard = new(fen);
            var origin = Coordinate.GetInstance(position1);
            var destination = Coordinate.GetInstance(position2);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq E3 0 1", "D7", "E4")]
        [DataRow("rnbqkbnr/pppp1ppp/8/4p3/8/8/PPPPPPPP/RNBQKBNR w KQkq E6 0 1", "D2", "E5")]
        [TestMethod]
        public void MovePiece_PawnOutOfRange_ShouldNotBeAbleToCapture(string fen, string position1, string position2)
        {
            Chessboard chessboard = new(fen);
            var origin = Coordinate.GetInstance(position1);
            var destination = Coordinate.GetInstance(position2);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
        }

        [TestMethod]
        public void MovePiece_ShouldSetPawnAsVulnerableOnMovingTwoSquaresForward()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);

            Assert.IsNull(chessboard.EnPassantAvailability.EnPassantPosition);
            chessboard.MovePiece(Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.AreEqual(Coordinate.GetInstance("E3"), chessboard.EnPassantAvailability.EnPassantPosition);
        }

        [TestMethod]
        public void MovePiece_ShouldNotSetPawnAsVulnerableOnMovingOneSquaresForward()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);

            chessboard.MovePiece(Coordinate.GetInstance("E2"), Coordinate.GetInstance("E3"));
            Assert.IsNull(chessboard.EnPassantAvailability.EnPassantPosition);
        }

    }
}