using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CheckTests
    {
        [DataRow('b', "r7/3R2k1/4P3/4K3/8/8/8/8 w - - 0 1")]
        [DataRow('b', "rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1")]
        [DataRow('b', "rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1")]
        [DataRow('b', "3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('w', "3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('w', "r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1")]
        [DataRow('w', "rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1")]
        [DataRow('w', "rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1")]
        [TestMethod]
        public void IsInCheck_PlayerInCheck_ShouldReturnTrue(char color, string fen)
        {
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);

            Assert.IsTrue(Check.IsInCheck(player, chessboard));
        }

        [DataRow('w', "r7/3R2k1/4P3/4K3/8/8/8/8 w - - 0 1")]
        [DataRow('w', "rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1")]
        [DataRow('w', "rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1")]
        [DataRow('w', "3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('w', "rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow('w', "8/8/8/5PK1/8/k7/8/3r4 w - - 0 1")]
        [DataRow('b', "3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('b', "r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1")]
        [DataRow('b', "rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1")]
        [DataRow('b', "rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1")]
        [DataRow('b', "rnbqkbnr/pp2pppp/2p5/3P4/3P4/8/PPP2PPP/RNBQKBNR b KQkq - 0 1")]
        [DataRow('b', "rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow('b', "8/8/8/5PK1/8/k7/8/3r4 w - - 0 1")]
        [TestMethod]
        public void IsInCheck_PlayerNotInCheck_ShouldReturnFalse(char color, string fen)
        {
            Chessboard chessboard = new(fen);
            Color player = color == 'w' ? Color.White : Color.Black;

            Assert.IsFalse(Check.IsInCheck(player, chessboard));
        }

        [DataRow('w', "3b4/8/4n3/5PK1/8/k4r2/8/3r4 w - - 0 1")]
        [DataRow('w', "r2qkb1r/1pp2ppp/p2p4/4p3/7b/1B1n1P2/PPPP2PP/RNBQK2R b KQkq - 0 1")]
        [DataRow('b', "3b4/4n3/8/5PK1/2N5/k4R2/8/3r4 w - - 0 1")]
        [DataRow('b', "r1bqkbnr/1pp2ppp/p1Bp1N2/4p3/4P3/8/PPPP1PPP/RNBQK2R b KQkq - 0 1")]
        [TestMethod]
        public void CheckAmount_DoubleCheck_ShouldReturn2(char color, string fen)
        {
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);

            Assert.AreEqual(2, Check.CalculateCheckAmount(player, chessboard));
        }

        [DataRow('b', "rn1qkb1r/ppp2pp1/5n1p/1B1p2B1/3P2b1/4P1P1/PP3P1P/RN1QK1NR b KQkq - 0 1")]
        [DataRow('w', "rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1")]
        [DataRow('b', "rn1qkb1r/pp1n2p1/2p2p1p/1B1p2BQ/3P4/4P1P1/PP3P1P/RN2K1NR b KQkq - 0 1")]
        [DataRow('b', "3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1")]
        [TestMethod]
        public void CheckAmount_Check_ShouldReturn1(char color, string fen)
        {
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);

            Assert.AreEqual(1, Check.CalculateCheckAmount(player, chessboard));
        }

        [DataRow('w', "3bk3/5P2/4P3/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('w', "rnbqkbnr/pp1ppppp/2p5/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow('w', "8/8/8/5PK1/8/k7/8/3r4 w - - 0 1")]
        [DataRow('b', "3bk3/8/4Pp2/4K3/8/8/4B3/8 w - - 0 1")]
        [DataRow('b', "r5K1/3R4/4Pk2/8/8/8/8/8 w - - 0 1")]
        [DataRow('b', "rnb1kbnr/pppp1ppp/8/8/2B1Pp1q/8/PPPP2PP/RNBQK1NR b KQkq - 0 1")]
        [DataRow('b', "rnb1k1nr/ppp1qppp/8/8/3N4/1B6/PPP3PP/RNBQK2R b KQkq - 0 1")]
        [TestMethod]
        public void CheckAmount_NotInCheck_ShouldReturn0(char color, string fen)
        {
            Chessboard chessboard = new(fen);
            Color player = Utils.ColorFromChar(color);

            Assert.AreEqual(0, Check.CalculateCheckAmount(player, chessboard));
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
            IReadOnlyPiece piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece!;

            Assert.IsTrue(Check.IsHittingTheEnemyKing(piece, chessboard));
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
            IReadOnlyPiece piece = chessboard.GetReadOnlySquare(origin).ReadOnlyPiece!;

            Assert.IsFalse(Check.IsHittingTheEnemyKing(piece, chessboard));
        }
    }
}