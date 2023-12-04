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

        [DataRow("A1", 0)]
        [DataRow("A2", 1)]
        [DataRow("B3", 2)]
        [DataRow("C4", 3)]
        [DataRow("D5", 4)]
        [DataRow("D6", 5)]
        [DataRow("D7", 6)]
        [DataRow("D8", 7)]
        [TestMethod]
        public void RowToIndex_ShouldReturnRowAsInt(string c, int row)
        {
            Coordinate coordinate = Coordinate.GetInstance(c);

            Assert.AreEqual(row, coordinate.RowToInt);
        }

        [DataRow("A1", 0)]
        [DataRow("B2", 1)]
        [DataRow("C3", 2)]
        [DataRow("D4", 3)]
        [DataRow("E5", 4)]
        [DataRow("F6", 5)]
        [DataRow("G7", 6)]
        [DataRow("H8", 7)]
        [TestMethod]
        public void ColumnToIndex_ShouldReturnColumnAsInt(string c, int row)
        {
            Coordinate coordinate = Coordinate.GetInstance(c);

            Assert.AreEqual(row, coordinate.ColumnToInt);
        }

        [DataRow("A1", '1')]
        [DataRow("A2", '2')]
        [DataRow("A3", '3')]
        [DataRow("A4", '4')]
        [DataRow("A5", '5')]
        [DataRow("A6", '6')]
        [DataRow("A7", '7')]
        [DataRow("A8", '8')]
        [TestMethod]
        public void Row_ShouldBeNumber(string c, char r)
        {
            Coordinate coordinate = Coordinate.GetInstance(c);

            Assert.AreEqual(r, coordinate.Row);
        }

        [DataRow("A1", 'A')]
        [DataRow("B1", 'B')]
        [DataRow("C1", 'C')]
        [DataRow("D1", 'D')]
        [DataRow("E1", 'E')]
        [DataRow("F1", 'F')]
        [DataRow("G1", 'G')]
        [DataRow("H1", 'H')]
        [TestMethod]
        public void Column_ShouldBeLetter(string c, char r)
        {
            Coordinate coordinate = Coordinate.GetInstance(c);

            Assert.AreEqual(r, coordinate.Column);
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
    }
};