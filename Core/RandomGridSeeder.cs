using System;

namespace GameOfLife.Core
{
    public class RandomGridSeeder
    {
        public static void Seed(LifeGrid grid)
        {
            var numberOfCells = grid.NumberOfRows * grid.NumberOfColumns;
            var random = new Random();

            for (var x = 0; x < grid.NumberOfColumns; x++)
            {
                for (var y = 0; y < grid.NumberOfRows; y++)
                {
                    var randomNumber = random.Next(0, 5);

                    if (randomNumber == 1)
                        grid.BringToLife(y + 1, x + 1);
                }
            }
        }
    }
}
