using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        public void NameProperty_BlackKing_ShouldBeLowercaseK()
        {
            King king = new(Color.Black, Coordinate.GetInstance("E8"));

            Assert.AreEqual(king.Name, 'k');
        }
        [TestMethod]
        public void NameProperty_WhiteKing_ShouldBeUppercaseK()
        {
            King king = new(Color.White, Coordinate.GetInstance("E1"));

            Assert.AreEqual(king.Name, 'K');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            King king = new(Color.White, Coordinate.GetInstance("E1"));
            King king2 = new(Color.Black, Coordinate.GetInstance("E8"));

            Assert.IsFalse(king.IsLongRange);
            Assert.IsFalse(king2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            King king = new(Color.White, Coordinate.GetInstance("E1"));
            King king2 = new(Color.Black, Coordinate.GetInstance("E8"));

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

            CollectionAssert.AreEquivalent(directions, king.Directions);
            CollectionAssert.AreEquivalent(directions, king2.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            King king = new(Color.White, Coordinate.GetInstance("E4"));

            List<Move> expectedMoves = new()
            {
                ExpectedMoves.GetMove(king.Origin, new Up(), king.MoveAmount),
                ExpectedMoves.GetMove(king.Origin, new Down(), king.MoveAmount),
                ExpectedMoves.GetMove(king.Origin, new Left(), king.MoveAmount),
                ExpectedMoves.GetMove(king.Origin, new Right(), king.MoveAmount),
                ExpectedMoves.GetMove(king.Origin, new UpperLeft(), king.MoveAmount),
                ExpectedMoves.GetMove(king.Origin, new UpperRight(), king.MoveAmount),
                ExpectedMoves.GetMove(king.Origin, new LowerLeft(), king.MoveAmount),
                ExpectedMoves.GetMove(king.Origin, new LowerRight(), king.MoveAmount),
            };

            List<Move> moves = king.CalculateMoveRange();

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