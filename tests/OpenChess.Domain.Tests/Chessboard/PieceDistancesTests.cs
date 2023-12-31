using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class PieceDistancesTests
    {

        [DataRow("E4", "B7", 3)]
        [DataRow("E4", "A8", 4)]
        [DataRow("B5", "F1", 4)]
        [DataRow("B1", "H7", 6)]
        [DataRow("A1", "H8", 7)]
        [DataRow("A1", "A8", 7)]
        [DataRow("B3", "B6", 3)]
        [DataRow("F5", "F2", 3)]
        [DataRow("D4", "D8", 4)]
        [DataRow("A1", "A8", 7)]
        [DataRow("B3", "B6", 3)]
        [DataRow("F5", "F2", 3)]
        [DataRow("D4", "D8", 4)]
        [DataRow("A4", "H4", 7)]
        [DataRow("D6", "A6", 3)]
        [TestMethod]
        public void CalculateDistance_ShouldCalculateCorrectDistance(string origin, string destination, int expectedDistance)
        {
            Coordinate c1 = Coordinate.GetInstance(origin);
            Coordinate c2 = Coordinate.GetInstance(destination);

            Assert.AreEqual(PieceDistances.CalculateDistance(c1, c2), expectedDistance);
        }

        [TestMethod]
        public void CalculateDistance_ShouldCalculateDistancesFromAListOfPieces()
        {
            Chessboard chessboard = new(new FenInfo("r2qk2r/1pp2pp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP2PP1/R2QK2R b KQkq - 0 1"));

            var pieceOfReference = chessboard.GetReadOnlySquare("D1").ReadOnlyPiece!;
            var pieceAtD4 = chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            var pieceAtD5 = chessboard.GetReadOnlySquare("D5").ReadOnlyPiece!;
            var pieceAtD8 = chessboard.GetReadOnlySquare("D8").ReadOnlyPiece!;

            List<IReadOnlyPiece> pieces = new()
            {
                pieceAtD4,
                pieceAtD5,
                pieceAtD8,
            };
            List<PieceDistances> expectedDistances = new() { new(3, pieceAtD4), new(4, pieceAtD5), new(7, pieceAtD8), };
            List<PieceDistances> distances = PieceDistances.CalculateDistance(pieceOfReference, pieces);

            CollectionAssert.AreEqual(expectedDistances, distances);
        }

        [TestMethod]
        public void CalculateDistance_UnsortedRange_ShouldReturnSortedList()
        {
            Chessboard chessboard = new(new FenInfo("r2qk2r/1pp2pp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP2PP1/R2QK2R b KQkq - 0 1"));

            var pieceOfReference = chessboard.GetReadOnlySquare("D1").ReadOnlyPiece!;
            var pieceAtD4 = chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            var pieceAtD5 = chessboard.GetReadOnlySquare("D5").ReadOnlyPiece!;
            var pieceAtD8 = chessboard.GetReadOnlySquare("D8").ReadOnlyPiece!;

            List<IReadOnlyPiece> pieces = new()
            {
                pieceAtD4,
                pieceAtD8,
                pieceAtD5,
            };
            List<PieceDistances> expectedDistances = new() { new(3, pieceAtD4), new(4, pieceAtD5), new(7, pieceAtD8) };
            List<PieceDistances> distances = PieceDistances.CalculateDistance(pieceOfReference, pieces);

            CollectionAssert.AreEqual(expectedDistances, distances);
        }
    }
}