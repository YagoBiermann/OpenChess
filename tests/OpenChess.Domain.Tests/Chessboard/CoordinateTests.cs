using OpenChess.Domain;

namespace OpenChess.Tests
{
    [TestClass]
    public class CoordinateTests
    {
        [TestMethod]
        public void NewInstance_ColNumberLessThanZero_ShouldThrowException()
        {
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance(-1, 0));
        }

        [TestMethod]
        public void NewInstance_RowNumberLessThanZero_ShouldThrowException()
        {
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance(0, -1));
        }

        [TestMethod]
        public void NewInstance_ColNumberGreaterThanSeven_ShouldThrowException()
        {
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance(8, 0));
        }

        [TestMethod]
        public void NewInstance_RowNumberGreaterThanSeven_ShouldThrowException()
        {
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance(0, 8));
        }

        [TestMethod]
        public void NewInstance_GivenTwoNumbers_ShouldConvertToAlgebraicNotation()
        {
            Coordinate coordinate = Coordinate.GetInstance(0, 0);
            Coordinate coordinate2 = Coordinate.GetInstance(5, 2);
            Coordinate coordinate3 = Coordinate.GetInstance(7, 7);

            Assert.AreEqual("A1", coordinate.ToString());
            Assert.AreEqual("F3", coordinate2.ToString());
            Assert.AreEqual("H8", coordinate3.ToString());
        }

        [TestMethod]
        public void NewInstance_GivenTwoNumbers_ShouldInstantiate()
        {

            List<int> indexes = new()
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6,
                7,
            };

            foreach (int col in indexes)
            {
                foreach (int row in indexes)
                {
                    Coordinate.GetInstance(col, row);
                }
            }
        }

        [TestMethod]
        public void NewInstance_GivenAlgebraicNotation_ShouldInstantiateNewCoordinate()
        {
            List<string> cols = new()
            {
                "A",
                "B",
                "C",
                "D",
                "E",
                "F",
                "G",
                "H",
            };
            List<string> rows = new()
            {
                "1",
                "2",
                "3",
                "4",
                "5",
                "6",
                "7",
                "8",
            };

            foreach (string col in cols)
            {
                foreach (string row in rows)
                {
                    Coordinate.GetInstance($"{col}{row}");
                }
            }
        }

        [TestMethod]
        public void NewInstance_GivenInvalidAlgebraicNotation_ShouldThrowException()
        {
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance("K1"));
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance("V2"));
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance("11"));
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance(".2"));
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance("A0"));
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance("A9"));
            Assert.ThrowsException<CoordinateException>(() => Coordinate.GetInstance("A10"));
        }

        [TestMethod]
        public void Equals_ObjectsWithSameAlgebraicNotation_ShouldReturnTrue()
        {
            Coordinate coordinate1 = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A1");

            Assert.IsTrue(coordinate1.Equals(coordinate2));
        }

        [TestMethod]
        public void Equals_ObjectsWithDifferentAlgebraicNotation_ShouldReturnFalse()
        {
            Coordinate coordinate1 = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A2");

            Assert.IsFalse(coordinate1.Equals(coordinate2));
        }

        [TestMethod]
        public void GetInstance_ObjectThatAlreadyExists_ShouldReturnFromCache()
        {
            Coordinate coordinate1 = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A1");

            Coordinate coordinate3 = Coordinate.GetInstance(0, 2);
            Coordinate coordinate4 = Coordinate.GetInstance(0, 2);

            Assert.IsTrue(ReferenceEquals(coordinate1, coordinate2));
            Assert.IsTrue(ReferenceEquals(coordinate3, coordinate4));
        }

        [TestMethod]
        public void GetInstance_DifferentObjects_ShouldNotReferenceSameInstance()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("A2");

            Coordinate coordinate3 = Coordinate.GetInstance(0, 2);
            Coordinate coordinate4 = Coordinate.GetInstance(0, 3);

            Assert.IsFalse(ReferenceEquals(coordinate, coordinate2));
            Assert.IsFalse(ReferenceEquals(coordinate3, coordinate4));
        }

        [TestMethod]
        public void RowToIndex_ShouldReturnRowAsInt()
        {
            Coordinate coordinate0 = Coordinate.GetInstance("A1");
            Coordinate coordinate1 = Coordinate.GetInstance("A2");
            Coordinate coordinate2 = Coordinate.GetInstance("B3");
            Coordinate coordinate3 = Coordinate.GetInstance("C4");
            Coordinate coordinate4 = Coordinate.GetInstance("D5");
            Coordinate coordinate5 = Coordinate.GetInstance("D6");
            Coordinate coordinate6 = Coordinate.GetInstance("D7");
            Coordinate coordinate7 = Coordinate.GetInstance("D8");

            Assert.AreEqual(0, coordinate0.RowToInt);
            Assert.AreEqual(1, coordinate1.RowToInt);
            Assert.AreEqual(2, coordinate2.RowToInt);
            Assert.AreEqual(3, coordinate3.RowToInt);
            Assert.AreEqual(4, coordinate4.RowToInt);
            Assert.AreEqual(5, coordinate5.RowToInt);
            Assert.AreEqual(6, coordinate6.RowToInt);
            Assert.AreEqual(7, coordinate7.RowToInt);
        }

        [TestMethod]
        public void ColumnToIndex_ShouldReturnColumnAsInt()
        {
            Coordinate coordinate0 = Coordinate.GetInstance("A1");
            Coordinate coordinate1 = Coordinate.GetInstance("B2");
            Coordinate coordinate2 = Coordinate.GetInstance("C3");
            Coordinate coordinate3 = Coordinate.GetInstance("D4");
            Coordinate coordinate4 = Coordinate.GetInstance("E5");
            Coordinate coordinate5 = Coordinate.GetInstance("F6");
            Coordinate coordinate6 = Coordinate.GetInstance("G7");
            Coordinate coordinate7 = Coordinate.GetInstance("H8");

            Assert.AreEqual(0, coordinate0.ColumnToInt);
            Assert.AreEqual(1, coordinate1.ColumnToInt);
            Assert.AreEqual(2, coordinate2.ColumnToInt);
            Assert.AreEqual(3, coordinate3.ColumnToInt);
            Assert.AreEqual(4, coordinate4.ColumnToInt);
            Assert.AreEqual(5, coordinate5.ColumnToInt);
            Assert.AreEqual(6, coordinate6.ColumnToInt);
            Assert.AreEqual(7, coordinate7.ColumnToInt);
        }

        [TestMethod]
        public void Row_ShouldBeNumber()
        {
            Coordinate coordinate0 = Coordinate.GetInstance("A1");
            Coordinate coordinate1 = Coordinate.GetInstance("A2");
            Coordinate coordinate2 = Coordinate.GetInstance("A3");
            Coordinate coordinate3 = Coordinate.GetInstance("A4");
            Coordinate coordinate4 = Coordinate.GetInstance("A5");
            Coordinate coordinate5 = Coordinate.GetInstance("A6");
            Coordinate coordinate6 = Coordinate.GetInstance("A7");
            Coordinate coordinate7 = Coordinate.GetInstance("A8");

            Assert.AreEqual('1', coordinate0.Row);
            Assert.AreEqual('2', coordinate1.Row);
            Assert.AreEqual('3', coordinate2.Row);
            Assert.AreEqual('4', coordinate3.Row);
            Assert.AreEqual('5', coordinate4.Row);
            Assert.AreEqual('6', coordinate5.Row);
            Assert.AreEqual('7', coordinate6.Row);
            Assert.AreEqual('8', coordinate7.Row);
        }

        [TestMethod]
        public void Column_ShouldBeLetter()
        {
            Coordinate coordinate0 = Coordinate.GetInstance("A1");
            Coordinate coordinate1 = Coordinate.GetInstance("B1");
            Coordinate coordinate2 = Coordinate.GetInstance("C1");
            Coordinate coordinate3 = Coordinate.GetInstance("D1");
            Coordinate coordinate4 = Coordinate.GetInstance("E1");
            Coordinate coordinate5 = Coordinate.GetInstance("F1");
            Coordinate coordinate6 = Coordinate.GetInstance("G1");
            Coordinate coordinate7 = Coordinate.GetInstance("H1");

            Assert.AreEqual('A', coordinate0.Column);
            Assert.AreEqual('B', coordinate1.Column);
            Assert.AreEqual('C', coordinate2.Column);
            Assert.AreEqual('D', coordinate3.Column);
            Assert.AreEqual('E', coordinate4.Column);
            Assert.AreEqual('F', coordinate5.Column);
            Assert.AreEqual('G', coordinate6.Column);
            Assert.AreEqual('H', coordinate7.Column);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionUp_Amount8_ShouldReturnCoordinatesFromE5toE8()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("E5"),
                Coordinate.GetInstance("E6"),
                Coordinate.GetInstance("E7"),
                Coordinate.GetInstance("E8")
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            Up up = new();
            int amount = 5;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, up, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionLeft_Amount8_ShouldReturnCoordinatesFromE4toA4()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("D4"),
                Coordinate.GetInstance("C4"),
                Coordinate.GetInstance("B4"),
                Coordinate.GetInstance("A4")
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            Left left = new();
            int amount = 8;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, left, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionRight_Amount8_ShouldReturnCoordinatesFromE4toH4()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("F4"),
                Coordinate.GetInstance("G4"),
                Coordinate.GetInstance("H4"),
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            Right right = new();
            int amount = 8;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, right, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionDown_Amount8_ShouldReturnCoordinatesFromE4toE1()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("E3"),
                Coordinate.GetInstance("E2"),
                Coordinate.GetInstance("E1"),
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            Down down = new();
            int amount = 8;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, down, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionUpperLeft_Amount8_ShouldReturnCoordinatesFromE4toA8()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("D5"),
                Coordinate.GetInstance("C6"),
                Coordinate.GetInstance("B7"),
                Coordinate.GetInstance("A8"),
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            UpperLeft upperLeft = new();
            int amount = 8;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, upperLeft, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionUpperRight_Amount8_ShouldReturnCoordinatesFromE4toH7()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("F5"),
                Coordinate.GetInstance("G6"),
                Coordinate.GetInstance("H7"),
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            UpperRight upperRight = new();
            int amount = 8;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, upperRight, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionLowerLeft_Amount8_ShouldReturnCoordinatesFromE4toA1()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("D3"),
                Coordinate.GetInstance("C2"),
                Coordinate.GetInstance("B1"),
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            LowerLeft lowerLeft = new();
            int amount = 8;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, lowerLeft, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

        [TestMethod]
        public void CalculateSequence_OriginE4_DirectionLowerRight_Amount8_ShouldReturnCoordinatesFromE4toH1()
        {
            List<Coordinate> coordinates = new();
            List<Coordinate> expectedCoordinates = new()
            {
                Coordinate.GetInstance("F3"),
                Coordinate.GetInstance("G2"),
                Coordinate.GetInstance("H1"),
            };
            Coordinate origin = Coordinate.GetInstance("E4");
            LowerRight lowerRight = new();
            int amount = 8;

            coordinates.AddRange(Coordinate.CalculateSequence(origin, lowerRight, amount));

            CollectionAssert.AreEqual(expectedCoordinates, coordinates);
        }

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

            Assert.AreEqual(Coordinate.CalculateDistance(c1, c2), expectedDistance);
        }

        [TestMethod]
        public void CalculateDistance_ShouldCalculateDistanceInRangeOfCoordinates()
        {
            Coordinate c1 = Coordinate.GetInstance("E5");
            Coordinate c2 = Coordinate.GetInstance("E6");
            Coordinate c3 = Coordinate.GetInstance("E7");
            List<Coordinate> coordinates = new() { c1, c2, c3 };
            Coordinate origin = Coordinate.GetInstance("E4");
            List<PieceDistances> expectedDistances = new() { new PieceDistances(1, c1), new PieceDistances(2, c2), new PieceDistances(3, c3), };

            List<PieceDistances> distances = Coordinate.CalculateDistance(origin, coordinates);

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

            List<PieceDistances> expectedDistances = new()
            {
                new PieceDistances(3, c1),
                new PieceDistances(1, c2),
                new PieceDistances(4, c3),
                new PieceDistances(3, c4),
            };

            List<PieceDistances> distances = Coordinate.CalculateDistance(origin, coordinates);

            CollectionAssert.AreEqual(expectedDistances, distances);
        }
    }
};