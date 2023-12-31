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
            Chessboard chessboard = new("r2qk2r/1pp2pp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP2PP1/R2QK2R b KQkq - 0 1");

            var pieceOfReference = chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;
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
        public void CalculateDistance_UnsortedRange_ShouldCalculateDistancesCorrectly()
        {
            Chessboard chessboard = new("r2qk2r/1pp2pp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP2PP1/R2QK2R b KQkq - 0 1");

            var pieceOfReference = chessboard.GetReadOnlySquare("E4").ReadOnlyPiece!;
            var pieceAtD4 = chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            var pieceAtD5 = chessboard.GetReadOnlySquare("D5").ReadOnlyPiece!;
            var pieceAtD8 = chessboard.GetReadOnlySquare("D8").ReadOnlyPiece!;

            List<IReadOnlyPiece> pieces = new()
            {
                pieceAtD4,
                pieceAtD8,
                pieceAtD5,
            };
            List<PieceDistances> expectedDistances = new() { new(3, pieceAtD4), new(7, pieceAtD8), new(4, pieceAtD5) };
            List<PieceDistances> distances = PieceDistances.CalculateDistance(pieceOfReference, pieces);

            CollectionAssert.AreEqual(expectedDistances, distances);
        }

        [TestMethod]
        public void CalculateNearestDistance_ShouldReturnNearestPieceFromOrigin()
        {
            Chessboard chessboard = new("r2qk2r/1pp2pp1/p1n2n1p/1B1pp1B1/1b1PP1b1/P1N2N1P/1PP2PP1/R2QK2R b KQkq - 0 1");

            var pieceOfReference = chessboard.GetReadOnlySquare("B2").ReadOnlyPiece!;
            var pieceAtC3 = chessboard.GetReadOnlySquare("C3").ReadOnlyPiece!;
            var pieceAtD4 = chessboard.GetReadOnlySquare("D4").ReadOnlyPiece!;
            var pieceAtE5 = chessboard.GetReadOnlySquare("E5").ReadOnlyPiece!;
            var pieceAtF6 = chessboard.GetReadOnlySquare("F6").ReadOnlyPiece!;
            var pieceAtG7 = chessboard.GetReadOnlySquare("G7").ReadOnlyPiece!;
            var pieceAtH8 = chessboard.GetReadOnlySquare("H8").ReadOnlyPiece!;

            List<PieceDistances> pieceDistances = new()
            {
                new(5,pieceAtG7),
                new(6,pieceAtH8),
                new(1,pieceAtC3),
                new(3,pieceAtE5),
                new(2,pieceAtD4),
                new(4,pieceAtF6),
            };
            PieceDistances nearestPiece = PieceDistances.CalculateNearestDistance(pieceDistances);
            PieceDistances expectedDistance = new(1, pieceAtC3);

            Assert.AreEqual(expectedDistance, nearestPiece);
        }
    }
}