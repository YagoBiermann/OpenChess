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
    }
}