using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PromotionTests
    {

        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "A2", "A1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "B2", "B1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "C2", "C1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "D2", "D1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "E2", "E1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "F2", "F1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "G2", "G1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "H2", "H1")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "A7", "A8")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "B7", "B8")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "C7", "C8")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "D7", "D8")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "E7", "E8")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "F7", "F8")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "G7", "G8")]
        [DataRow("8/PPPPPPPP/8/8/8/8/pppppppp/8 w - - 0 1", "H7", "H8")]
        [TestMethod]
        public void IsPromoting_ShouldReturnTrue(string fen, string position1, string position2)
        {
            Chessboard chessboard = new(fen);
            Promotion promotion = new(chessboard);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.IsTrue(promotion.IsPromoting(origin, destination));
        }

        [DataRow("8/K7/4P3/8/8/4p3/k1q5/8 w - - 0 1", "E6", "E8")]
        [DataRow("8/K7/4P3/8/8/4p3/k1q5/8 w - - 0 1", "E6", "E7")]
        [DataRow("8/K7/4P3/8/8/4p3/k1q5/8 w - - 0 1", "E3", "E1")]
        [DataRow("8/K7/4P3/8/8/4p3/k1q5/8 w - - 0 1", "E3", "E2")]
        [DataRow("8/K7/4P3/8/8/4p3/k1q5/8 w - - 0 1", "C2", "C1")]
        [DataRow("8/K7/4P3/8/8/4p3/k1q5/8 w - - 0 1", "A2", "A1")]
        [DataRow("8/K7/4P3/8/8/4p3/k1q5/8 w - - 0 1", "A7", "A8")]
        public void IsPromoting_ShouldReturnFalse(string fen, string position1, string position2)
        {
            Chessboard chessboard = new(fen);
            Promotion promotion = new(chessboard);
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.IsFalse(promotion.IsPromoting(origin, destination));
        }
    }
}