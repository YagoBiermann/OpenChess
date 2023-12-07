using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PGNBuilderTests
    {

        [TestMethod]
        public void PawnTextMoveBuilder_Build_ShouldCreateDefaultMove()
        {
            Coordinate origin = Coordinate.GetInstance("E4");
            Coordinate destination = Coordinate.GetInstance("E5");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            string move = builder.Build().Result;

            Assert.AreEqual("1. e5", move);
        }

        [TestMethod]
        public void PawnTextMoveBuilder_Build_MoveWithCheck_ShouldAddCheckSign()
        {
            Coordinate origin = Coordinate.GetInstance("E4");
            Coordinate destination = Coordinate.GetInstance("E5");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            string move = builder.Build().AppendCheckSign().Result;

            Assert.AreEqual("1. e5+", move);
        }

        [TestMethod]
        public void PawnTextMoveBuilder_Build_MoveWithCheckmate_ShouldAddCheckmateSign()
        {
            Coordinate origin = Coordinate.GetInstance("E4");
            Coordinate destination = Coordinate.GetInstance("E5");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            string move = builder.Build().AppendCheckMateSign().Result;

            Assert.AreEqual("1. e5#", move);
        }

        [TestMethod]
        public void PawnTextMoveBuilder_Build_WithCapture_ShouldAddCaptureSignToMove()
        {
            Coordinate origin = Coordinate.GetInstance("E4");
            Coordinate destination = Coordinate.GetInstance("D5");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            string move = builder.Build().AppendCaptureSign().Result;

            Assert.AreEqual("1. exd5", move);
        }
        [TestMethod]
        public void PawnTextMoveBuilder_Build_WithPromotion_ShouldAddPromotionSignToMove()
        {
            Coordinate origin = Coordinate.GetInstance("D7");
            Coordinate destination = Coordinate.GetInstance("D8");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            builder.Build();
            string move = builder.AppendPromotionSign('Q').Result;

            Assert.AreEqual("1. d8=Q", move);
        }
    }
}