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
        public void CalculateLineOfSight_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new("rnbqkbnr/pppppppp/8/8/4Q3/8/PPPPPPPP/RNB1KBNR w KQkq - 0 1");
            Queen queen = (Queen)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<PieceLineOfSight> expectedMoves = new()
            {
                ExpectedMoves.GetLineOfSight(chessboard, queen, new Up(), queen.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, queen, new Down(), queen.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, queen, new Left(), queen.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, queen, new Right(), queen.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, queen, new UpperLeft(), queen.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, queen, new UpperRight(), queen.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, queen, new LowerLeft(), queen.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, queen, new LowerRight(), queen.MoveAmount),
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceLineOfSight> moves = moveCalculator.CalculateLineOfSight(queen);

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (PieceLineOfSight move in moves)
            {
                int index = moves.IndexOf(move);
                CollectionAssert.AreEqual(expectedMoves[index].LineOfSight, move.LineOfSight);
                Assert.AreEqual(expectedMoves[index].Direction, move.Direction);
            }
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldIncludeEnemyPieces()
        {
            Chessboard chessboard = new("8/8/1Q1K1b2/8/1r1q2R1/4P3/1k6/8 w - - 0 1");
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(queen);

            var rightMoves = moves.Find(m => m.Direction is Right);
            var upperLeftMoves = moves.Find(m => m.Direction is UpperLeft);
            var lowerRightMoves = moves.Find(m => m.Direction is LowerRight);

            List<Coordinate> expectedRightMoves = new() { Coordinate.GetInstance("E4"), Coordinate.GetInstance("F4"), Coordinate.GetInstance("G4"), };
            List<Coordinate> expectedUpperLeftMoves = new() { Coordinate.GetInstance("C5"), Coordinate.GetInstance("B6"), };
            List<Coordinate> expectedLowerRightMoves = new() { Coordinate.GetInstance("E3"), };

            CollectionAssert.AreEqual(expectedRightMoves, rightMoves.RangeOfAttack);
            CollectionAssert.AreEqual(expectedUpperLeftMoves, upperLeftMoves.RangeOfAttack);
            CollectionAssert.AreEqual(expectedLowerRightMoves, lowerRightMoves.RangeOfAttack);

            Assert.IsNotNull(rightMoves.NearestPiece);
            Assert.IsNotNull(upperLeftMoves.NearestPiece);
            Assert.IsNotNull(lowerRightMoves.NearestPiece);
            Assert.AreEqual(ColorUtils.GetOppositeColor(rightMoves.Piece.Color), rightMoves.NearestPiece.Color);
            Assert.AreEqual(ColorUtils.GetOppositeColor(rightMoves.Piece.Color), upperLeftMoves.NearestPiece.Color);
            Assert.AreEqual(ColorUtils.GetOppositeColor(rightMoves.Piece.Color), lowerRightMoves.NearestPiece.Color);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/8/1Q1K1b2/8/1r1q2R1/4P3/1k6/8 w - - 0 1");
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(queen);

            var leftMoves = moves.Find(m => m.Direction is Left);
            var upperRightMoves = moves.Find(m => m.Direction is UpperRight);

            List<Coordinate> expectedLeftMovesMoves = new() { Coordinate.GetInstance("C4") };
            List<Coordinate> expectedUpperRightMoves = new() { Coordinate.GetInstance("E5") };

            CollectionAssert.AreEqual(expectedLeftMovesMoves, leftMoves.RangeOfAttack);
            CollectionAssert.AreEqual(expectedUpperRightMoves, upperRightMoves.RangeOfAttack);
            Assert.IsNull(leftMoves.NearestPiece);
            Assert.IsNull(upperRightMoves.NearestPiece);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("8/8/1Q1K1b2/8/1r1q2R1/4P3/1k6/8 w - - 0 1");
            Queen queen = (Queen)chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(queen);
            var downMoves = moves.Find(m => m.Direction is Down);
            List<Coordinate> expectedDownMoves = new() { Coordinate.GetInstance("D3"), Coordinate.GetInstance("D2"), Coordinate.GetInstance("D1"), };

            CollectionAssert.AreEqual(expectedDownMoves, downMoves.RangeOfAttack);
            Assert.IsNull(downMoves.NearestPiece);
        }
    }
}