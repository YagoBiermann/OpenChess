using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class RookTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
            Rook rook = (Rook)chessboard.GetReadOnlySquare("A1").ReadOnlyPiece!;
            Rook rook2 = (Rook)chessboard.GetReadOnlySquare("A8").ReadOnlyPiece!;

            Assert.IsTrue(rook.IsLongRange);
            Assert.IsTrue(rook2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnUpDownLeftRight()
        {
            Chessboard chessboard = new(new FenInfo(FenInfo.InitialPosition));
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
        public void CalculateLineOfSight_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new(new FenInfo("rnbqkbnr/pppppppp/8/8/4R3/8/PPPPPPPP/RNB1KBNR w KQkq - 0 1"));
            Rook rook = (Rook)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;

            List<PieceLineOfSight> expectedMoves = new()
            {
                ExpectedMoves.GetLineOfSight(chessboard, rook, new Up(), rook.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, rook, new Down(), rook.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, rook, new Left(), rook.MoveAmount),
                ExpectedMoves.GetLineOfSight(chessboard, rook, new Right(), rook.MoveAmount)
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceLineOfSight> moves = moveCalculator.CalculateLineOfSight(rook);

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
            Chessboard chessboard = new(new FenInfo("8/8/5K2/8/1RR2r1p/8/5k2/8 b - - 0 1"));
            Rook rook = (Rook)chessboard.GetReadOnlySquare("F4").ReadOnlyPiece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("E4"),
                Coordinate.GetInstance("D4"),
                Coordinate.GetInstance("C4"),
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(rook);
            var leftMoves = moves.Find(m => m.Direction is Left);

            CollectionAssert.AreEqual(expectedMove, leftMoves.RangeOfAttack);
            Assert.IsNotNull(leftMoves.NearestPiece);
            Assert.AreEqual(ColorUtils.GetOppositeColor(leftMoves.Piece.Color), leftMoves.NearestPiece.Color);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_ShouldIncludeAllyPieces()
        {
            Chessboard chessboard = new(new FenInfo("8/8/5K2/8/1RR2r1p/8/5k2/8 b - - 0 1"));
            Rook rook = (Rook)chessboard.GetReadOnlySquare("F4").ReadOnlyPiece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("G4"),
                Coordinate.GetInstance("H4"),
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(rook);
            var rightMoves = moves.Find(m => m.Direction is Right);

            CollectionAssert.AreEqual(expectedMove, rightMoves.RangeOfAttack);
            Assert.IsNotNull(rightMoves.NearestPiece);
        }

        [TestMethod]
        public void CalculateRangeOfAttack_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new(new FenInfo("8/8/4K3/8/1RR1r3/8/4k3/8 b - - 0 1"));
            Rook rook = (Rook)chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;
            List<Coordinate> expectedMove = new()
            {
                Coordinate.GetInstance("F4"),
                Coordinate.GetInstance("G4"),
                Coordinate.GetInstance("H4"),
            };

            IMoveCalculator moveCalculator = new MovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = moveCalculator.CalculateRangeOfAttack(rook);
            var rightMoves = moves.Find(m => m.Direction is Right);

            CollectionAssert.AreEqual(expectedMove, rightMoves.RangeOfAttack);
        }
    }
}