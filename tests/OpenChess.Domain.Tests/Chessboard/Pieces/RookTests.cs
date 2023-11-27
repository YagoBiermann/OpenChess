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

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/8/5K2/8/1RR2r1p/8/5k2/8 b - - 0 1");
            Rook rook = (Rook)chessboard.GetSquare(Coordinate.GetInstance("F4")).Piece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("G4"),
            };

            List<Move> moves = rook.CalculateLegalMoves(chessboard);
            List<Coordinate> rightMoves = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, rightMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("8/8/5K2/8/1RR2r1p/8/5k2/8 b - - 0 1");
            Rook rook = (Rook)chessboard.GetSquare(Coordinate.GetInstance("F4")).Piece!;
            List<Coordinate> expectedUpMove = new() { Coordinate.GetInstance("F5") };
            List<Coordinate> expectedDownMove = new() { Coordinate.GetInstance("F3") };

            List<Move> moves = rook.CalculateLegalMoves(chessboard);
            List<Coordinate> upMoves = moves.Find(m => m.Direction.Equals(new Up())).Coordinates;
            List<Coordinate> downMoves = moves.Find(m => m.Direction.Equals(new Down())).Coordinates;

            CollectionAssert.AreEqual(expectedUpMove, upMoves);
            CollectionAssert.AreEqual(expectedDownMove, downMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("8/8/4K3/8/1RR1r3/8/4k3/8 b - - 0 1");
            Rook rook = (Rook)chessboard.GetSquare(Coordinate.GetInstance("E4")).Piece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("F4"),
                Coordinate.GetInstance("G4"),
                Coordinate.GetInstance("H4"),
            };

            List<Move> moves = rook.CalculateLegalMoves(chessboard);
            List<Coordinate> rightMoves = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, rightMoves);
        }
    }
}