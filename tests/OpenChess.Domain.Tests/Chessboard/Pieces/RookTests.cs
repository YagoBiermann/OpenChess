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

            foreach (Move move in moves)
            {
                int index = moves.IndexOf(move);
                CollectionAssert.AreEqual(expectedMoves[index].Coordinates, move.Coordinates);
                Assert.AreEqual(expectedMoves[index].Direction, move.Direction);
            }

        }
    }
}