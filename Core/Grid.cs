using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameOfLife.Core
{
    public class Grid
    {
        private Boolean[,] cells;

        public Grid(Int32 numberOfRows, Int32 numberOfColumns)
        {
            cells = new Boolean[numberOfRows, numberOfColumns];

            for (var i = 0; i < numberOfRows; i++)
                for (var j = 0; j < numberOfColumns; j++)
                    cells[i, j] = false;
        }

        public void BringToLife(Int32 rowNumber, Int32 columnNumber)
        {
            cells[rowNumber - 1, columnNumber - 1] = true;
        }

        public void Update()
        {
            var copiedCells = new Boolean[cells.GetLength(0), cells.GetLength(1)];
            Array.Copy(cells, copiedCells, cells.Length);

            for (var i = 0; i < cells.GetLength(0); i++)
            {
                for (var j = 0; j < cells.GetLength(1); j++)
                {
                    var neighbors = GetNeighbors(i, j);
                    var numberOfLiveNeighbors = neighbors.Count(n => n);

                    if (cells[i, j])
                        copiedCells[i, j] = numberOfLiveNeighbors > 1 && numberOfLiveNeighbors < 4;
                    else
                        copiedCells[i, j] = numberOfLiveNeighbors == 3;
                }
            }

            Array.Copy(copiedCells, cells, copiedCells.Length);
        }

        private IEnumerable<Boolean> GetNeighbors(Int32 rowIndex, Int32 columnIndex)
        {
            var neighbors = new List<Boolean>();
            var numberOfRows = cells.GetLength(0);
            var numberOfColumns = cells.GetLength(1);

            //for (var i = -1; i <= 1; i++)
            //{
            //    for (var j = -1; j <= 1; j++)
            //    {
            //        var neighborRowIndex = rowIndex + i;
            //        var neighborColumnIndex = columnIndex + j;

            //        if (neighborRowIndex != rowIndex && neighborColumnIndex != columnIndex)
            //            if (AreValidNeighborIndices(neighborRowIndex, neighborColumnIndex))
            //                neighbors.Add(cells[neighborRowIndex, neighborColumnIndex]);
            //    }
            //}

            // Above
            if (rowIndex > 0)
                neighbors.Add(cells[rowIndex - 1, columnIndex]);

            // Top Right
            if (rowIndex > 0 && columnIndex < numberOfColumns - 1)
                neighbors.Add(cells[rowIndex - 1, columnIndex + 1]);

            // Right
            if (columnIndex < numberOfColumns - 1)
                neighbors.Add(cells[rowIndex, columnIndex + 1]);

            // Bottom Right
            if (rowIndex < numberOfRows - 1 && columnIndex < numberOfColumns - 1)
                neighbors.Add(cells[rowIndex + 1, columnIndex + 1]);

            // Bottom
            if (rowIndex < numberOfRows - 1)
                neighbors.Add(cells[rowIndex + 1, columnIndex]);

            // Bottom Left
            if (rowIndex < numberOfRows - 1 && columnIndex > 0)
                neighbors.Add(cells[rowIndex + 1, columnIndex - 1]);

            // Left
            if (columnIndex > 0)
                neighbors.Add(cells[rowIndex, columnIndex - 1]);

            // Top Left
            if (rowIndex > 0 && columnIndex > 0)
                neighbors.Add(cells[rowIndex - 1, columnIndex - 1]);

            return neighbors;
        }

        private Boolean AreValidNeighborIndices(Int32 neighborRowIndex, Int32 neighborColumnIndex)
        {
            var numberOfRows = cells.GetLength(0);
            var numberOfColumns = cells.GetLength(1);

            return neighborRowIndex >= 0 &&
                   neighborRowIndex < numberOfRows &&
                   neighborColumnIndex >= 0 &&
                   neighborColumnIndex < numberOfColumns - 1;
        }

        public Boolean IsCellAlive(Int32 rowNumber, Int32 columnNumber)
        {
            return cells[rowNumber - 1, columnNumber - 1];
        }
    }
}
