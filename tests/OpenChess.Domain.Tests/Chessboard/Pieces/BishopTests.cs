using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class BishopTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Bishop whiteBishop = (Bishop)chessboard.GetReadOnlySquare("C1").ReadOnlyPiece!;
            Bishop blackBishop = (Bishop)chessboard.GetReadOnlySquare("C8").ReadOnlyPiece!;

            Assert.IsTrue(whiteBishop.IsLongRange);
            Assert.IsTrue(blackBishop.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnDiagonalDirections()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Bishop whiteBishop = (Bishop)chessboard.GetReadOnlySquare("C1").ReadOnlyPiece!;
            Bishop blackBishop = (Bishop)chessboard.GetReadOnlySquare("C8").ReadOnlyPiece!;

            List<Direction> directions = new()
            {
                new UpperLeft(),
                new UpperRight(),
                new LowerLeft(),
                new LowerRight()
            };

            CollectionAssert.AreEquivalent(directions, whiteBishop.Directions);
            CollectionAssert.AreEquivalent(directions, blackBishop.Directions);
        }

        [TestMethod]
        public void CalculateLineOfSight_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new("r1bqk1nr/pppp2p1/5p1p/4P1N1/4B1Q1/8/PPP2PPP/RN2K2R b KQkq - 0 1");
            Bishop bishop = (Bishop)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<PieceLineOfSight> expectedMoves = new()
            {
                ExpectedMoves.GetLineOfSight(chessboard, bishop, new UpperLeft(), bishop.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, bishop, new UpperRight(), bishop.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, bishop, new LowerLeft(), bishop.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, bishop, new LowerRight(), bishop.MoveAmount),
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceLineOfSight> moves = moveCalculator.CalculateLineOfSight(bishop);

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
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetReadOnlySquare("C5").ReadOnlyPiece!;

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(bishop);

            var lowerRightMove = moves.Find(m => m.Direction is LowerRight);
            List<Coordinate> expectedLowerRightMove = new() { Coordinate.GetInstance("D4"), Coordinate.GetInstance("E3"), Coordinate.GetInstance("F2"), };

            CollectionAssert.AreEqual(expectedLowerRightMove, lowerRightMove.RangeOfAttack);
            Assert.IsNotNull(lowerRightMove.NearestPiece);
            Assert.IsTrue(lowerRightMove.NearestPiece is Pawn);
            Assert.IsFalse(lowerRightMove.IsHittingTheEnemyKing);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldIncludeAllyPieces()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetReadOnlySquare("C5").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(bishop);

            var lowerLeftMove = moves.Find(m => m.Direction is LowerLeft);
            List<Coordinate> expectedLowerLeftMovesMove = new() { Coordinate.GetInstance("B4"), Coordinate.GetInstance("A3") };

            CollectionAssert.AreEqual(expectedLowerLeftMovesMove, lowerLeftMove.RangeOfAttack);
            Assert.IsNotNull(lowerLeftMove.NearestPiece);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldIncludeEnemyKing()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetReadOnlySquare("C5").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(bishop);

            var upperRightMove = moves.Find(m => m.Direction is UpperRight);
            List<Coordinate> expectedUpperRightMoves = new() { Coordinate.GetInstance("D6"), Coordinate.GetInstance("E7") };

            CollectionAssert.AreEqual(expectedUpperRightMoves, upperRightMove.RangeOfAttack);
            Assert.IsNotNull(upperRightMove.NearestPiece);
            Assert.IsTrue(upperRightMove.NearestPiece is King);
            Assert.IsTrue(upperRightMove.IsHittingTheEnemyKing);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("2b5/2P1k3/8/2B1p3/4K3/P7/5p2/8 b - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetReadOnlySquare("C5").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(bishop);

            var upperLeftMove = moves.Find(m => m.Direction is UpperLeft);
            List<Coordinate> expectedUpperLeftMoves = new() { Coordinate.GetInstance("B6"), Coordinate.GetInstance("A7") };

            CollectionAssert.AreEqual(expectedUpperLeftMoves, upperLeftMove.RangeOfAttack);
            Assert.IsNull(upperLeftMove.NearestPiece);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_PositionOutOfChessboard_ShouldReturnEmptyList()
        {
            Chessboard chessboard = new("2b5/B1P1k3/8/4p3/4K3/P7/5p2/8 w - - 0 1");
            Bishop bishop = (Bishop)chessboard.GetReadOnlySquare("A7").ReadOnlyPiece!;
            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(bishop);

            Assert.IsFalse(moves.Find(m => m.Direction is UpperLeft).RangeOfAttack.Any());
            Assert.IsFalse(moves.Find(m => m.Direction is LowerLeft).RangeOfAttack.Any());
        }
    }
}