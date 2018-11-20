using System;
using System.Configuration;
using System.Drawing;
using CherwellTest.Utilites;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CherwellTestTests
{
    [TestClass]
    public class TriangleLayoutTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_GetVerticies_ThrowsExceptionWhenColumnOutOfRange()
        {
            var layout = new TriangleLayout(6, 12);
            layout.GetVerticies(new TriangleLocation('G', 10));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_GetVerticies_ThrowsExceptionWhenRowOutOfRange()
        {
            var layout = new TriangleLayout(6, 12);
            layout.GetVerticies(new TriangleLocation('d', 20));
        }

        [TestMethod]
        public void Test_GetVerticies_ReturnTheCorrectLocationForD10()
        {
            var layout = new TriangleLayout(6, 12);
            var triangleVerticies = layout.GetVerticies(new TriangleLocation('D', 10));

            Assert.AreEqual(new Point(40, 30), triangleVerticies.V1, "Unepected vertex 1.");
            Assert.AreEqual(new Point(50, 30), triangleVerticies.V2, "Unepected vertex 2.");
            Assert.AreEqual(new Point(50, 40), triangleVerticies.V3, "Unepected vertex 3.");
        }

        [TestMethod]
        public void Test_GetVerticies_ReturnTheCorrectLocationForF7()
        {
            var layout = new TriangleLayout(6, 12);
            var triangleVerticies = layout.GetVerticies(new TriangleLocation('F', 7));

            Assert.AreEqual(new Point(30, 50), triangleVerticies.V1, "Unepected vertex 1.");
            Assert.AreEqual(new Point(30, 60), triangleVerticies.V2, "Unepected vertex 2.");
            Assert.AreEqual(new Point(40, 60), triangleVerticies.V3, "Unepected vertex 3.");
        }

        [TestMethod]
        public void Test_GetVerticies_ReturnTheCorrectLocationForE12()
        {
            var layout = new TriangleLayout(6, 12);
            var triangleVerticies = layout.GetVerticies(new TriangleLocation('E', 12));

            Assert.AreEqual(new Point(50, 40), triangleVerticies.V1, "Unepected vertex 1.");
            Assert.AreEqual(new Point(60, 40), triangleVerticies.V2, "Unepected vertex 2.");
            Assert.AreEqual(new Point(60, 50), triangleVerticies.V3, "Unepected vertex 3.");
        }

        [TestMethod]
        public void Test_GetVerticies_ReturnTheCorrectLocationForC3()
        {
            var layout = new TriangleLayout(6, 12);
            var triangleVerticies = layout.GetVerticies(new TriangleLocation('C', 3));

            Assert.AreEqual(new Point(10, 20), triangleVerticies.V1, "Unepected vertex 1.");
            Assert.AreEqual(new Point(10, 30), triangleVerticies.V2, "Unepected vertex 2.");
            Assert.AreEqual(new Point(20, 30), triangleVerticies.V3, "Unepected vertex 3.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_GetTriangleLocation_ThrowsExceptionWhenXOutOfMinRange()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(40, 30), new Point(-1, 30), new Point(50, 40));
            layout.GetTriangleLocation(verticies);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_GetTriangleLocation_ThrowsExceptionWhenYOutOfMinRange()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(40, -1), new Point(50, 50), new Point(50, 40));
            layout.GetTriangleLocation(verticies);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_GetTriangleLocation_ThrowsExceptionWhenXOutOfMaxRange()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(40, 30), new Point(50, 30), new Point(61, 40));
            layout.GetTriangleLocation(verticies);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_GetTriangleLocation_ThrowsExceptionWhenYOutOfMaxRange()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(40, 30), new Point(50, 61), new Point(50, 40));
            layout.GetTriangleLocation(verticies);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_GetTriangleLocation_ThrowsExceptionWhenCoordinatesTooWide()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(40, 0), new Point(50, 0), new Point(60, 10));
            var location = layout.GetTriangleLocation(verticies);

            Assert.AreEqual('A', location.Row, "Unepected row.");
            Assert.AreEqual(10, location.Column, "Unepected column.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_GetTriangleLocation_ThrowsExceptionWhenCoordinatesTooTall()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(40, 0), new Point(50, 0), new Point(50, 20));
            var location = layout.GetTriangleLocation(verticies);

            Assert.AreEqual('A', location.Row, "Unepected row.");
            Assert.AreEqual(10, location.Column, "Unepected column.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Test_GetTriangleLocation_ThrowsExceptionWhenTriangleIsInvalid()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(30, 50), new Point(30, 60), new Point(40, 50));
            var location = layout.GetTriangleLocation(verticies);
        }

        [TestMethod]
        public void Test_GetTriangleLocation_ReturnTheCorrectLocationForA10()
        {
            var layout = new TriangleLayout(6, 12);
            var verticies = new TriangleVerticies(new Point(40, 0), new Point(50, 0), new Point(50, 10));
            var location = layout.GetTriangleLocation(verticies);

            Assert.AreEqual('A', location.Row, "Unepected row.");
            Assert.AreEqual(10, location.Column, "Unepected column.");
        }

        [TestMethod]
        public void Test_GetTriangleLocation_All()
        {
            var layout = new TriangleLayout(6, 12);

            for (char rowIndex = 'A'; rowIndex <= 'F'; rowIndex++)
            {
                for (int colIndex = 1; colIndex <= 12; colIndex++)
                {
                    var verticies = layout.GetVerticies(new TriangleLocation(rowIndex, colIndex));
                    var location = layout.GetTriangleLocation(verticies);

                    Assert.AreEqual(rowIndex, location.Row, $"Unepected row ({rowIndex},{colIndex}).");
                    Assert.AreEqual(colIndex, location.Column, $"Unepected column ({rowIndex},{colIndex}).");
                }
            }
        }
    }
}
