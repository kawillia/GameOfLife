using System;
using System.Diagnostics;
using GameOfLife.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameOfLife.UnitTests
{
    [TestClass]
    public class LifeGridSpeedTests
    {
        private LifeGrid grid;

        [TestInitialize]
        public void TestInitialize()
        {
            grid = new LifeGrid(1000, 1000);
            RandomGridSeeder.Seed(grid);
        }

        [TestMethod]
        public void HandlesLargeGrid()
        {
            var stopwatch = Stopwatch.StartNew();
            grid.Tick();
            stopwatch.Stop();

            var message = "Timespan was " + stopwatch.Elapsed;
            Assert.IsTrue(stopwatch.Elapsed < new TimeSpan(0, 0, 0, 0, 500), message);
            Console.WriteLine(message);
        }
    }
}
