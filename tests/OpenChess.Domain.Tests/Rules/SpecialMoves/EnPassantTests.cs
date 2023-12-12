using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class EnPassantTests
    {

        [DataRow("E4", "E3")]
        [DataRow("E5", "E6")]
        [TestMethod]
        public void GetEnPassantPosition_ShouldReturnPositionBehind(string origin, string expected)
        {
            Coordinate position = Coordinate.GetInstance(origin);
            Coordinate expectedPosition = Coordinate.GetInstance(expected);
            Chessboard chessboard = new("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1");
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;

            Assert.AreEqual(expectedPosition, chessboard.EnPassant.GetEnPassantPosition(pawn));
        }

        [DataRow("E2")]
        [DataRow("E7")]
        [TestMethod]
        public void GetEnPassantPosition_PawnNotVulnerable_ShouldReturnNull(string origin)
        {
            Coordinate position = Coordinate.GetInstance(origin);
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;

            Assert.IsNull(chessboard.EnPassant.GetEnPassantPosition(pawn));
        }

        [DataRow("E4")]
        [DataRow("E5")]
        [TestMethod]
        public void IsVulnerableToEnPassant_PawnVulnerable_ShouldReturnTrue(string origin)
        {
            Coordinate position = Coordinate.GetInstance(origin);
            Chessboard chessboard = new("rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1");
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;

            Assert.IsTrue(chessboard.EnPassant.IsVulnerableToEnPassant(pawn));
        }

        [DataRow("E2")]
        [DataRow("E7")]
        [TestMethod]
        public void IsVulnerableToEnPassant_PawnNotVulnerable_ShouldReturnFalse(string origin)
        {
            Coordinate position = Coordinate.GetInstance(origin);
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare(position).ReadOnlyPiece!;

            Assert.IsFalse(chessboard.EnPassant.IsVulnerableToEnPassant(pawn));
        }

        [DataRow("D4", "E4", "rnbqkbnr/pppp1ppp/8/8/3pP3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("D5", "E5", "rnbqkbnr/pppp1ppp/8/3Pp3/8/8/PPP1PPPP/RNBQKBNR w KQkq - 0 1")]
        [TestMethod]
        public void CanCaptureByEnPassant_WithinTheRange_ShouldReturnTrue(string origin, string enPassant, string fen)
        {
            Chessboard chessboard = new(fen);
            IReadOnlyPiece? vulnerablePawn = chessboard.GetReadOnlySquare(Coordinate.GetInstance(enPassant)).ReadOnlyPiece;
            chessboard.EnPassant.SetVulnerablePawn(vulnerablePawn);
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin)).ReadOnlyPiece!;

            Assert.IsTrue(chessboard.EnPassant.CanCaptureByEnPassant(pawn));
        }

        [DataRow("D7", "E4", "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("D2", "E5", "rnbqkbnr/pppp1ppp/8/4p3/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [TestMethod]
        public void CanCaptureByEnPassant_OutOfTheRange_ShouldReturnFalse(string origin, string enPassant, string fen)
        {
            Chessboard chessboard = new(fen);
            IReadOnlyPiece? vulnerablePawn = chessboard.GetReadOnlySquare(Coordinate.GetInstance(enPassant)).ReadOnlyPiece;
            chessboard.EnPassant.SetVulnerablePawn(vulnerablePawn);
            Pawn pawn = (Pawn)chessboard.GetReadOnlySquare(Coordinate.GetInstance(origin)).ReadOnlyPiece!;

            Assert.IsFalse(chessboard.EnPassant.CanCaptureByEnPassant(pawn));
        }
    }
}