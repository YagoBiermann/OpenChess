using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PGNBuilderTests
    {
        private Chessboard _chessboard = new(FEN.InitialPosition);
        private Coordinate _defaultMoveDestination = Coordinate.GetInstance("D8");

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
        public void PawnTextMoveBuilder_Build_WithCaptureAndCheck_ShouldAddCaptureAndCheckSignToMove()
        {
            Coordinate origin = Coordinate.GetInstance("E4");
            Coordinate destination = Coordinate.GetInstance("D5");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            string move = builder.Build().AppendCaptureSign().AppendCheckSign().Result;

            Assert.AreEqual("1. exd5+", move);
        }

        [TestMethod]
        public void PawnTextMoveBuilder_Build_WithCaptureAndCheckmate_ShouldAddCaptureAndCheckmateSignToMove()
        {
            Coordinate origin = Coordinate.GetInstance("E4");
            Coordinate destination = Coordinate.GetInstance("D5");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            string move = builder.Build().AppendCaptureSign().AppendCheckMateSign().Result;

            Assert.AreEqual("1. exd5#", move);
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

        [TestMethod]
        public void PawnTextMoveBuilder_Build_WithPromotionAndCheck_ShouldAddPromotionAndCheckSignToMove()
        {
            Coordinate origin = Coordinate.GetInstance("D7");
            Coordinate destination = Coordinate.GetInstance("D8");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            builder.Build();
            builder.AppendPromotionSign('Q');
            builder.AppendCheckSign();

            Assert.AreEqual("1. d8=Q+", builder.Result);
        }

        [TestMethod]
        public void PawnTextMoveBuilder_Build_WithPromotionAndCheckmate_ShouldAddPromotionAndCheckmateSignToMove()
        {
            Coordinate origin = Coordinate.GetInstance("D7");
            Coordinate destination = Coordinate.GetInstance("D8");
            PawnTextMoveBuilder builder = new(1, origin, destination);

            builder.Build();
            builder.AppendPromotionSign('Q');
            builder.AppendCheckMateSign();

            Assert.AreEqual("1. d8=Q#", builder.Result);
        }
    }
}