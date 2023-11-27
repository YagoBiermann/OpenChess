using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PawnTests
    {
        [TestMethod]
        public void IsLongRangeProperty_ShouldBeFalse()
        {
            Pawn pawn = new(Color.Black, Coordinate.GetInstance("A2"));

            Assert.IsFalse(pawn.IsLongRange);
        }

        [TestMethod]
        public void NewInstance_BlackDirections_ShouldBeDownwards()
        {
            Pawn pawn = new(Color.Black, Coordinate.GetInstance("A2"));

            Assert.IsTrue(pawn.Directions.Contains(new Down()));
            Assert.IsTrue(pawn.Directions.Contains(new LowerLeft()));
            Assert.IsTrue(pawn.Directions.Contains(new LowerRight()));

            Assert.IsFalse(pawn.Directions.Contains(new UpperRight()));
            Assert.IsFalse(pawn.Directions.Contains(new UpperLeft()));
            Assert.IsFalse(pawn.Directions.Contains(new Up()));
            Assert.IsFalse(pawn.Directions.Contains(new Left()));
            Assert.IsFalse(pawn.Directions.Contains(new Right()));
        }

        [TestMethod]
        public void NewInstance_WhiteDirections_ShouldBeUpwards()
        {
            Pawn pawn = new(Color.White, Coordinate.GetInstance("A2"));

            Assert.IsTrue(pawn.Directions.Contains(new UpperRight()));
            Assert.IsTrue(pawn.Directions.Contains(new UpperLeft()));
            Assert.IsTrue(pawn.Directions.Contains(new Up()));

            Assert.IsFalse(pawn.Directions.Contains(new Down()));
            Assert.IsFalse(pawn.Directions.Contains(new LowerLeft()));
            Assert.IsFalse(pawn.Directions.Contains(new LowerRight()));
            Assert.IsFalse(pawn.Directions.Contains(new Left()));
            Assert.IsFalse(pawn.Directions.Contains(new Right()));
        }

        [TestMethod]
        public void NameProperty_WhitePawn_ShoulBeUppercaseP()
        {
            Pawn pawn = new(Color.White, Coordinate.GetInstance("A2"));

            Assert.AreEqual(pawn.Name, 'P');
        }

        [TestMethod]
        public void NameProperty_BlackPawn_ShoulBeLowecaseP()
        {
            Pawn pawn = new(Color.Black, Coordinate.GetInstance("A2"));

            Assert.AreEqual(pawn.Name, 'p');
        }

        [TestMethod]
        public void IsFirstMove_BlackPawn_InSeventhRow_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            var blackPawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("E7")).Piece!;
            Assert.IsTrue(blackPawn.IsFirstMove);
        }

        [TestMethod]
        public void IsFirstMove_WhitePawn_InSecondRow_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            var whitePawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("E2")).Piece!;
            Assert.IsTrue(whitePawn.IsFirstMove);
        }

        [TestMethod]
        public void IsFirstMove_BlackPawn_NotInSeventhRow_ShouldReturnFalse()
        {
            Chessboard chessboard = new("rnbqk2r/pppp1ppp/4pn2/8/1bPPP3/2N2N2/PP3PPP/R1BQKB1R b KQkq - 0 1");
            var blackPawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("E6")).Piece!;
            Assert.IsFalse(blackPawn.IsFirstMove);
        }

        [TestMethod]
        public void IsFirstMove_WhitePawn_NotInSecondRow_ShouldReturnFalse()
        {
            Chessboard chessboard = new("rnbqk2r/pppp1ppp/4pn2/8/1bPPP3/2N2N2/PP3PPP/R1BQKB1R b KQkq - 0 1");
            var whitePawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("E4")).Piece!;
            Assert.IsFalse(whitePawn.IsFirstMove);
        }

        [TestMethod]
        public void ForwardDirection_ShouldReturnUpForWhite()
        {
            Pawn pawn = new(Color.White, Coordinate.GetInstance("E4"));
            Assert.AreEqual(pawn.ForwardDirection, new Up());
        }

        [TestMethod]
        public void ForwardDirection_ShouldReturnDownForBlack()
        {
            Pawn pawn = new(Color.Black, Coordinate.GetInstance("E4"));
            Assert.AreEqual(pawn.ForwardDirection, new Down());
        }

        [DataRow("E2")]
        [DataRow("E4")]
        [TestMethod]
        public void CalculateMoveRange_WhitePawn_ShouldReturnAllMoves(string origin)
        {
            Pawn pawn = new(Color.White, Coordinate.GetInstance(origin));

            List<Move> expectedMoves = new()
            {
                ExpectedMoves.GetMove(pawn.Origin, new Up(), pawn.ForwardMoveAmount),
                ExpectedMoves.GetMove(pawn.Origin, new UpperLeft(), pawn.MoveAmount),
                ExpectedMoves.GetMove(pawn.Origin, new UpperRight(), pawn.MoveAmount)
            };

            List<Move> moves = pawn.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (Move move in moves)
            {
                int index = moves.IndexOf(move);
                CollectionAssert.AreEqual(expectedMoves[index].Coordinates, move.Coordinates);
                Assert.AreEqual(expectedMoves[index].Direction, move.Direction);
            }
        }

        [DataRow("E7")]
        [DataRow("E5")]
        [TestMethod]
        public void CalculateMoveRange_BlackPawn_ShouldReturnAllMoves(string origin)
        {
            Pawn pawn = new(Color.Black, Coordinate.GetInstance(origin));

            List<Move> expectedMoves = new()
            {
                ExpectedMoves.GetMove(pawn.Origin, new Down(), pawn.ForwardMoveAmount),
                ExpectedMoves.GetMove(pawn.Origin, new LowerLeft(), pawn.MoveAmount),
                ExpectedMoves.GetMove(pawn.Origin, new LowerRight(), pawn.MoveAmount)
            };

            List<Move> moves = pawn.CalculateMoveRange();

            Assert.AreEqual(moves.Count, expectedMoves.Count);
            foreach (Move move in moves)
            {
                int index = moves.IndexOf(move);
                CollectionAssert.AreEqual(expectedMoves[index].Coordinates, move.Coordinates);
                Assert.AreEqual(expectedMoves[index].Direction, move.Direction);
            }
        }
        [DataRow("E4")]
        [DataRow("E5")]
        [DataRow("F2")]
        [DataRow("F7")]
        [TestMethod]
        public void CalculateLegalMoves_ForwardMoves_ShouldNotIncludePieces(string origin)
        {
            Chessboard chessboard = new("r1bqk2r/ppppbppp/2n2n2/1B2p3/3PP3/2N2N2/PPP2PPP/R1BQK2R b KQkq - 0 1");
            Pawn pawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance(origin)).Piece!;

            List<Move> forwardMoves = pawn.CalculateLegalMoves(chessboard).Where(m => m.Direction.Equals(pawn.ForwardDirection)).ToList();

            foreach (Move move in forwardMoves)
            {
                Assert.IsFalse(move.Coordinates.Any());
            }
        }

        [DataRow("E2", "E3", "E4")]
        [DataRow("E7", "E6", "E5")]
        [TestMethod]
        public void CalculateLegalMoves_ForwardMoves_FirstMove_ShouldReturnTwoCoordinates(string origin, string coordinate, string coordinate2)
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            Pawn pawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance(origin)).Piece!;

            List<Coordinate> expectedMoves = new()
            {
                Coordinate.GetInstance(coordinate),
                Coordinate.GetInstance(coordinate2),
            };

            List<Coordinate> forwardMoves = pawn
            .CalculateLegalMoves(chessboard)
            .Where(m => m.Direction.Equals(pawn.ForwardDirection))
            .FirstOrDefault()
            .Coordinates;

            CollectionAssert.AreEqual(expectedMoves, forwardMoves);
        }

        [DataRow("D4", "D5")]
        [DataRow("C6", "C5")]
        [TestMethod]
        public void CalculateLegalMoves_ForwardMoves_NotFirstMove_ShouldReturnOneCoordinate(string origin, string expected)
        {
            Chessboard chessboard = new("rn1qkbnr/pp2pppp/2p5/5b2/3PN3/8/PPP2PPP/R1BQKBNR w KQkq - 0 1");
            Pawn whitePawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance(origin)).Piece!;

            List<Coordinate> forwardMoves = whitePawn.CalculateLegalMoves(chessboard).Where(m => m.Direction.Equals(whitePawn.ForwardDirection)).FirstOrDefault().Coordinates;
            List<Coordinate> expectedMoves = new() { Coordinate.GetInstance(expected) };

            CollectionAssert.AreEqual(expectedMoves, forwardMoves);
        }
    }
}