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
        public void NewInstance_GivenAlgebraicNotation_ShouldInstantiateNewCoordinate()
        {
            Coordinate coordinate = Coordinate.GetInstance("A1");
            Coordinate coordinate2 = Coordinate.GetInstance("C4");
            Coordinate coordinate3 = Coordinate.GetInstance("H8");

            Assert.AreEqual("A1", coordinate.ToString());
            Assert.AreEqual("C4", coordinate2.ToString());
            Assert.AreEqual("H8", coordinate3.ToString());
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
    }
};