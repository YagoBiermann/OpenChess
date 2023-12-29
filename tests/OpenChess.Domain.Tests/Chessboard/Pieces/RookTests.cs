using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class RookTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeTrue()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
            Rook rook = (Rook)chessboard.GetReadOnlySquare("A1").ReadOnlyPiece!;
            Rook rook2 = (Rook)chessboard.GetReadOnlySquare("A8").ReadOnlyPiece!;

            Assert.IsTrue(rook.IsLongRange);
            Assert.IsTrue(rook2.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnUpDownLeftRight()
        {
            Chessboard chessboard = new(FenInfo.InitialPosition);
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

            List<PieceRangeOfAttack> expectedMoves = new()
            {
                ExpectedMoves.GetMove(rook.Origin, new Up(), rook.MoveAmount, rook),
                ExpectedMoves.GetMove(rook.Origin, new Down(), rook.MoveAmount, rook),
                ExpectedMoves.GetMove(rook.Origin, new Left(), rook.MoveAmount, rook),
                ExpectedMoves.GetMove(rook.Origin, new Right(), rook.MoveAmount, rook)
            };

            List<PieceRangeOfAttack> moves = rook.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (PieceRangeOfAttack move in moves)
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

            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = legalMoves.CalculateMoves(rook);
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

            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = legalMoves.CalculateMoves(rook);
            List<Coordinate> rightMoves = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, rightMoves);
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

            IMoveCalculator legalMoves = new LegalMovesCalculator(chessboard);
            List<PieceRangeOfAttack> moves = legalMoves.CalculateMoves(rook);
            List<Coordinate> rightMoves = moves.Find(m => m.Direction.Equals(new Right())).Coordinates;

            CollectionAssert.AreEqual(expectedMove, rightMoves);
        }
    }
}