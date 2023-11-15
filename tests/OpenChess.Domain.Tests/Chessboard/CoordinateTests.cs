namespace OpenChess.Domain.Tests;

[TestClass]
public class CoordinateTests
{
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
    public void IsValidColumn_GivenValidChar_ShouldReturnTrue()
    {
        Assert.IsTrue(Coordinate.IsValidColumn('A'));
        Assert.IsTrue(Coordinate.IsValidColumn('B'));
        Assert.IsTrue(Coordinate.IsValidColumn('C'));
        Assert.IsTrue(Coordinate.IsValidColumn('D'));
        Assert.IsTrue(Coordinate.IsValidColumn('E'));
        Assert.IsTrue(Coordinate.IsValidColumn('F'));
        Assert.IsTrue(Coordinate.IsValidColumn('G'));
        Assert.IsTrue(Coordinate.IsValidColumn('H'));
    }

    [TestMethod]
    public void IsValidRow_GivenValidChar_ShouldReturnTrue()
    {
        Assert.IsTrue(Coordinate.IsValidRow('1'));
        Assert.IsTrue(Coordinate.IsValidRow('2'));
        Assert.IsTrue(Coordinate.IsValidRow('3'));
        Assert.IsTrue(Coordinate.IsValidRow('4'));
        Assert.IsTrue(Coordinate.IsValidRow('5'));
        Assert.IsTrue(Coordinate.IsValidRow('6'));
        Assert.IsTrue(Coordinate.IsValidRow('7'));
        Assert.IsTrue(Coordinate.IsValidRow('8'));
    }

    [TestMethod]
    public void IsValidColumn_GivenValidInt_ShouldReturnTrue()
    {
        Assert.IsTrue(Coordinate.IsValidColumn(0));
        Assert.IsTrue(Coordinate.IsValidColumn(1));
        Assert.IsTrue(Coordinate.IsValidColumn(2));
        Assert.IsTrue(Coordinate.IsValidColumn(3));
        Assert.IsTrue(Coordinate.IsValidColumn(4));
        Assert.IsTrue(Coordinate.IsValidColumn(5));
        Assert.IsTrue(Coordinate.IsValidColumn(6));
        Assert.IsTrue(Coordinate.IsValidColumn(7));
    }

    [TestMethod]
    public void IsValidRow_GivenValidInt_ShouldReturnTrue()
    {
        Assert.IsTrue(Coordinate.IsValidRow(0));
        Assert.IsTrue(Coordinate.IsValidRow(1));
        Assert.IsTrue(Coordinate.IsValidRow(2));
        Assert.IsTrue(Coordinate.IsValidRow(3));
        Assert.IsTrue(Coordinate.IsValidRow(4));
        Assert.IsTrue(Coordinate.IsValidRow(5));
        Assert.IsTrue(Coordinate.IsValidRow(6));
        Assert.IsTrue(Coordinate.IsValidRow(7));
    }

    [TestMethod]
    public void IsValidColumn_GivenInvalidChar_ShouldReturnFalse()
    {
        Assert.IsFalse(Coordinate.IsValidColumn('N'));
        Assert.IsFalse(Coordinate.IsValidColumn('X'));
        Assert.IsFalse(Coordinate.IsValidColumn('.'));
        Assert.IsFalse(Coordinate.IsValidColumn('1'));
        Assert.IsFalse(Coordinate.IsValidColumn('6'));
        Assert.IsFalse(Coordinate.IsValidColumn('z'));
    }

    [TestMethod]
    public void IsValidRow_GivenInvalidChar_ShouldReturnFalse()
    {
        Assert.IsFalse(Coordinate.IsValidRow('0'));
        Assert.IsFalse(Coordinate.IsValidRow('9'));
        Assert.IsFalse(Coordinate.IsValidRow('.'));
        Assert.IsFalse(Coordinate.IsValidRow('f'));
        Assert.IsFalse(Coordinate.IsValidRow(';'));
        Assert.IsFalse(Coordinate.IsValidRow('A'));
        Assert.IsFalse(Coordinate.IsValidRow('F'));
        Assert.IsFalse(Coordinate.IsValidRow('G'));
    }
}