using GameOfLife.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GameOfLife.UnitTests
{
    [TestClass]
    public class LifeGridTests
    {
        private LifeGrid grid;

        [TestInitialize]
        public void TestInitialize()
        {
            grid = new LifeGrid(4, 4);
        }

        [TestMethod]
        public void AnyLiveCellHavingThreeNeighborsWithFewerThanTwoLiveNeighborsDies()
        {
            grid.BringToLife(1, 1);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 1);
            Assert.IsFalse(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingFiveNeighborsWithFewerThanTwoLiveNeighborsDies()
        {
            grid.BringToLife(1, 2);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 2);
            Assert.IsFalse(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingEightNeighborsWithFewerThanTwoLiveNeighborsDies()
        {
            grid.BringToLife(2, 2);
            grid.Tick();

            var isAlive = grid.IsCellAlive(2, 2);
            Assert.IsFalse(isAlive);
        }

        [TestMethod]
        public void AnyDeadCellHavingThreeNeighborsWithExactlyThreeLiveNeighborsBecomesALiveCell()
        {
            grid.BringToLife(1, 2);
            grid.BringToLife(2, 2);
            grid.BringToLife(2, 1);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 1);
            Assert.IsTrue(isAlive);
        }

        [TestMethod]
        public void AnyDeadCellHavingFiveNeighborsWithExactlyThreeLiveNeighborsBecomesALiveCell()
        {
            grid.BringToLife(1, 1);
            grid.BringToLife(1, 3);
            grid.BringToLife(2, 3);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 2);
            Assert.IsTrue(isAlive);
        }

        [TestMethod]
        public void AnyDeadCellHavingEightNeighborsWithExactlyThreeLiveNeighborsBecomesALiveCell()
        {
            grid.BringToLife(1, 2);
            grid.BringToLife(2, 1);
            grid.BringToLife(1, 1);
            grid.Tick();

            var isAlive = grid.IsCellAlive(2, 2);
            Assert.IsTrue(isAlive);
        }
        
        [TestMethod]
        public void AnyLiveCellHavingFiveNeighborsWithMoreThanThreeLiveNeighborsDies()
        {
            grid.BringToLife(1, 2);
            grid.BringToLife(1, 1);
            grid.BringToLife(1, 3);
            grid.BringToLife(2, 3);
            grid.BringToLife(2, 2);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 2);
            Assert.IsFalse(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingEightNeighborsWithMoreThanThreeLiveNeighborsDies()
        {
            grid.BringToLife(1, 1);
            grid.BringToLife(1, 2);
            grid.BringToLife(1, 3);
            grid.BringToLife(2, 2);
            grid.BringToLife(2, 1);
            
            grid.Tick();

            var isAlive = grid.IsCellAlive(2, 2);
            Assert.IsFalse(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingThreeNeighborsWithExactlyTwoLiveNeighborsLivesOn()
        {
            grid.BringToLife(1, 1);
            grid.BringToLife(1, 2);
            grid.BringToLife(2, 2);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 1);
            Assert.IsTrue(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingFiveNeighborsWithExactlyTwoLiveNeighborsLivesOn()
        {
            grid.BringToLife(1, 2);
            grid.BringToLife(1, 1);
            grid.BringToLife(2, 3);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 2);
            Assert.IsTrue(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingRightNeighborsWithExactlyTwoLiveNeighborsLivesOn()
        {
            grid.BringToLife(2, 2);
            grid.BringToLife(2, 1);
            grid.BringToLife(1, 1);
            grid.Tick();

            var isAlive = grid.IsCellAlive(2, 2);
            Assert.IsTrue(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingThreeNeighborsWithExactlyThreeLiveNeighborsLivesOn()
        {
            grid.BringToLife(1, 1);
            grid.BringToLife(1, 2);
            grid.BringToLife(2, 2);
            grid.BringToLife(2, 1);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 1);
            Assert.IsTrue(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingFiveNeighborsWithExactlyThreeLiveNeighborsLivesOn()
        {
            grid.BringToLife(1, 2);
            grid.BringToLife(1, 1);
            grid.BringToLife(1, 3);
            grid.BringToLife(2, 3);
            grid.Tick();

            var isAlive = grid.IsCellAlive(1, 2);
            Assert.IsTrue(isAlive);
        }

        [TestMethod]
        public void AnyLiveCellHavingEightNeighborsWithExactlyThreeLiveNeighborsLivesOn()
        {
            grid.BringToLife(2, 2);
            grid.BringToLife(1, 2);
            grid.BringToLife(2, 1);
            grid.BringToLife(1, 1);
            grid.Tick();

            var isAlive = grid.IsCellAlive(2, 2);
            Assert.IsTrue(isAlive);
        }
    }
}
