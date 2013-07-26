using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using GameOfLife.Core;
using System.Threading;

namespace SignalRApplication
{
    public class GameOfLifeHub : Hub
    {
        private readonly GameOfLifeExecutor gameOfLifeExecutor;

        private const Int32 NumberOfRows = 50;
        private const Int32 NumberOfColumns = 50;
        private const Int32 TickDelay = 250;

        public GameOfLifeHub() : this(GameOfLifeExecutor.Instance) { }

        public GameOfLifeHub(GameOfLifeExecutor gameOfLifeExecutor)
        {
            this.gameOfLifeExecutor = gameOfLifeExecutor;
        }

        public IEnumerable<Coordinate> GetLivingCells()
        {
            return gameOfLifeExecutor.GetLivingCells();
        }
    }
}