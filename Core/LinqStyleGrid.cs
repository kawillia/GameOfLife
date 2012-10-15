using System.Collections.Generic;
using System.Linq;

namespace GameOfLife.Core
{
    public class LinqStyleGrid
    {
        private readonly ISet<Coordinate> _aliveCoordinates;

        public LinqStyleGrid(params Coordinate[] aliveCoordinatesSeed)
        {
            _aliveCoordinates = new HashSet<Coordinate>(aliveCoordinatesSeed);
        }

        public LinqStyleGrid CreateNextGeneration()
        {
            var keepAliveCoordinates = _aliveCoordinates
                .Where(c => GetNumberOfAliveNeighboursOf(c) == 2 || GetNumberOfAliveNeighboursOf(c) == 3);
 
            var reviveCoordinates = _aliveCoordinates
                .SelectMany(GetDeadNeighboursOf)
                .Where(c => GetNumberOfAliveNeighboursOf(c) == 3);

            return new LinqStyleGrid(keepAliveCoordinates.Union(reviveCoordinates).ToArray());
        }
 
        private IEnumerable<Coordinate> GetDeadNeighboursOf(Coordinate coordinate) {
            return GetNeighboursOf(coordinate).Where(c => !IsAlive(c)); 
        }
 
        private static IEnumerable<Coordinate> GetNeighboursOf(Coordinate coordinate) {
            return Enumerable.Range(-1, 3).SelectMany(
                    x => Enumerable.Range(-1, 3).Select(y => new Coordinate(coordinate.X + x, coordinate.Y + y)))
                    .Except(new []{coordinate});
        }
 
        private int GetNumberOfAliveNeighboursOf(Coordinate coordinate) {
            return GetNeighboursOf(coordinate).Count(IsAlive); 
        }
 
        public bool IsAlive(Coordinate coordinate) {
            return _aliveCoordinates.Contains(coordinate);
        }
    }
}
