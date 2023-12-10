namespace OpenChess.Domain
{
    [TestClass]
    public class CastlingTests
    {
        [DataRow("E1", "G1")]
        [DataRow("E1", "C1")]
        [DataRow("E8", "G8")]
        [DataRow("E8", "C8")]
        [TestMethod]
        public void IsCastling_ShouldReturnTrue(string position1, string position2)
        {
            Chessboard chessboard = new("r3k2r/pp3ppp/2p5/8/8/3P4/PPP2PPP/R3K2R w KQkq - 0 1");
            Coordinate origin = Coordinate.GetInstance(position1);
            Coordinate destination = Coordinate.GetInstance(position2);

            Assert.IsTrue(Castling.IsCastling(origin, destination, chessboard));
        }
    }
}