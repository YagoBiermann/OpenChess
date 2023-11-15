using System.Diagnostics;

namespace OpenChess.Domain.Tests;

[TestClass]
public class CoordinateTests
{
    [TestMethod]
    public void NewInstance_GivenTwoNumbers_ShouldConvertToAlgebraicNotation()
    {
        Coordinate coordinate = new(0, 0);
        Coordinate coordinate2 = new(5, 2);
        Coordinate coordinate3 = new(7, 7);


        Assert.AreEqual("A1", coordinate.ToString());
        Assert.AreEqual("F3", coordinate2.ToString());
        Assert.AreEqual("H8", coordinate3.ToString());
    }

    [TestMethod]
    public void NewInstance_ColNumberLessThanZero_ShouldThrowException()
    {
        Assert.ThrowsException<CoordinateException>(() => new Coordinate(-1, 0));
    }

    [TestMethod]
    public void NewInstance_RowNumberLessThanZero_ShouldThrowException()
    {
        Assert.ThrowsException<CoordinateException>(() => new Coordinate(0, -1));
    }

    [TestMethod]
    public void NewInstance_ColNumberGreaterThanSeven_ShouldThrowException()
    {
        Assert.ThrowsException<CoordinateException>(() => new Coordinate(8, 0));
    }

    [TestMethod]
    public void NewInstance_RowNumberGreaterThanSeven_ShouldThrowException()
    {
        Assert.ThrowsException<CoordinateException>(() => new Coordinate(0, 8));
    }

    [TestMethod]
    public void NewInstance_GivenAlgebraicNotation_ShouldInstantiateNewCoordinate()
    {
        Coordinate coordinate = new("A1");
        Coordinate coordinate2 = new("C4");
        Coordinate coordinate3 = new("H8");

        Assert.AreEqual("A1", coordinate.ToString());
        Assert.AreEqual("C4", coordinate2.ToString());
        Assert.AreEqual("H8", coordinate3.ToString());
    }


    [TestMethod]
    public void NewInstance_GivenInvalidAlgebraicNotation_ShouldThrowException()
    {
        Assert.ThrowsException<CoordinateException>(() => new Coordinate("K1"));
        Assert.ThrowsException<CoordinateException>(() => new Coordinate("V2"));
        Assert.ThrowsException<CoordinateException>(() => new Coordinate("11"));
        Assert.ThrowsException<CoordinateException>(() => new Coordinate(".2"));
        Assert.ThrowsException<CoordinateException>(() => new Coordinate("A0"));
        Assert.ThrowsException<CoordinateException>(() => new Coordinate("A9"));
        Assert.ThrowsException<CoordinateException>(() => new Coordinate("A10"));
    }

    [TestMethod]
    public void Equals_ObjectsWithSameAlgebraicNotation_ShouldReturnTrue()
    {
        Coordinate coordinate1 = new("A1");
        Coordinate coordinate2 = new("A1");

        Assert.IsTrue(coordinate1.Equals(coordinate2));
    }

    [TestMethod]
    public void Equals_ObjectsWithDifferentAlgebraicNotation_ShouldReturnFalse()
    {
        Coordinate coordinate1 = new("A1");
        Coordinate coordinate2 = new("A2");

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
}