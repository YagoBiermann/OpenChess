using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class BishopTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Bishop whiteBishop = (Bishop)chessboard.GetSquare("C1").Piece!;
            Bishop blackBishop = (Bishop)chessboard.GetSquare("C8").Piece!;

            Assert.IsTrue(whiteBishop.IsLongRange);
            Assert.IsTrue(blackBishop.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnDiagonalDirections()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Bishop whiteBishop = (Bishop)chessboard.GetSquare("C1").Piece!;
            Bishop blackBishop = (Bishop)chessboard.GetSquare("C8").Piece!;

            List<Direction> directions = new()
            {
                new UpperLeft(),
                new UpperRight(),
                new LowerLeft(),
                new LowerRight()
            };

            CollectionAssert.AreEquivalent(directions, whiteBishop.Directions);
            CollectionAssert.AreEquivalent(directions, blackBishop.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new("r1bqk1nr/pppp2p1/5p1p/4P1N1/4B1Q1/8/PPP2PPP/RN2K2R b KQkq - 0 1");
            Bishop bishop = (Bishop)chessboard.GetSquare("E4").Piece!;

            List<MovePositions> expectedMoves = new()
            {
                ExpectedMoves.GetMove(bishop.Origin, new UpperLeft(), bishop.MoveAmount),
                ExpectedMoves.GetMove(bishop.Origin, new UpperRight(), bishop.MoveAmount),
                ExpectedMoves.GetMove(bishop.Origin, new LowerLeft(), bishop.MoveAmount),
                ExpectedMoves.GetMove(bishop.Origin, new LowerRight(), bishop.MoveAmount),
            };

            List<MovePositions> moves = bishop.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (MovePositions move in moves)
            {
                int index = moves.IndexOf(move);
                CollectionAssert.AreEqual(expectedMoves[index].Coordinates, move.Coordinates);
                Assert.AreEqual(expectedMoves[index].Direction, move.Direction);
            }
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldIncludeEnemyPieces()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetSquare("C5").Piece!;
            List<MovePositions> moves = bishop.CalculateLegalMoves();

            List<Coordinate> lowerRightMove = moves.Find(m => m.Direction.Equals(new LowerRight())).Coordinates;
            List<Coordinate> expectedLowerRightMove = new() { Coordinate.GetInstance("D4"), Coordinate.GetInstance("E3"), Coordinate.GetInstance("F2"), };

            CollectionAssert.AreEqual(expectedLowerRightMove, lowerRightMove);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetSquare("C5").Piece!;
            List<MovePositions> moves = bishop.CalculateLegalMoves();

            List<Coordinate> lowerLeftMove = moves.Find(m => m.Direction.Equals(new LowerLeft())).Coordinates;
            List<Coordinate> expectedLowerLeftMovesMove = new() { Coordinate.GetInstance("B4") };

            CollectionAssert.AreEqual(expectedLowerLeftMovesMove, lowerLeftMove);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetSquare("C5").Piece!;
            List<MovePositions> moves = bishop.CalculateLegalMoves();

            List<Coordinate> upperRightMove = moves.Find(m => m.Direction.Equals(new UpperRight())).Coordinates;
            List<Coordinate> expectedUpperRightMove = new() { Coordinate.GetInstance("D6") };

            CollectionAssert.AreEqual(expectedUpperRightMove, upperRightMove);
        }

        [TestMethod]
        public void CalculateLegalMoves_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetSquare("C5").Piece!;

            List<Coordinate> upperLeftMove = bishop.CalculateLegalMoves().Find(m => m.Direction.Equals(new UpperLeft())).Coordinates;
            List<Coordinate> expectedUpperLeftMoves = new() { Coordinate.GetInstance("B6"), Coordinate.GetInstance("A7") };

            CollectionAssert.AreEqual(expectedUpperLeftMoves, upperLeftMove);
        }
    }
}