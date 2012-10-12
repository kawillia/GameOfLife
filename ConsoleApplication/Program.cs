using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameOfLife.Core;
using System.Threading;

namespace GameOfLife.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new Grid(100, 100);
            RandomlySeedGrid(grid);

            while (true)
            {
                grid.Update();

                var cells = grid.GetCells();
                DisplayCells(cells);
                Thread.Sleep(500);
            }
        }

        private static void RandomlySeedGrid(Grid grid)
        {
            var numberOfCells = grid.NumberOfRows * grid.NumberOfColumns;
            var random = new Random();

            for (var i = 0; i < numberOfCells; i++)
            {
                var randomRowNumber = random.Next(1, grid.NumberOfRows);
                var randomColumnNumber = random.Next(1, grid.NumberOfColumns);

                grid.BringToLife(randomRowNumber, randomColumnNumber);
            }
        }

        private static void DisplayCells(Boolean[,] cells)
        {
            var numberOfRows = cells.GetLength(0);
            var numberOfColumns = cells.GetLength(1);

            for (var i = 0; i < numberOfColumns; i++)
            {
                for (var j = 0; j < numberOfRows; j++)
                {
                    if (cells[j, i])
                        Console.Write('X');
                    else
                        Console.Write('-');
                }

                Console.Write(Environment.NewLine);
            }

            Console.Write(Environment.NewLine);
        }
    }
}
