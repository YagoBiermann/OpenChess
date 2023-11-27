using System.Collections.ObjectModel;
using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class QueenTests
    {
        [TestMethod]
        public void NameProperty_BlackQueen_ShouldBeLowercaseQ()
        {
            Queen queen = new(Color.Black, Coordinate.GetInstance("D8"));

            Assert.AreEqual(queen.Name, 'q');
        }

        [TestMethod]
        public void NameProperty_WhiteQueen_ShouldBeUppercaseQ()
        {
            Queen queen = new(Color.White, Coordinate.GetInstance("D1"));

            Assert.AreEqual(queen.Name, 'Q');
        }

        [TestMethod]
        public void IsLongeRangeProperty_ShouldBeTrue()
        {
            Queen queen = new(Color.White, Coordinate.GetInstance("D1"));
            Queen queen2 = new(Color.Black, Coordinate.GetInstance("D1"));

            Assert.IsTrue(queen.IsLongRange);
            Assert.IsTrue(queen2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            Queen queen = new(Color.White, Coordinate.GetInstance("D1"));
            Queen queen2 = new(Color.Black, Coordinate.GetInstance("D1"));

            List<Direction> directions = new()
            {
                new Up(),
                new Down(),
                new Left(),
                new Right(),
                new UpperLeft(),
                new UpperRight(),
                new LowerLeft(),
                new LowerRight()
            };

            CollectionAssert.AreEquivalent(directions, queen.Directions);
            CollectionAssert.AreEquivalent(directions, queen2.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Queen queen = new(Color.White, Coordinate.GetInstance("E4"));

            List<Move> expectedMoves = new()
            {
                ExpectedMoves.GetMove(queen.Origin, new Up(), queen.MoveAmount),
                ExpectedMoves.GetMove(queen.Origin, new Down(), queen.MoveAmount),
                ExpectedMoves.GetMove(queen.Origin, new Left(), queen.MoveAmount),
                ExpectedMoves.GetMove(queen.Origin, new Right(), queen.MoveAmount),
                ExpectedMoves.GetMove(queen.Origin, new UpperLeft(), queen.MoveAmount),
                ExpectedMoves.GetMove(queen.Origin, new UpperRight(), queen.MoveAmount),
                ExpectedMoves.GetMove(queen.Origin, new LowerLeft(), queen.MoveAmount),
                ExpectedMoves.GetMove(queen.Origin, new LowerRight(), queen.MoveAmount),
            };

            List<Move> moves = queen.CalculateMoveRange();

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
            Chessboard chessboard = new("8/8/1Q1K1b2/8/1r1q2R1/4P3/1k6/8 w - - 0 1");
            Queen queen = (Queen)chessboard.GetSquare(Coordinate.GetInstance("D4")).Piece!;
            List<Move> moves = queen.CalculateLegalMoves(chessboard);

            List<Coordinate> rightMoves = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;
            List<Coordinate> upperLeftMoves = moves.Find(m => m.Direction.Equals(new UpperLeft())).Coordinates;
            List<Coordinate> lowerRightMoves = moves.Find(m => m.Direction.Equals(new LowerRight())).Coordinates;

            List<Coordinate> expectedRightMoves = new() { Coordinate.GetInstance("E4"), Coordinate.GetInstance("F4"), Coordinate.GetInstance("G4"), };
            List<Coordinate> expectedUpperLeftMoves = new() { Coordinate.GetInstance("C5"), Coordinate.GetInstance("B6"), };
            List<Coordinate> expectedLowerRightMoves = new() { Coordinate.GetInstance("E3"), };

            CollectionAssert.AreEqual(expectedRightMoves, rightMoves);
            CollectionAssert.AreEqual(expectedUpperLeftMoves, upperLeftMoves);
            CollectionAssert.AreEqual(expectedLowerRightMoves, lowerRightMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/8/1Q1K1b2/8/1r1q2R1/4P3/1k6/8 w - - 0 1");
            Queen queen = (Queen)chessboard.GetSquare(Coordinate.GetInstance("D4")).Piece!;
            List<Move> moves = queen.CalculateLegalMoves(chessboard);

            List<Coordinate> leftMoves = moves.Find(m => m.Direction.Equals(new Left())).Coordinates;
            List<Coordinate> upperRightMoves = moves.Find(m => m.Direction.Equals(new UpperRight())).Coordinates;

            List<Coordinate> expectedLeftMovesMoves = new() { Coordinate.GetInstance("C4") };
            List<Coordinate> expectedUpperRightMoves = new() { Coordinate.GetInstance("E5") };

            CollectionAssert.AreEqual(expectedLeftMovesMoves, leftMoves);
            CollectionAssert.AreEqual(expectedUpperRightMoves, upperRightMoves);
        }
    }
}