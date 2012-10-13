using System;
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
            var startTime = DateTime.Now;
            grid.Tick();
            var endTime = DateTime.Now;

            Assert.IsTrue(endTime - startTime < new TimeSpan(0, 0, 0, 0, 500));
        }
    }
}
