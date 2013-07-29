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
        private readonly static LifeGrid lifeGrid = new LifeGrid(50, 50);

        private readonly TimeSpan updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Object updateLivingCellsLock = new Object();
        private readonly Timer timer;
        private volatile Boolean updatingLivingCells = false;

        private GameOfLifeExecutor(IHubConnectionContext clients)
        {
            Clients = clients;

            RandomGridSeeder.Seed(lifeGrid);

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
            return lifeGrid.GetLivingCells();
        }

        private void UpdateLivingCells(Object state)
        {
            lock (updateLivingCellsLock)
            {
                if (!updatingLivingCells)
                {
                    updatingLivingCells = true;
                    lifeGrid.Tick();
                    Clients.All.updateLivingCells(lifeGrid.GetLivingCells());
                    updatingLivingCells = false;
                }
            }
        }

        public void Restart()
        {
            lock (updateLivingCellsLock)
            {
                if (!updatingLivingCells)
                {
                    updatingLivingCells = true;
                    RandomGridSeeder.Seed(lifeGrid);
                    updatingLivingCells = false;
                }
            }
        }
    }
}