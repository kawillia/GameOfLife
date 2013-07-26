using System.Collections.Generic;
using GameOfLife.Core;
using Microsoft.AspNet.SignalR;

namespace SignalRApplication
{
    public class GameOfLifeHub : Hub
    {
        private readonly GameOfLifeExecutor gameOfLifeExecutor;

        public GameOfLifeHub() : this(GameOfLifeExecutor.Instance) { }

        public GameOfLifeHub(GameOfLifeExecutor gameOfLifeExecutor)
        {
            this.gameOfLifeExecutor = gameOfLifeExecutor;
        }

        public IEnumerable<Coordinate> GetLivingCells()
        {
            return gameOfLifeExecutor.GetLivingCells();
        }

        public void Restart()
        {
            gameOfLifeExecutor.Restart();
        }
    }
}