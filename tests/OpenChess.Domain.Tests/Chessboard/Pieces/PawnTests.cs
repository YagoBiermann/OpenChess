using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PawnTests
    {
        [TestMethod]
        public void NewInstance_IsLongRange_ShouldBeFalse()
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
    }
}