using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class QueenTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D1").ReadOnlyPiece!;
            Queen queen2 = (Queen)chessboard.GetReadOnlySquare("D8").ReadOnlyPiece!;

            Assert.IsTrue(queen.IsLongRange);
            Assert.IsTrue(queen2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D1").ReadOnlyPiece!;
            Queen queen2 = (Queen)chessboard.GetReadOnlySquare("D8").ReadOnlyPiece!;

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
            Chessboard chessboard = new("rnbqkbnr/pppppppp/8/8/4Q3/8/PPPPPPPP/RNB1KBNR w KQkq - 0 1");
            Queen queen = (Queen)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<MoveDirections> expectedMoves = new()
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

            List<MoveDirections> moves = queen.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (MoveDirections move in moves)
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
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<MoveDirections> moves = legalMoves.CalculateMoves(queen);

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
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<MoveDirections> moves = legalMoves.CalculateMoves(queen);

            List<Coordinate> leftMoves = moves.Find(m => m.Direction.Equals(new Left())).Coordinates;
            List<Coordinate> upperRightMoves = moves.Find(m => m.Direction.Equals(new UpperRight())).Coordinates;

            List<Coordinate> expectedLeftMovesMoves = new() { Coordinate.GetInstance("C4") };
            List<Coordinate> expectedUpperRightMoves = new() { Coordinate.GetInstance("E5") };

            CollectionAssert.AreEqual(expectedLeftMovesMoves, leftMoves);
            CollectionAssert.AreEqual(expectedUpperRightMoves, upperRightMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("8/8/1Q1K1b2/8/1r1q2R1/4P3/1k6/8 w - - 0 1");
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<MoveDirections> moves = legalMoves.CalculateMoves(queen);

            List<Coordinate> lowerLeftMoves = moves.Find(m => m.Direction.Equals(new LowerLeft())).Coordinates;
            List<Coordinate> upMoves = moves.Find(m => m.Direction.Equals(new Up())).Coordinates;

            List<Coordinate> expectedLowerLeftMoves = new() { Coordinate.GetInstance("C3") };
            List<Coordinate> expectedUpMoves = new() { Coordinate.GetInstance("D5") };

            CollectionAssert.AreEqual(expectedLowerLeftMoves, lowerLeftMoves);
            CollectionAssert.AreEqual(expectedUpMoves, upMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("8/8/1Q1K1b2/8/1r1q2R1/4P3/1k6/8 w - - 0 1");
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<MoveDirections> moves = legalMoves.CalculateMoves(queen);
            List<Coordinate> downMoves = moves.Find(m => m.Direction.Equals(new Down())).Coordinates;
            List<Coordinate> expectedDownMoves = new() { Coordinate.GetInstance("D3"), Coordinate.GetInstance("D2"), Coordinate.GetInstance("D1"), };

            CollectionAssert.AreEqual(expectedDownMoves, downMoves);
        }
    }
}