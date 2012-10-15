using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using GameOfLife.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameOfLife.UnitTests
{
    [TestClass]
    public class LinqStyleGridSpeedTests
    {
        private const Int32 NumberOfRows = 1000;
        private const Int32 NumberOfColumns = 1000;

        private LinqStyleGrid grid;

        [TestInitialize]
        public void TestInitialize()
        {
            var numberOfCells = NumberOfRows * NumberOfColumns;
            var random = new Random();
            var aliveCoordinates = new HashSet<Coordinate>();

            for (var i = 0; i < numberOfCells; i++)
            {
                var randomRowNumber = random.Next(1, NumberOfRows);
                var randomColumnNumber = random.Next(1, NumberOfColumns);

                aliveCoordinates.Add(new Coordinate(randomColumnNumber - 1, randomRowNumber - 1));
            }

            grid = new LinqStyleGrid(aliveCoordinates.ToArray());
        }

        [TestMethod, Ignore]
        public void TempGridHandlesLargeGrid()
        {
            var startTime = DateTime.Now;
            grid.CreateNextGeneration();
            var endTime = DateTime.Now;

            var timeSpan = endTime - startTime;
            var message = "Timespan was " + timeSpan;
            Assert.IsTrue(timeSpan < new TimeSpan(0, 0, 0, 0, 500), message);
            Debug.WriteLine(message);
        }
    }
}
