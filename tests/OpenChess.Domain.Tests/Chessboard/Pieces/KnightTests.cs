using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KnightTests
    {
        [TestMethod]
        public void NameProperty_BlackKnight_ShouldBeLowercaseN()
        {
            Knight knight = new(Color.Black, Coordinate.GetInstance("B8"));

            Assert.AreEqual(knight.Name, 'n');
        }
        [TestMethod]
        public void NameProperty_WhiteKnight_ShouldBeUppercaseN()
        {
            Knight knight = new(Color.White, Coordinate.GetInstance("B1"));

            Assert.AreEqual(knight.Name, 'N');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            Knight knight = new(Color.White, Coordinate.GetInstance("B1"));
            Knight knight2 = new(Color.Black, Coordinate.GetInstance("B8"));

            Assert.IsFalse(knight.IsLongRange);
            Assert.IsFalse(knight2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            Knight knight = new(Color.White, Coordinate.GetInstance("B1"));
            Knight knight2 = new(Color.Black, Coordinate.GetInstance("B8"));

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


            CollectionAssert.AreEquivalent(directions, knight.Directions);
            CollectionAssert.AreEquivalent(directions, knight2.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Knight knight = new(Color.White, Coordinate.GetInstance("E4"));

            List<Move> expectedMoves = new()
            {
                ExpectedMoves.GetMove(knight.Origin, new Direction(1,2), knight.MoveAmount),
                ExpectedMoves.GetMove(knight.Origin, new Direction(-1,2), knight.MoveAmount),
                ExpectedMoves.GetMove(knight.Origin, new Direction(1,-2), knight.MoveAmount),
                ExpectedMoves.GetMove(knight.Origin, new Direction(-1,-2), knight.MoveAmount),
                ExpectedMoves.GetMove(knight.Origin, new Direction(2,1), knight.MoveAmount),
                ExpectedMoves.GetMove(knight.Origin, new Direction(2,-1), knight.MoveAmount),
                ExpectedMoves.GetMove(knight.Origin, new Direction(-2,-1), knight.MoveAmount),
                ExpectedMoves.GetMove(knight.Origin, new Direction(-2,1), knight.MoveAmount),
            };

            List<Move> moves = knight.CalculateMoveRange();

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
            Chessboard chessboard = new("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1");
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("G4")).Piece!;
            List<Move> moves = knight.CalculateLegalMoves(chessboard);
            List<Coordinate> upperLeftMoves = moves.Find(m => m.Direction.Equals(new Direction(-1, 2))).Coordinates;
            List<Coordinate> upperLeftMoves2 = moves.Find(m => m.Direction.Equals(new Direction(-2, 1))).Coordinates;

            List<Coordinate> expectedUpperLeftMove1 = new() { Coordinate.GetInstance("F6") };
            List<Coordinate> expectedUpperLeftMove2 = new() { Coordinate.GetInstance("E5") };

            CollectionAssert.AreEqual(upperLeftMoves, expectedUpperLeftMove1);
            CollectionAssert.AreEqual(upperLeftMoves2, expectedUpperLeftMove2);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1");
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("G4")).Piece!;
            List<Move> moves = knight.CalculateLegalMoves(chessboard);
            List<Coordinate> lowerRightMove = moves.Find(m => m.Direction.Equals(new Direction(1, -2))).Coordinates;

            Assert.IsFalse(lowerRightMove.Any());
        }
    }
}