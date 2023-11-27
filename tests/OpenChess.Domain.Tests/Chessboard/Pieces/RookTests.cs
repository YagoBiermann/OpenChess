using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class RookTests
    {
        [TestMethod]
        public void NameProperty_BlackRook_ShouldBeLowercaseR()
        {
            Rook rook = new(Color.Black, Coordinate.GetInstance("A1"));

            Assert.AreEqual(rook.Name, 'r');
        }

        [TestMethod]
        public void NameProperty_WhiteRook_ShouldBeUppercaseR()
        {
            Rook rook = new(Color.White, Coordinate.GetInstance("A1"));

            Assert.AreEqual(rook.Name, 'R');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Rook rook = new(Color.White, Coordinate.GetInstance("A1"));
            Rook rook2 = new(Color.Black, Coordinate.GetInstance("A1"));

            Assert.IsTrue(rook.IsLongRange);
            Assert.IsTrue(rook2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnUpDownLeftRight()
        {
            Rook rook = new(Color.White, Coordinate.GetInstance("A1"));
            Rook rook2 = new(Color.Black, Coordinate.GetInstance("A1"));

            List<Direction> directions = new()
            {
                new Up(),
                new Down(),
                new Left(),
                new Right(),
            };

            CollectionAssert.AreEquivalent(directions, rook.Directions);
            CollectionAssert.AreEquivalent(directions, rook2.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Rook rook = new(Color.White, Coordinate.GetInstance("E4"));

            List<Move> expectedMoves = new()
            {
                ExpectedMoves.GetMove(rook.Origin, new Up(), rook.MoveAmount),
                ExpectedMoves.GetMove(rook.Origin, new Down(), rook.MoveAmount),
                ExpectedMoves.GetMove(rook.Origin, new Left(), rook.MoveAmount),
                ExpectedMoves.GetMove(rook.Origin, new Right(), rook.MoveAmount)
            };

            List<Move> moves = rook.CalculateMoveRange();

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
            Chessboard chessboard = new("8/8/5K2/8/1RR2r1p/8/5k2/8 b - - 0 1");
            Rook rook = (Rook)chessboard.GetSquare(Coordinate.GetInstance("F4")).Piece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("E4"),
                Coordinate.GetInstance("D4"),
                Coordinate.GetInstance("C4"),
            };

            List<Move> moves = rook.CalculateLegalMoves(chessboard);
            List<Coordinate> leftMoves = moves.Find(m => m.Direction.Equals(new Left())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, leftMoves);
        }
    }
}