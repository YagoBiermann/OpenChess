using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class KnightTests
    {
        [TestMethod]
        public void NameProperty_BlackKnight_ShouldBeLowercaseN()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("B8")).Piece!;

            Assert.AreEqual(knight.Name, 'n');
        }
        [TestMethod]
        public void NameProperty_WhiteKnight_ShouldBeUppercaseN()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("B1")).Piece!;

            Assert.AreEqual(knight.Name, 'N');
        }

        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Knight whiteKnight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("B1")).Piece!;
            Knight blackKnight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("B8")).Piece!;

            Assert.IsFalse(whiteKnight.IsLongRange);
            Assert.IsFalse(blackKnight.IsLongRange);
        }

        [TestMethod]
        public void DirectionsProperty_ShouldReturnAllDirections()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Knight whiteKnight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("B1")).Piece!;
            Knight blackKnight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("B8")).Piece!;

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


            CollectionAssert.AreEquivalent(directions, whiteKnight.Directions);
            CollectionAssert.AreEquivalent(directions, blackKnight.Directions);
        }

        [TestMethod]
        public void CalculateMoveRange_ShouldReturnAllMoves()
        {
            Chessboard chessboard = new("rnbqkbnr/pppppppp/8/8/4N3/8/PPPPPPPP/RNBQKB1R w KQkq - 0 1");
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("E4")).Piece!;

            List<MovePositions> expectedMoves = new()
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

            List<MovePositions> moves = knight.CalculateMoveRange();

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
            Chessboard chessboard = new("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1");
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("G4")).Piece!;
            List<MovePositions> moves = knight.CalculateLegalMoves();
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
            List<MovePositions> moves = knight.CalculateLegalMoves();
            List<Coordinate> lowerRightMove = moves.Find(m => m.Direction.Equals(new Direction(1, -2))).Coordinates;

            Assert.IsFalse(lowerRightMove.Any());
        }

        [TestMethod]
        public void CalculateLegalMoves_ShouldNotIncludeTheKing()
        {
            Chessboard chessboard = new("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1");
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("G4")).Piece!;
            List<MovePositions> moves = knight.CalculateLegalMoves();
            List<Coordinate> lowerLeftMove = moves.Find(m => m.Direction.Equals(new Direction(-2, -1))).Coordinates;
            List<Coordinate> upperRightMove = moves.Find(m => m.Direction.Equals(new Direction(1, 2))).Coordinates;

            Assert.IsFalse(lowerLeftMove.Any());
            Assert.IsFalse(upperRightMove.Any());
        }

        [TestMethod]
        public void CalculateLegalMoves_NoPiecesFound_ShouldReturnAllCoordinatesFromCurrentDirection()
        {
            Chessboard chessboard = new("8/8/5p1K/4r3/6N1/4k3/7P/8 w - - 0 1");
            Knight knight = (Knight)chessboard.GetSquare(Coordinate.GetInstance("G4")).Piece!;
            List<MovePositions> moves = knight.CalculateLegalMoves();
            List<Coordinate> lowerLeftMove = moves.Find(m => m.Direction.Equals(new Direction(-1, -2))).Coordinates;
            List<Coordinate> expectedLowerLeftMove = new() { Coordinate.GetInstance("F2") };

            CollectionAssert.AreEqual(lowerLeftMove, expectedLowerLeftMove);
        }
    }
}