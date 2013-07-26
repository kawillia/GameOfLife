using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameOfLife.Core;

namespace GameOfLife.ConsoleApplication
{
    class Program
    {
        private static readonly Int32 NumberOfRows = 4;
        private static readonly Int32 NumberOfColumns = 4;

        static void Main(string[] args)
        {
            var grid = new LifeGrid(NumberOfRows, NumberOfColumns);
            RandomGridSeeder.Seed(grid);

            while (true)
            {
                grid.Tick();

                var cells = grid.GetLivingCells();
                DisplayCells(cells);
                Thread.Sleep(500);
            }
        }

        private static void DisplayCells(IEnumerable<Coordinate> liveCells)
        {
            for (var i = 0; i < NumberOfColumns; i++)
            {
                for (var j = 0; j < NumberOfRows; j++)
                {
                    if (liveCells.Any(c => c.X == i && c.Y == j))
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
