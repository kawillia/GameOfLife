using System;

namespace GameOfLife.Core
{
    public class RandomGridSeeder
    {
        public static void Seed(LifeGrid grid)
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
    }
}
