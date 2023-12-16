using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PromotionTests
    {
        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/8 b - - 0 1", "F2", "F1", "K")]
        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/8 b - - 0 1", "F2", "F1", "P")]
        [TestMethod]
        public void MovePiece_PromotingPawnToAnInvalidPiece_ShouldThrowException(string fen, string position1, string position2, string promotingPiece)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination, promotingPiece));
        }

        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/4R3 b - - 0 1", "F2", "E1", null)]
        [DataRow("4r3/5P2/8/8/2K5/8/2kR4/8 w - - 0 1", "F7", "E8", null)]
        [TestMethod]
        public void MovePiece_PromotingPawn_NullString_ShouldPromotePawnToQueen(string fen, string position1, string position2, string promotingPiece)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);
            chessboard.MovePiece(origin, destination, promotingPiece);

            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasTypeOfPiece(typeof(Queen)));
        }

        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/8 b - - 0 1", "F2", "F1", 'q', "Q")]
        [DataRow("8/8/8/3r4/2KP4/8/2k2p2/4R3 b - - 0 1", "F2", "E1", 'n', "N")]
        [DataRow("4r3/5P2/8/8/2K5/8/2kR4/8 w - - 0 1", "F7", "E8", 'b', "B")]
        [DataRow("4r3/5P2/8/8/2K5/8/2kR4/8 w - - 0 1", "F7", "F8", 'r', "R")]
        [TestMethod]
        public void MovePiece_ShouldHandlePawnPromotion(string fen, string position1, string position2, char pieceType, string promotingPiece)
        {
            Chessboard chessboard = new(fen);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);
            chessboard.MovePiece(origin, destination, promotingPiece);
            Type? piece = Utils.GetPieceType(pieceType);

            Assert.IsFalse(chessboard.GetReadOnlySquare(origin).HasPiece);
            Assert.IsTrue(chessboard.GetReadOnlySquare(destination).HasTypeOfPiece(piece!));
        }
    }
}