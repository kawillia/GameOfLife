using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using GameOfLife.Core;
using GameOfLife.SignalRApplication.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace GameOfLife.SignalRApplication
{
    public class GameOfLifeExecutor
    {
        public const Int32 NumberOfRows = 100;
        public const Int32 NumberOfColumns = 100;

        private readonly static Lazy<GameOfLifeExecutor> instance = new Lazy<GameOfLifeExecutor>(() => new GameOfLifeExecutor(GlobalHost.ConnectionManager.GetHubContext<GameOfLifeHub>().Clients));
        private readonly static Lazy<LifeGrid> lifeGrid = new Lazy<LifeGrid>(() => new LifeGrid(50, 50));
        private readonly static Lazy<IGridSeeder> gridSeeder = new Lazy<IGridSeeder>(() => new RandomGridSeeder());

        private readonly IHubConnectionContext clients;
        private readonly Timer timer;
        private readonly TimeSpan updateInterval = TimeSpan.FromMilliseconds(250);

        private GameOfLifeExecutor(IHubConnectionContext clients)
        {
            this.clients = clients;
            gridSeeder.Value.Seed(lifeGrid.Value);
            timer = new Timer(UpdateLivingCells, null, updateInterval, updateInterval);
        }

        public static GameOfLifeExecutor Instance
        {
            get { return instance.Value; }
        }

        public IEnumerable<Coordinate> GetLivingCells()
        {
            return lifeGrid.Value.GetLivingCells();
        }

        private void UpdateLivingCells(Object state)
        {
            lifeGrid.Value.Tick();
            clients.All.updateLivingCells(lifeGrid.Value.GetLivingCells());
        }

        public void Restart()
        {
            gridSeeder.Value.Seed(lifeGrid.Value);
        }
    }
}