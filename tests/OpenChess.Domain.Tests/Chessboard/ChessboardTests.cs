using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class ChessboardTests
    {

        [TestMethod]
        public void NewInstance_ShouldConvertFenStringCorrectly()
        {
            Chessboard chessboard = new(FEN.InitialPosition);

            Assert.AreEqual(Color.White, chessboard.Turn);
            Assert.IsNull(chessboard.EnPassant);
            Assert.IsTrue(chessboard.CastlingAvailability.WhiteKingSide);
            Assert.IsTrue(chessboard.CastlingAvailability.WhiteQueenSide);
            Assert.IsTrue(chessboard.CastlingAvailability.BlackKingSide);
            Assert.IsTrue(chessboard.CastlingAvailability.BlackQueenSide);
            Assert.AreEqual(0, chessboard.HalfMove);
            Assert.AreEqual(1, chessboard.FullMove);
        }

        [TestMethod]
        public void NewInstance_ShouldConvertCastlingCorrectly()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b - - 0 1");

            Assert.IsFalse(chessboard.CastlingAvailability.WhiteKingSide);
            Assert.IsFalse(chessboard.CastlingAvailability.WhiteQueenSide);
            Assert.IsFalse(chessboard.CastlingAvailability.BlackKingSide);
            Assert.IsFalse(chessboard.CastlingAvailability.BlackQueenSide);
            Assert.AreEqual(Color.Black, chessboard.Turn);
            Assert.IsNull(chessboard.EnPassant);
            Assert.AreEqual(0, chessboard.HalfMove);
            Assert.AreEqual(1, chessboard.FullMove);
        }

        [TestMethod]
        public void NewInstance_ShouldConvertCastlingCorrectly_case2()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b Kk - 0 1");

            Assert.IsTrue(chessboard.CastlingAvailability.WhiteKingSide);
            Assert.IsTrue(chessboard.CastlingAvailability.BlackKingSide);
        }

        [TestMethod]
        public void NewInstance_ShouldConvertEnPassantCorrectly()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b Kk E3 0 1");
            Assert.AreEqual(Coordinate.GetInstance("E3"), chessboard.EnPassant);
        }
        [TestMethod]
        public void NewInstance_NoEnPassant_ShouldBeNull()
        {
            Chessboard chessboard = new("6r1/8/P7/1P5k/8/8/7K/8 b Kk - 0 1");
            Assert.IsNull(chessboard.EnPassant);
        }
        [TestMethod]
        public void NewInstance_GivenFenString_ShouldAddWhitePiecesCorrectly()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate A1 = Coordinate.GetInstance("A1");
            Coordinate B1 = Coordinate.GetInstance("B1");
            Coordinate C1 = Coordinate.GetInstance("C1");
            Coordinate D1 = Coordinate.GetInstance("D1");
            Coordinate E1 = Coordinate.GetInstance("E1");
            Coordinate F1 = Coordinate.GetInstance("F1");
            Coordinate G1 = Coordinate.GetInstance("G1");
            Coordinate H1 = Coordinate.GetInstance("H1");
            Coordinate A2 = Coordinate.GetInstance("A2");
            Coordinate B2 = Coordinate.GetInstance("B2");
            Coordinate C2 = Coordinate.GetInstance("C2");
            Coordinate D2 = Coordinate.GetInstance("D2");
            Coordinate E2 = Coordinate.GetInstance("E2");
            Coordinate F2 = Coordinate.GetInstance("F2");
            Coordinate G2 = Coordinate.GetInstance("G2");
            Coordinate H2 = Coordinate.GetInstance("H2");

            Piece? whiteRook = chessboard.GetSquare(A1).Piece;
            Piece? whiteKnight = chessboard.GetSquare(B1).Piece;
            Piece? whiteBishop = chessboard.GetSquare(C1).Piece;
            Piece? whiteQueen = chessboard.GetSquare(D1).Piece;
            Piece? whiteKing = chessboard.GetSquare(E1).Piece;
            Piece? whiteBishop2 = chessboard.GetSquare(F1).Piece;
            Piece? whiteKnight2 = chessboard.GetSquare(G1).Piece;
            Piece? whiteRook2 = chessboard.GetSquare(H1).Piece;
            Piece? whitePawn1 = chessboard.GetSquare(A2).Piece;
            Piece? whitePawn2 = chessboard.GetSquare(B2).Piece;
            Piece? whitePawn3 = chessboard.GetSquare(C2).Piece;
            Piece? whitePawn4 = chessboard.GetSquare(D2).Piece;
            Piece? whitePawn5 = chessboard.GetSquare(E2).Piece;
            Piece? whitePawn6 = chessboard.GetSquare(F2).Piece;
            Piece? whitePawn7 = chessboard.GetSquare(G2).Piece;
            Piece? whitePawn8 = chessboard.GetSquare(H2).Piece;

            Assert.IsNotNull(whiteRook);
            Assert.IsInstanceOfType(whiteRook, typeof(Rook));
            Assert.AreEqual(whiteRook?.Color, Color.White);

            Assert.IsNotNull(whiteKnight);
            Assert.IsInstanceOfType(whiteKnight, typeof(Knight));
            Assert.AreEqual(whiteKnight?.Color, Color.White);

            Assert.IsNotNull(whiteBishop);
            Assert.IsInstanceOfType(whiteBishop, typeof(Bishop));
            Assert.AreEqual(whiteBishop?.Color, Color.White);

            Assert.IsNotNull(whiteQueen);
            Assert.IsInstanceOfType(whiteQueen, typeof(Queen));
            Assert.AreEqual(whiteQueen?.Color, Color.White);

            Assert.IsNotNull(whiteKing);
            Assert.IsInstanceOfType(whiteKing, typeof(King));
            Assert.AreEqual(whiteKing?.Color, Color.White);

            Assert.IsNotNull(whiteBishop2);
            Assert.IsInstanceOfType(whiteBishop2, typeof(Bishop));
            Assert.AreEqual(whiteBishop2?.Color, Color.White);

            Assert.IsNotNull(whiteKnight2);
            Assert.IsInstanceOfType(whiteKnight2, typeof(Knight));
            Assert.AreEqual(whiteKnight2?.Color, Color.White);

            Assert.IsNotNull(whiteRook2);
            Assert.IsInstanceOfType(whiteRook2, typeof(Rook));
            Assert.AreEqual(whiteRook2?.Color, Color.White);

            Assert.IsNotNull(whitePawn1);
            Assert.IsInstanceOfType(whitePawn1, typeof(Pawn));
            Assert.AreEqual(whitePawn1?.Color, Color.White);

            Assert.IsNotNull(whitePawn2);
            Assert.IsInstanceOfType(whitePawn2, typeof(Pawn));
            Assert.AreEqual(whitePawn2?.Color, Color.White);

            Assert.IsNotNull(whitePawn3);
            Assert.IsInstanceOfType(whitePawn3, typeof(Pawn));
            Assert.AreEqual(whitePawn3?.Color, Color.White);

            Assert.IsNotNull(whitePawn4);
            Assert.IsInstanceOfType(whitePawn4, typeof(Pawn));
            Assert.AreEqual(whitePawn4?.Color, Color.White);

            Assert.IsNotNull(whitePawn5);
            Assert.IsInstanceOfType(whitePawn5, typeof(Pawn));
            Assert.AreEqual(whitePawn5?.Color, Color.White);

            Assert.IsNotNull(whitePawn6);
            Assert.IsInstanceOfType(whitePawn6, typeof(Pawn));
            Assert.AreEqual(whitePawn6?.Color, Color.White);

            Assert.IsNotNull(whitePawn7);
            Assert.IsInstanceOfType(whitePawn7, typeof(Pawn));
            Assert.AreEqual(whitePawn7?.Color, Color.White);

            Assert.IsNotNull(whitePawn8);
            Assert.IsInstanceOfType(whitePawn8, typeof(Pawn));
            Assert.AreEqual(whitePawn8?.Color, Color.White);
        }

        [TestMethod]
        public void NewInstance_GivenFenString_ShouldAddBlackPiecesCorrectly()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Coordinate A8 = Coordinate.GetInstance("A8");
            Coordinate B8 = Coordinate.GetInstance("B8");
            Coordinate C8 = Coordinate.GetInstance("C8");
            Coordinate D8 = Coordinate.GetInstance("D8");
            Coordinate E8 = Coordinate.GetInstance("E8");
            Coordinate F8 = Coordinate.GetInstance("F8");
            Coordinate G8 = Coordinate.GetInstance("G8");
            Coordinate H8 = Coordinate.GetInstance("H8");
            Coordinate A7 = Coordinate.GetInstance("A7");
            Coordinate B7 = Coordinate.GetInstance("B7");
            Coordinate C7 = Coordinate.GetInstance("C7");
            Coordinate D7 = Coordinate.GetInstance("D7");
            Coordinate E7 = Coordinate.GetInstance("E7");
            Coordinate F7 = Coordinate.GetInstance("F7");
            Coordinate G7 = Coordinate.GetInstance("G7");
            Coordinate H7 = Coordinate.GetInstance("H7");

            Piece? blackPawn1 = chessboard.GetSquare(A7).Piece;
            Piece? blackPawn2 = chessboard.GetSquare(B7).Piece;
            Piece? blackPawn3 = chessboard.GetSquare(C7).Piece;
            Piece? blackPawn4 = chessboard.GetSquare(D7).Piece;
            Piece? blackPawn5 = chessboard.GetSquare(E7).Piece;
            Piece? blackPawn6 = chessboard.GetSquare(F7).Piece;
            Piece? blackPawn7 = chessboard.GetSquare(G7).Piece;
            Piece? blackPawn8 = chessboard.GetSquare(H7).Piece;

            Piece? blackRook = chessboard.GetSquare(A8).Piece;
            Piece? blackKnight = chessboard.GetSquare(B8).Piece;
            Piece? blackBishop = chessboard.GetSquare(C8).Piece;
            Piece? blackQueen = chessboard.GetSquare(D8).Piece;
            Piece? blackKing = chessboard.GetSquare(E8).Piece;
            Piece? blackBishop2 = chessboard.GetSquare(F8).Piece;
            Piece? blackKnight2 = chessboard.GetSquare(G8).Piece;
            Piece? blackRook2 = chessboard.GetSquare(H8).Piece;

            Assert.IsNotNull(blackRook);
            Assert.IsInstanceOfType(blackRook, typeof(Rook));
            Assert.AreEqual(blackRook?.Color, Color.Black);

            Assert.IsNotNull(blackKnight);
            Assert.IsInstanceOfType(blackKnight, typeof(Knight));
            Assert.AreEqual(blackKnight?.Color, Color.Black);

            Assert.IsNotNull(blackBishop);
            Assert.IsInstanceOfType(blackBishop, typeof(Bishop));
            Assert.AreEqual(blackBishop?.Color, Color.Black);

            Assert.IsNotNull(blackQueen);
            Assert.IsInstanceOfType(blackQueen, typeof(Queen));
            Assert.AreEqual(blackQueen?.Color, Color.Black);

            Assert.IsNotNull(blackKing);
            Assert.IsInstanceOfType(blackKing, typeof(King));
            Assert.AreEqual(blackKing?.Color, Color.Black);

            Assert.IsNotNull(blackBishop2);
            Assert.IsInstanceOfType(blackBishop2, typeof(Bishop));
            Assert.AreEqual(blackBishop2?.Color, Color.Black);

            Assert.IsNotNull(blackKnight2);
            Assert.IsInstanceOfType(blackKnight2, typeof(Knight));
            Assert.AreEqual(blackKnight2?.Color, Color.Black);

            Assert.IsNotNull(blackRook2);
            Assert.IsInstanceOfType(blackRook2, typeof(Rook));
            Assert.AreEqual(blackRook2?.Color, Color.Black);

            Assert.IsNotNull(blackPawn1);
            Assert.IsInstanceOfType(blackPawn1, typeof(Pawn));
            Assert.AreEqual(blackPawn1?.Color, Color.Black);

            Assert.IsNotNull(blackPawn2);
            Assert.IsInstanceOfType(blackPawn2, typeof(Pawn));
            Assert.AreEqual(blackPawn2?.Color, Color.Black);

            Assert.IsNotNull(blackPawn3);
            Assert.IsInstanceOfType(blackPawn3, typeof(Pawn));
            Assert.AreEqual(blackPawn3?.Color, Color.Black);

            Assert.IsNotNull(blackPawn4);
            Assert.IsInstanceOfType(blackPawn4, typeof(Pawn));
            Assert.AreEqual(blackPawn4?.Color, Color.Black);

            Assert.IsNotNull(blackPawn5);
            Assert.IsInstanceOfType(blackPawn5, typeof(Pawn));
            Assert.AreEqual(blackPawn5?.Color, Color.Black);

            Assert.IsNotNull(blackPawn6);
            Assert.IsInstanceOfType(blackPawn6, typeof(Pawn));
            Assert.AreEqual(blackPawn6?.Color, Color.Black);

            Assert.IsNotNull(blackPawn7);
            Assert.IsInstanceOfType(blackPawn7, typeof(Pawn));
            Assert.AreEqual(blackPawn7?.Color, Color.Black);

            Assert.IsNotNull(blackPawn8);
            Assert.IsInstanceOfType(blackPawn8, typeof(Pawn));
            Assert.AreEqual(blackPawn8?.Color, Color.Black);
        }

        [TestMethod]
        public void NewInstance_InitialPosition_EmptySquares_ShouldReturnPieceAsNull()
        {
            Chessboard chessboard = new(FEN.InitialPosition);

            for (int row = 2; row <= 5; row++)
            {
                for (int col = 0; col <= 7; col++)
                {

                    Square square = chessboard.GetSquare(Coordinate.GetInstance(col, row));
                    Assert.IsNull(square.Piece);
                }
            }
        }

        [TestMethod]
        public void MovePiece_GivenOriginAndDestination_ShouldChangePiecePosition()
        {
            Chessboard chessboard = new("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1");
            Coordinate origin = Coordinate.GetInstance("B7");
            Coordinate destination = Coordinate.GetInstance("B3");

            chessboard.ChangePiecePosition(origin, destination);

            Assert.IsFalse(chessboard.GetSquare(origin).HasPiece);
            Assert.IsTrue(chessboard.GetSquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetSquare(destination).Piece, typeof(Rook));
        }

        [TestMethod]
        public void MovePiece_OriginWithEmptySquare_ShouldThrowException()
        {
            Chessboard chessboard = new("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1");
            Coordinate origin = Coordinate.GetInstance("A1");
            Coordinate destination = Coordinate.GetInstance("A2");

            Assert.ThrowsException<ChessboardException>(() => chessboard.ChangePiecePosition(origin, destination));
        }

        [TestMethod]
        public void MovePiece_GivenOriginAndDestinationWithEmptySquare_ShouldChangePiecPosition()
        {
            Chessboard chessboard = new("7k/1R6/7K/8/8/1b6/8/8 w - - 0 1");
            Coordinate origin = Coordinate.GetInstance("B7");
            Coordinate destination = Coordinate.GetInstance("B8");

            chessboard.ChangePiecePosition(origin, destination);

            Assert.IsFalse(chessboard.GetSquare(origin).HasPiece);
            Assert.IsTrue(chessboard.GetSquare(destination).HasPiece);
            Assert.IsInstanceOfType(chessboard.GetSquare(destination).Piece, typeof(Rook));
        }
    }
}