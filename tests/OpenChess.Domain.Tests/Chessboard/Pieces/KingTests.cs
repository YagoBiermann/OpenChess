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
        public void CalculateLineOfSight_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new("r1bqk1nr/pppp2p1/5p1p/1B2P1N1/4K1Q1/8/PPP2PPP/RN5R b KQkq - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<PieceLineOfSight> expectedMoves = new()
            {
                ExpectedMoves.GetLineOfSight(chessboard, king, new Up(), king.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, king, new Down(), king.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, king, new Left(), king.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, king, new Right(), king.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, king, new UpperLeft(), king.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, king, new UpperRight(), king.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, king, new LowerLeft(), king.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, king, new LowerRight(), king.MoveAmount),
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceLineOfSight> moves = moveCalculator.CalculateLineOfSight(king);

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (PieceLineOfSight move in moves)
            {
                int index = moves.IndexOf(move);
                CollectionAssert.AreEqual(expectedMoves[index].LineOfSight, move.LineOfSight);
                Assert.AreEqual(expectedMoves[index].Direction, move.Direction);
            }
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldIncludeEnemyPieces()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            var moves = moveCalculator.CalculateRangeOfAttack(king).Find(m => m.Direction is UpperRight);

            List<Coordinate> expectedMove = new() { Coordinate.GetInstance("F2") };

            CollectionAssert.AreEqual(expectedMove, moves.RangeOfAttack);
            Assert.IsNotNull(moves.NearestPiece);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldNotIncludeAllyPieces()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/3r1P2/2q1k3 b - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            PieceRangeOfAttack move = moveCalculator.CalculateRangeOfAttack(king).Find(m => m.Direction is UpperLeft);

            Assert.IsNull(move.NearestPiece);
            Assert.IsFalse(move.RangeOfAttack.Any());
        }

        [TestMethod]
        public void CalculateRangeOfAttack_NoPiecesFound_ShouldReturnAllPositionsFromCurrentDirection()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/8/4k3 b - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("E1").ReadOnlyPiece!;

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(king);
            List<Coordinate> up = moves.Find(m => m.Direction is Up).RangeOfAttack;
            List<Coordinate> left = moves.Find(m => m.Direction is Left).RangeOfAttack;
            List<Coordinate> right = moves.Find(m => m.Direction is Right).RangeOfAttack;
            List<Coordinate> upperLeft = moves.Find(m => m.Direction is UpperLeft).RangeOfAttack;
            List<Coordinate> upperRight = moves.Find(m => m.Direction is UpperRight).RangeOfAttack;

            List<Coordinate> expectedUpMove = new() { Coordinate.GetInstance("E2") };
            List<Coordinate> expectedLeftMove = new() { Coordinate.GetInstance("D1") };
            List<Coordinate> expectedRightMove = new() { Coordinate.GetInstance("F1") };
            List<Coordinate> expectedUpperLeftMove = new() { Coordinate.GetInstance("D2") };
            List<Coordinate> expectedUpperRightMove = new() { Coordinate.GetInstance("F2") };

            CollectionAssert.AreEqual(expectedUpMove, up);
            CollectionAssert.AreEqual(expectedLeftMove, left);
            CollectionAssert.AreEqual(expectedRightMove, right);
            CollectionAssert.AreEqual(expectedUpperLeftMove, upperLeft);
            CollectionAssert.AreEqual(expectedUpperRightMove, upperRight);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_PositionOutOfChessboard_ShouldReturnEmptyList()
        {
            Chessboard chessboard = new("8/4R3/5KB1/4p3/8/8/8/k7 b - - 0 1");
            King king = (King)chessboard.GetReadOnlySquare("A1").ReadOnlyPiece!;

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(king);

            Assert.IsFalse(moves.Find(m => m.Direction is UpperLeft).RangeOfAttack.Any());
            Assert.IsFalse(moves.Find(m => m.Direction is Left).RangeOfAttack.Any());
            Assert.IsFalse(moves.Find(m => m.Direction is LowerLeft).RangeOfAttack.Any());
            Assert.IsFalse(moves.Find(m => m.Direction is LowerRight).RangeOfAttack.Any());
            Assert.IsFalse(moves.Find(m => m.Direction is Down).RangeOfAttack.Any());
        }
    }
}