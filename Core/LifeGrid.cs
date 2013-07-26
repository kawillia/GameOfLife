using System;
using System.Collections.Generic;

namespace GameOfLife.Core
{
    public class LifeGrid
    {
        public Int32 NumberOfRows { get; set; }
        public Int32 NumberOfColumns { get; set; }

        private Boolean[,] cells;
        private Boolean[,] copiedCells;

        public LifeGrid(Int32 numberOfRows, Int32 numberOfColumns)
        {
            NumberOfRows = numberOfRows;
            NumberOfColumns = numberOfColumns;

            cells = new Boolean[numberOfRows, numberOfColumns];
        }

        public void BringToLife(Int32 rowNumber, Int32 columnNumber)
        {
            cells[rowNumber - 1, columnNumber - 1] = true;
        }

        public void Tick()
        {
            copiedCells = new Boolean[NumberOfRows, NumberOfColumns];
            Array.Copy(cells, copiedCells, cells.Length);
            
            for (var x = 0; x < NumberOfColumns; x++)
                ProcessCell(x, 0);

            for (var y = 0; y < NumberOfRows; y++)
                ProcessCell(0, y);

            for (var x = 0; x < NumberOfColumns; x++)
                ProcessCell(x, NumberOfRows - 1);

            for (var y = 0; y < NumberOfRows; y++)
                ProcessCell(NumberOfColumns - 1, y);

            for (var y = 1; y < NumberOfRows - 1; y++)
                for (var x = 1; x < NumberOfColumns - 1; x++)
                    ProcessCell(x, y);

            cells = copiedCells;
        }

        private void ProcessCell(Int32 x, Int32 y)
        {
            copiedCells[y, x] = ShouldBeAlive(cells[y, x], x, y);
        }

        private Boolean ShouldBeAlive(Boolean isAlive, Int32 x, Int32 y)
        {
            var numberOfLiveNeighbors = 0;
            var numberOfDeadNeighbors = 0;
            var neighbors = GetNeighbors(x, y);

            foreach (var neighbor in neighbors)
            {
                if (neighbor)
                    numberOfLiveNeighbors++;
                else
                    numberOfDeadNeighbors++;

                if (numberOfLiveNeighbors > 3 || numberOfDeadNeighbors == 7)
                    return false;
            }

            return numberOfLiveNeighbors == 3 || 
                   (isAlive && numberOfLiveNeighbors == 2);
        }

        private IEnumerable<Boolean> GetNeighbors(Int32 x, Int32 y)
        {
            // Top
            if (y > 0)
            {
                yield return cells[y - 1, x];

                // Top Right
                if (x < NumberOfColumns - 1)
                    yield return cells[y - 1, x + 1];
            }

            // Right
            if (x < NumberOfColumns - 1)
            {
                yield return cells[y, x + 1];

                // Bottom Right
                if (y < NumberOfRows - 1)
                    yield return cells[y + 1, x + 1];
            }
            
            // Bottom
            if (y < NumberOfRows - 1)
            {
                yield return cells[y + 1, x];

                // Bottom Left
                if (x > 0)
                    yield return cells[y + 1, x - 1];
            }

            // Left
            if (x > 0)
            {
                yield return cells[y, x - 1];

                // Top Left
                if (y > 0)
                    yield return cells[y - 1, x - 1];
            }
        }

        public Boolean IsCellAlive(Int32 columnNumber, Int32 rowNumber)
        {
            return cells[rowNumber - 1, columnNumber - 1];
        }

        public IEnumerable<Coordinate> GetLivingCells()
        {
            var liveCells = new List<Coordinate>();

            for (var i = 0; i < NumberOfRows; i++)
                for (var j = 0; j < NumberOfColumns; j++)
                    if (cells[i, j])
                        liveCells.Add(new Coordinate(j, i));

            return liveCells;
        }
    }
}
