using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class RookTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Rook rook = (Rook)chessboard.GetReadOnlySquare("A1").ReadOnlyPiece!;
            Rook rook2 = (Rook)chessboard.GetReadOnlySquare("A8").ReadOnlyPiece!;

            Assert.IsTrue(rook.IsLongRange);
            Assert.IsTrue(rook2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnUpDownLeftRight()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Rook rook = (Rook)chessboard.GetReadOnlySquare("A1").ReadOnlyPiece!;
            Rook rook2 = (Rook)chessboard.GetReadOnlySquare("A8").ReadOnlyPiece!;

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
            Chessboard chessboard = new("rnbqkbnr/pppppppp/8/8/4R3/8/PPPPPPPP/RNB1KBNR w KQkq - 0 1");
            Rook rook = (Rook)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<MoveDirections> expectedMoves = new()
            {
                ExpectedMoves.GetMove(rook.Origin, new Up(), rook.MoveAmount),
                ExpectedMoves.GetMove(rook.Origin, new Down(), rook.MoveAmount),
                ExpectedMoves.GetMove(rook.Origin, new Left(), rook.MoveAmount),
                ExpectedMoves.GetMove(rook.Origin, new Right(), rook.MoveAmount)
            };

            List<MoveDirections> moves = rook.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (MoveDirections move in moves)
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
            Rook rook = (Rook)chessboard.GetReadOnlySquare("F4").ReadOnlyPiece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("E4"),
                Coordinate.GetInstance("D4"),
                Coordinate.GetInstance("C4"),
            };

            List<MoveDirections> moves = rook.CalculateLegalMoves();
            List<Coordinate> leftMoves = moves.Find(m => m.Direction.Equals(new Left())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, leftMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/8/5K2/8/1RR2r1p/8/5k2/8 b - - 0 1");
            Rook rook = (Rook)chessboard.GetReadOnlySquare("F4").ReadOnlyPiece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("G4"),
            };

            List<MoveDirections> moves = rook.CalculateLegalMoves();
            List<Coordinate> rightMoves = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, rightMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("8/8/5K2/8/1RR2r1p/8/5k2/8 b - - 0 1");
            Rook rook = (Rook)chessboard.GetReadOnlySquare("F4").ReadOnlyPiece!;
            List<Coordinate> expectedUpMove = new() { Coordinate.GetInstance("F5") };
            List<Coordinate> expectedDownMove = new() { Coordinate.GetInstance("F3") };

            List<MoveDirections> moves = rook.CalculateLegalMoves();
            List<Coordinate> upMoves = moves.Find(m => m.Direction.Equals(new Up())).Coordinates;
            List<Coordinate> downMoves = moves.Find(m => m.Direction.Equals(new Down())).Coordinates;

            CollectionAssert.AreEqual(expectedUpMove, upMoves);
            CollectionAssert.AreEqual(expectedDownMove, downMoves);
        }

        [TestMethod]
        public void CalculateLegalMoves_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("8/8/4K3/8/1RR1r3/8/4k3/8 b - - 0 1");
            Rook rook = (Rook)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("F4"),
                Coordinate.GetInstance("G4"),
                Coordinate.GetInstance("H4"),
            };

            List<MoveDirections> moves = rook.CalculateLegalMoves();
            List<Coordinate> rightMoves = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, rightMoves);
        }
    }
}