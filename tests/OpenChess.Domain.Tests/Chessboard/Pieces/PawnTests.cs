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
        public void IsFirstMove_BlackPawn_FirstMove_ShouldReturnTrue()
        {
            Chessboard chessboard = new(FEN.InitialPosition);
            var blackPawn = (Pawn)chessboard.GetSquare(Coordinate.GetInstance("E7")).Piece!;
            Assert.IsTrue(blackPawn.IsFirstMove);
        }

        [TestMethod]
        public void IsFirstMove_WhitePawn_FirstMove_ShouldReturnTrue()
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
    }
}