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

        [TestMethod]
        public void CalculateLegalMoves_ShouldIncludeEnemyPieces()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;

            List<Coordinate> moves = king.CalculateLegalMoves(chessboard).Find(m => m.Direction.Equals(new UpperRight())).Coordinates;
            List<Coordinate> expectedMove = new() { Coordinate.GetInstance("F2") };

            CollectionAssert.AreEqual(expectedMove, moves);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;

            List<Coordinate> moves = king.CalculateLegalMoves(chessboard).Find(m => m.Direction.Equals(new UpperLeft())).Coordinates;

            Assert.IsFalse(moves.Any());
        }
    }
}