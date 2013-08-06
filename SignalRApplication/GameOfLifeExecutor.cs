using System;
using System.Collections.Generic;
using System.Threading;
using GameOfLife.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRApplication
{
    public class GameOfLifeExecutor
    {
        private readonly static Lazy<GameOfLifeExecutor> instance = new Lazy<GameOfLifeExecutor>(() => new GameOfLifeExecutor(GlobalHost.ConnectionManager.GetHubContext<GameOfLifeHub>().Clients));
        private readonly static Lazy<LifeGrid> lifeGrid = new Lazy<LifeGrid>(() => new LifeGrid(50, 50));
        private readonly static Lazy<IGridSeeder> gridSeeder = new Lazy<IGridSeeder>(() => new RandomGridSeeder());

        private readonly TimeSpan updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Timer timer;

        private GameOfLifeExecutor(IHubConnectionContext clients)
        {
            Clients = clients;
            gridSeeder.Value.Seed(lifeGrid.Value);
            timer = new Timer(UpdateLivingCells, null, updateInterval, updateInterval);
        }

        public static GameOfLifeExecutor Instance
        {
            get { return instance.Value; }
        }

        private IHubConnectionContext Clients
        {
            get;
            set;
        }

        public IEnumerable<Coordinate> GetLivingCells()
        {
            return lifeGrid.Value.GetLivingCells();
        }

        private void UpdateLivingCells(Object state)
        {
            lifeGrid.Value.Tick();
            Clients.All.updateLivingCells(lifeGrid.Value.GetLivingCells());
        }

        public void Restart()
        {
            gridSeeder.Value.Seed(lifeGrid.Value);
        }
    }
}