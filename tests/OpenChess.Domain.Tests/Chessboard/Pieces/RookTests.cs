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

            List<Coordinate> up = new()
            {
                Coordinate.GetInstance("E5"),
                Coordinate.GetInstance("E6"),
                Coordinate.GetInstance("E7"),
                Coordinate.GetInstance("E8")
            };
            List<Coordinate> down = new()
            {
                Coordinate.GetInstance("E3"),
                Coordinate.GetInstance("E2"),
                Coordinate.GetInstance("E1")
            };
            List<Coordinate> left = new()
            {
                Coordinate.GetInstance("D4"),
                Coordinate.GetInstance("C4"),
                Coordinate.GetInstance("B4"),
                Coordinate.GetInstance("A4")
            };
            List<Coordinate> right = new()
            {
                Coordinate.GetInstance("F4"),
                Coordinate.GetInstance("G4"),
                Coordinate.GetInstance("H4")
            };

            List<Move> expectedMoves = new()
            {
                new(new Up(), up),
                new(new Down(), down),
                new(new Left(), left),
                new(new Right(), right),
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