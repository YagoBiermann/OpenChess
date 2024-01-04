using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KnightTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            Knight whiteKnight = (Knight)chessboard.GetReadOnlySquare("B1").ReadOnlyPiece!;
            Knight blackKnight = (Knight)chessboard.GetReadOnlySquare("B8").ReadOnlyPiece!;

            Assert.IsFalse(whiteKnight.IsLongRange);
            Assert.IsFalse(blackKnight.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            Knight whiteKnight = (Knight)chessboard.GetReadOnlySquare("B1").ReadOnlyPiece!;
            Knight blackKnight = (Knight)chessboard.GetReadOnlySquare("B8").ReadOnlyPiece!;

            List<Direction> directions = new()
                {
                    new Direction(1,2),
                    new Direction(-1,2),
                    new Direction(1,-2),
                    new Direction(-1,-2),
                    new Direction(2,1),
                    new Direction(2,-1),
                    new Direction(-2,-1),
                    new Direction(-2,1)
                };


            CollectionAssert.AreEquivalent(directions, whiteKnight.Directions);
            CollectionAssert.AreEquivalent(directions, blackKnight.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new(new FenInfo("rnbqkbnr/pppppppp/8/8/4N3/8/PPPPPPPP/RNBQKB1R w KQkq - 0 1"));
            Knight knight = (Knight)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<PieceLineOfSight> expectedMoves = new()
            {
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(1,2), knight.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(-1,2), knight.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(1,-2), knight.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(-1,-2), knight.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(2,1), knight.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(2,-1), knight.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(-2,-1), knight.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, knight, new Direction(-2,1), knight.MoveAmount),
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceLineOfSight> moves = moveCalculator.CalculateLineOfSight(knight);

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
            Chessboard chessboard = new(new FenInfo("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1"));
            Knight knight = (Knight)chessboard.GetReadOnlySquare("G4").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(knight);

            List<Coordinate> upperLeftMoves = moves.Find(m => m.Direction.Equals(new Direction(-1, 2))).RangeOfAttack;
            List<Coordinate> upperLeftMoves2 = moves.Find(m => m.Direction.Equals(new Direction(-2, 1))).RangeOfAttack;

            List<Coordinate> expectedUpperLeftMove1 = new() { Coordinate.GetInstance("F6") };
            List<Coordinate> expectedUpperLeftMove2 = new() { Coordinate.GetInstance("E5") };

            CollectionAssert.AreEqual(upperLeftMoves, expectedUpperLeftMove1);
            CollectionAssert.AreEqual(upperLeftMoves2, expectedUpperLeftMove2);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldIncludeAllyPieces()
        {
            Chessboard chessboard = new(new FenInfo("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1"));
            Knight knight = (Knight)chessboard.GetReadOnlySquare("G4").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(knight);
            var lowerRightMove = moves.Find(m => m.Direction.Equals(new Direction(1, -2)));

            Assert.IsTrue(lowerRightMove.RangeOfAttack.Any());
            Assert.IsNotNull(lowerRightMove.NearestPiece);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new(new FenInfo("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1"));
            Knight knight = (Knight)chessboard.GetReadOnlySquare("G4").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(knight);
            List<Coordinate> lowerLeftMove = moves.Find(m => m.Direction.Equals(new Direction(-1, -2))).RangeOfAttack;
            List<Coordinate> expectedLowerLeftMove = new() { Coordinate.GetInstance("F2") };

            CollectionAssert.AreEqual(lowerLeftMove, expectedLowerLeftMove);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_PositionOutOfChessboard_ShouldReturnEmptyList()
        {
            Chessboard chessboard = new(new FenInfo("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1"));
            Knight knight = (Knight)chessboard.GetReadOnlySquare("G4").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(knight);

            Assert.IsFalse(moves.Find(m => m.Direction.Equals(new Direction(2, 1))).RangeOfAttack.Any());
            Assert.IsFalse(moves.Find(m => m.Direction.Equals(new Direction(2, -1))).RangeOfAttack.Any());
        }
    }
}