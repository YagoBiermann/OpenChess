using System.Data;
using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class ChessboardTests
    {

        [TestMethod]
        public void NewInstance_ShouldConvertFenStringCorrectly()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);

            Assert.AreEqual(Color.White, chessboard.CurrentPlayer);
            Assert.IsNull(chessboard.EnPassantAvailability.EnPassantPosition);
            Assert.IsTrue(chessboard.CastlingAvailability.IsWhiteKingSideAvailable);
            Assert.IsTrue(chessboard.CastlingAvailability.IsWhiteQueenSideAvailable);
            Assert.IsTrue(chessboard.CastlingAvailability.IsBlackKingSideAvailable);
            Assert.IsTrue(chessboard.CastlingAvailability.IsBlackQueenSideAvailable);
            Assert.AreEqual(0, chessboard.HalfMove);
            Assert.AreEqual(1, chessboard.FullMove);
        }

        [DataRow("C3", 'Q', 'w')]
        [DataRow("B4", 'N', 'w')]
        [DataRow("C5", 'B', 'w')]
        [DataRow("D4", 'R', 'w')]
        [DataRow("C4", 'K', 'w')]
        [DataRow("E2", 'P', 'w')]
        [DataRow("F5", 'q', 'b')]
        [DataRow("G6", 'n', 'b')]
        [DataRow("E6", 'b', 'b')]
        [DataRow("F7", 'r', 'b')]
        [DataRow("F6", 'k', 'b')]
        [DataRow("C7", 'p', 'b')]
        [TestMethod]
        public void NewInstance_ShouldConvertPiecesCorrectly(string position, char name, char color)
        {
            Chessboard chessboard = new("8/2p2r2/4bkn1/2B2q2/1NKR4/2Q5/4P3/8 w - - 0 1");

            Coordinate coordinate = Coordinate.GetInstance(position);
            Type? pieceType = Utils.GetPieceType(name);
            Color pieceColor = Utils.ColorFromChar(color);
            IReadOnlyPiece? piece = chessboard.GetPiece(coordinate);

            Assert.IsInstanceOfType(piece, pieceType);
            Assert.AreEqual(pieceColor, piece.Color);
        }

        [TestMethod]
        public void NewInstance_ShouldConvertCastlingCorrectly()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b - - 0 1");

            Assert.IsFalse(chessboard.CastlingAvailability.IsWhiteKingSideAvailable);
            Assert.IsFalse(chessboard.CastlingAvailability.IsWhiteQueenSideAvailable);
            Assert.IsFalse(chessboard.CastlingAvailability.IsBlackKingSideAvailable);
            Assert.IsFalse(chessboard.CastlingAvailability.IsBlackQueenSideAvailable);
            Assert.AreEqual(Color.Black, chessboard.CurrentPlayer);
            Assert.IsNull(chessboard.EnPassantAvailability.EnPassantPosition);
            Assert.AreEqual(0, chessboard.HalfMove);
            Assert.AreEqual(1, chessboard.FullMove);
        }

        [TestMethod]
        public void NewInstance_ShouldConvertCastlingCorrectly_case2()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b Kk - 0 1");

            Assert.IsTrue(chessboard.CastlingAvailability.IsWhiteKingSideAvailable);
            Assert.IsTrue(chessboard.CastlingAvailability.IsBlackKingSideAvailable);
        }

        [TestMethod]
        public void NewInstance_ShouldConvertEnPassantCorrectly()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b Kk E3 0 1");
            Assert.AreEqual(Coordinate.GetInstance("E3"), chessboard.EnPassantAvailability.EnPassantPosition);
        }
        [TestMethod]
        public void NewInstance_NoEnPassant_ShouldBeNull()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b Kk - 0 1");
            Assert.IsNull(chessboard.EnPassantAvailability.EnPassantPosition);
        }

        [DataRow("A1", 'R', 'w')]
        [DataRow("B1", 'N', 'w')]
        [DataRow("C1", 'B', 'w')]
        [DataRow("D1", 'Q', 'w')]
        [DataRow("E1", 'K', 'w')]
        [DataRow("F1", 'B', 'w')]
        [DataRow("G1", 'N', 'w')]
        [DataRow("H1", 'R', 'w')]
        [DataRow("A2", 'P', 'w')]
        [DataRow("B2", 'P', 'w')]
        [DataRow("C2", 'P', 'w')]
        [DataRow("D2", 'P', 'w')]
        [DataRow("E2", 'P', 'w')]
        [DataRow("F2", 'P', 'w')]
        [DataRow("G2", 'P', 'w')]
        [DataRow("H2", 'P', 'w')]
        [DataRow("A8", 'r', 'b')]
        [DataRow("B8", 'n', 'b')]
        [DataRow("C8", 'b', 'b')]
        [DataRow("D8", 'q', 'b')]
        [DataRow("E8", 'k', 'b')]
        [DataRow("F8", 'b', 'b')]
        [DataRow("G8", 'n', 'b')]
        [DataRow("H8", 'r', 'b')]
        [DataRow("A7", 'p', 'b')]
        [DataRow("B7", 'p', 'b')]
        [DataRow("C7", 'p', 'b')]
        [DataRow("D7", 'p', 'b')]
        [DataRow("E7", 'p', 'b')]
        [DataRow("F7", 'p', 'b')]
        [DataRow("G7", 'p', 'b')]
        [DataRow("H7", 'p', 'b')]
        [TestMethod]
        public void NewInstance_GivenFenString_ShouldAddWhitePiecesCorrectly(string coordinate, char type, char c)
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);

            Coordinate origin = Coordinate.GetInstance(coordinate);
            IReadOnlyPiece? piece = chessboard.GetPiece(origin);
            Type? pieceType = Utils.GetPieceType(type);
            Color color = Utils.ColorFromChar(c);

            Assert.IsNotNull(piece);
            Assert.IsInstanceOfType(piece, pieceType);
            Assert.AreEqual(piece.Color, color);
        }

        [TestMethod]
        public void NewInstance_InitialPosition_EmptySquares_ShouldReturnPieceAsNull()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);

            for (int row = 2; row <= 5; row++)
            {
                for (int col = 0; col <= 7; col++)
                {

                    IReadOnlySquare square = chessboard.GetSquare(Coordinate.GetInstance(col, row));
                    Assert.IsNull(square.ReadOnlyPiece);
                }
            }
        }

        [DataRow("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1")]
        [DataRow("rnbqkb1r/pppppp1p/5np1/8/2PP4/8/PP2PPPP/RNBQKBNR w KQkq - 0 1")]
        [DataRow("8/5P2/3K4/8/8/1k6/8/q7 w - - 0 1")]
        [DataRow("3k4/1K6/2PbP3/3B4/8/8/8/8 w - - 0 1")]
        [DataRow("r1bqk2r/p2p1ppp/1pnbpn2/2p5/2PP4/3BPN1P/PP3PP1/RNBQ1RK1 b kq - 0 1")]
        [TestMethod]
        public void ToString_ShouldConvertChessboardToString(string fen)
        {
            Chessboard chessboard = new(fen);
            string fromChessboard = chessboard.ToString();

            Assert.IsTrue(FenInfo.IsValid(fromChessboard));
            Assert.AreEqual(chessboard.ToString(), fen);
        }

        [TestMethod]
        public void MovePiece_ShouldSwitchTurns()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);

            Assert.AreEqual(Color.White, chessboard.CurrentPlayer);
            chessboard.MovePiece(Coordinate.GetInstance("E2"), Coordinate.GetInstance("E4"));
            Assert.AreEqual(Color.Black, chessboard.CurrentPlayer);
        }

        [DataRow("r3k2r/ppp2pbp/2nqpnp1/3p1b2/3P1B2/2NQPNP1/PPP2PBP/R3K2R w KQkq - 0 1", "D6", "B6")]
        [TestMethod]
        public void MovePiece_InvalidMove_ShouldThrowExceptionAndRestoreChessboardToLastPosition(string position, string orig, string dest)
        {
            Chessboard chessboard = new(position);
            Coordinate origin = Coordinate.GetInstance(orig);
            Coordinate destination = Coordinate.GetInstance(dest);

            string currentPosition = chessboard.ToString();

            Assert.ThrowsException<ChessboardException>(() => chessboard.MovePiece(origin, destination));
            Assert.AreEqual(currentPosition, chessboard.ToString());
        }
    }
}