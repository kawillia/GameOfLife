using System;
using System.Threading;
using GameOfLife.Core;

namespace GameOfLife.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var grid = new LifeGrid(100, 100);
            RandomlySeedGrid(grid);

            while (true)
            {
                grid.Tick();

                var cells = grid.GetCells();
                DisplayCells(cells);
                Thread.Sleep(500);
            }
        }

        private static void RandomlySeedGrid(LifeGrid grid)
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
