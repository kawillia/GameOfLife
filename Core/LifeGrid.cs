using System;
using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Core
{
    public class LifeGrid
    {
        public Int32 NumberOfRows { get; set; }
        public Int32 NumberOfColumns { get; set; }

        private Boolean[,] cells;

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
            var cellsToBringToLife = new List<Coordinate>();

            for (var i = 0; i < NumberOfRows; i++)
            {
                for (var j = 0; j < NumberOfColumns; j++)
                {
                    var neighbors = GetNeighbors(i, j);
                    var numberOfLiveNeighbors = neighbors.Count(n => n);

                    if (ShouldCellBeAlive(cells[i, j], numberOfLiveNeighbors))
                        cellsToBringToLife.Add(new Coordinate(j, i));
                }
            }

            cells = new Boolean[NumberOfRows, NumberOfColumns];

            foreach (var cellToBringToLife in cellsToBringToLife)
                cells[cellToBringToLife.Y, cellToBringToLife.X] = true;
        }

        private bool ShouldCellBeAlive(Boolean isAlive, Int32 numberOfLiveNeighbors)
        {
            return (isAlive && numberOfLiveNeighbors > 1 && numberOfLiveNeighbors < 4) ||
                   (isAlive == false && numberOfLiveNeighbors == 3);
        }

        private IEnumerable<Boolean> GetNeighbors(Int32 rowIndex, Int32 columnIndex)
        {
            var neighbors = new List<Boolean>();

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
            if (rowIndex > 0 && columnIndex < NumberOfColumns - 1)
                neighbors.Add(cells[rowIndex - 1, columnIndex + 1]);

            // Right
            if (columnIndex < NumberOfColumns - 1)
                neighbors.Add(cells[rowIndex, columnIndex + 1]);

            // Bottom Right
            if (rowIndex < NumberOfRows - 1 && columnIndex < NumberOfColumns - 1)
                neighbors.Add(cells[rowIndex + 1, columnIndex + 1]);

            // Bottom
            if (rowIndex < NumberOfRows - 1)
                neighbors.Add(cells[rowIndex + 1, columnIndex]);

            // Bottom Left
            if (rowIndex < NumberOfRows - 1 && columnIndex > 0)
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
            return neighborRowIndex >= 0 &&
                   neighborRowIndex <= NumberOfRows - 1 &&
                   neighborColumnIndex >= 0 &&
                   neighborColumnIndex <= NumberOfColumns - 1;
        }
        
        public Boolean IsCellAlive(Int32 rowNumber, Int32 columnNumber)
        {
            return cells[rowNumber - 1, columnNumber - 1];
        }

        public IEnumerable<Coordinate> GetLiveCells()
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
