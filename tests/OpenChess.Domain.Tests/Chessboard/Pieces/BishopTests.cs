using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class BishopTests
    {
        [TestMethod]
        public void NameProperty_BlackBishop_ShouldBeLowercaseB()
        {
            Bishop bishop = new(Color.Black, Coordinate.GetInstance("C8"));

            Assert.AreEqual(bishop.Name, 'b');
        }
        [TestMethod]
        public void NameProperty_WhiteBishop_ShouldBeUppercaseB()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("C1"));

            Assert.AreEqual(bishop.Name, 'B');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("C1"));
            Bishop bishop2 = new(Color.Black, Coordinate.GetInstance("C1"));

            Assert.IsTrue(bishop.IsLongRange);
            Assert.IsTrue(bishop2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnDiagonalDirections()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("C1"));
            Bishop bishop2 = new(Color.Black, Coordinate.GetInstance("C1"));

            List<Direction> directions = new()
            {
                new UpperLeft(),
                new UpperRight(),
                new LowerLeft(),
                new LowerRight()
            };

            CollectionAssert.AreEquivalent(directions, bishop.Directions);
            CollectionAssert.AreEquivalent(directions, bishop2.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Bishop bishop = new(Color.White, Coordinate.GetInstance("E4"));

            List<Move> expectedMoves = new()
            {
                ExpectedMoves.GetMove(bishop.Origin, new UpperLeft(), bishop.MoveAmount),
                ExpectedMoves.GetMove(bishop.Origin, new UpperRight(), bishop.MoveAmount),
                ExpectedMoves.GetMove(bishop.Origin, new LowerLeft(), bishop.MoveAmount),
                ExpectedMoves.GetMove(bishop.Origin, new LowerRight(), bishop.MoveAmount),
            };

            List<Move> moves = bishop.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (Move move in moves)
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
            Bishop bishop = (Bishop)chessboard.GetSquare(Coordinate.GetInstance("C5")).Piece!;
            List<Move> moves = bishop.CalculateLegalMoves(chessboard);

            List<Coordinate> lowerRightMove = moves.Find(m => m.Direction.Equals(new LowerRight())).Coordinates;
            List<Coordinate> expectedLowerRightMove = new() { Coordinate.GetInstance("D4"), Coordinate.GetInstance("E3"), Coordinate.GetInstance("F2"), };

            CollectionAssert.AreEqual(expectedLowerRightMove, lowerRightMove);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetSquare(Coordinate.GetInstance("C5")).Piece!;
            List<Move> moves = bishop.CalculateLegalMoves(chessboard);

            List<Coordinate> lowerLeftMove = moves.Find(m => m.Direction.Equals(new LowerLeft())).Coordinates;
            List<Coordinate> expectedLowerLeftMovesMove = new() { Coordinate.GetInstance("B4") };

            CollectionAssert.AreEqual(expectedLowerLeftMovesMove, lowerLeftMove);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetSquare(Coordinate.GetInstance("C5")).Piece!;
            List<Move> moves = bishop.CalculateLegalMoves(chessboard);

            List<Coordinate> upperRightMove = moves.Find(m => m.Direction.Equals(new UpperRight())).Coordinates;
            List<Coordinate> expectedUpperRightMove = new() { Coordinate.GetInstance("D6") };

            CollectionAssert.AreEqual(expectedUpperRightMove, upperRightMove);
        }
    }
}