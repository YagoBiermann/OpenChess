using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CoordinateDistancesTests
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

            Assert.AreEqual(CoordinateDistances.CalculateDistance(c1, c2), expectedDistance);
        }

        [TestMethod]
        public void CalculateDistance_ShouldCalculateDistanceInRangeOfCoordinates()
        {
            Coordinate c1 = Coordinate.GetInstance("E5");
            Coordinate c2 = Coordinate.GetInstance("E6");
            Coordinate c3 = Coordinate.GetInstance("E7");
            List<Coordinate> coordinates = new() { c1, c2, c3 };
            Coordinate origin = Coordinate.GetInstance("E4");
            List<CoordinateDistances> expectedDistances = new() { new CoordinateDistances(1, origin, c1), new CoordinateDistances(2, origin, c2), new CoordinateDistances(3, origin, c3), };

            List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(origin, coordinates);

            CollectionAssert.AreEqual(expectedDistances, distances);
        }

        [TestMethod]
        public void CalculateDistance_UnsortedRange_ShouldCalculateDistancesCorrectly()
        {
            Coordinate c1 = Coordinate.GetInstance("E7");
            Coordinate c2 = Coordinate.GetInstance("E5");
            Coordinate c3 = Coordinate.GetInstance("A8");
            Coordinate c4 = Coordinate.GetInstance("H1");
            List<Coordinate> coordinates = new() { c1, c2, c3, c4 };
            Coordinate origin = Coordinate.GetInstance("E4");

            List<CoordinateDistances> expectedDistances = new()
            {
                new CoordinateDistances(3,origin, c1),
                new CoordinateDistances(1,origin, c2),
                new CoordinateDistances(4,origin, c3),
                new CoordinateDistances(3,origin, c4),
            };

            List<CoordinateDistances> distances = CoordinateDistances.CalculateDistance(origin, coordinates);

            CollectionAssert.AreEqual(expectedDistances, distances);
        }

        [TestMethod]
        public void CalculateNearestDistance_ShouldReturnNearestPieceFromOrigin()
        {
            Coordinate origin = Coordinate.GetInstance("A1");
            Coordinate c1 = Coordinate.GetInstance("A2");
            Coordinate c2 = Coordinate.GetInstance("A3");
            Coordinate c3 = Coordinate.GetInstance("A4");
            Coordinate c4 = Coordinate.GetInstance("A5");

            List<CoordinateDistances> distances = new() { new(1, origin, c1), new(2, origin, c2), new(3, origin, c3), new(4, origin, c4), };

            CoordinateDistances expectedPiece = new(1, origin, c1);
            CoordinateDistances nearestPiece = CoordinateDistances.CalculateNearestDistance(distances);

            Assert.AreEqual(expectedPiece, nearestPiece);
        }
    }
}