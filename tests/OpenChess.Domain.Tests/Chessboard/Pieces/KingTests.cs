using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KingTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            King whiteKing = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;
            King blackKing = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

            Assert.IsFalse(whiteKing.IsLongRange);
            Assert.IsFalse(blackKing.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            King whiteKing = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;
            King blackKing = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

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
            Chessboard chessboard = new("r1bqk1nr/pppp2p1/5p1p/1B2P1N1/4K1Q1/8/PPP2PPP/RN5R b KQkq - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<MoveDirections> expectedMoves = new()
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

            List<MoveDirections> moves = king.CalculateMoveRange();

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
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<Coordinate> moves = legalMoves.CalculateMoves(king).Find(m => m.Direction.Equals(new UpperRight())).Coordinates;
            List<Coordinate> expectedMove = new() { Coordinate.GetInstance("F2") };

            CollectionAssert.AreEqual(expectedMove, moves);
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<Coordinate> moves = legalMoves.CalculateMoves(king).Find(m => m.Direction.Equals(new UpperLeft())).Coordinates;

            Assert.IsFalse(moves.Any());
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("8/8/8/8/3k4/3K4/8/8 w - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("D3").ReadOnlyPiece!;

            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<Coordinate> up = legalMoves.CalculateMoves(king).Find(m => m.Direction.Equals(new Up())).Coordinates;

            Assert.IsFalse(up.Any());
        }

        [TestMethod]
        public void CalculateLegalMoves_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<MoveDirections> moves = legalMoves.CalculateMoves(king);
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