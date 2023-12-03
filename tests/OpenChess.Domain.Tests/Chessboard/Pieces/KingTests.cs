using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        public void NameProperty_BlackKing_ShouldBeLowercaseK()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("E8")).Piece!;

            Assert.AreEqual(king.Name, 'k');
        }
        [TestMethod]
        public void NameProperty_WhiteKing_ShouldBeUppercaseK()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;

            Assert.AreEqual(king.Name, 'K');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            King whiteKing = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;
            King blackKing = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;

            Assert.IsFalse(whiteKing.IsLongRange);
            Assert.IsFalse(blackKing.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            King whiteKing = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;
            King blackKing = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;

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

            CollectionAssert.AreEquivalent(directions, whiteKing.Directions);
            CollectionAssert.AreEquivalent(directions, blackKing.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new("r1bqk1nr/pppp2p1/5p1p/1B2P1N1/4K1Q1/8/PPP2PPP/RN5R b HAkq - 0 1");
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("E4")).Piece!;

            List<MovePositions> expectedMoves = new()
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

            List<MovePositions> moves = king.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (MovePositions move in moves)
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

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("8/8/8/8/3k4/3K4/8/8 w - - 0 1");
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("D3")).Piece!;

            List<MovePositions> moves = king.CalculateLegalMoves(chessboard);
            List<Coordinate> up = moves.Find(m => m.Direction.Equals(new Up())).Coordinates;

            Assert.IsFalse(up.Any());
        }

        [TestMethod]
        public void CalculateLegalMoves_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetSquare(Coordinate.GetInstance("E1")).Piece!;

            List<MovePositions> moves = king.CalculateLegalMoves(chessboard);
            List<Coordinate> up = moves.Find(m => m.Direction.Equals(new Up())).Coordinates;
            List<Coordinate> left = moves.Find(m => m.Direction.Equals(new Left())).Coordinates;
            List<Coordinate> right = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;
            List<Coordinate> down = moves.Find(m => m.Direction.Equals(new Down())).Coordinates;

            List<Coordinate> expectedUpMove = new() { Coordinate.GetInstance("E2") };
            List<Coordinate> expectedLeftMove = new() { Coordinate.GetInstance("D1") };
            List<Coordinate> expectedRightMove = new() { Coordinate.GetInstance("F1") };

            CollectionAssert.AreEqual(expectedUpMove, up);
            CollectionAssert.AreEqual(expectedLeftMove, left);
            CollectionAssert.AreEqual(expectedRightMove, right);
            Assert.IsFalse(down.Any());
        }
    }
}