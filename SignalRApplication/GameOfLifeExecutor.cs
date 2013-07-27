using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using GameOfLife.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace SignalRApplication
{
    public class GameOfLifeExecutor
    {
        private readonly static Lazy<GameOfLifeExecutor> instance = new Lazy<GameOfLifeExecutor>(() => new GameOfLifeExecutor(GlobalHost.ConnectionManager.GetHubContext<GameOfLifeHub>().Clients));
        private readonly static Lazy<LifeGrid> lifeGrid = new Lazy<LifeGrid>(() => new LifeGrid(50, 50));

        private readonly IHubConnectionContext clients;
        private readonly Timer timer;
        private readonly TimeSpan updateInterval = TimeSpan.FromMilliseconds(250);
        private readonly Object updateLivingCellsLock = new Object();        
        private volatile Boolean updatingLivingCells = false;

        private GameOfLifeExecutor(IHubConnectionContext clients)
        {
            this.clients = clients;
            timer = new Timer(UpdateLivingCells, null, updateInterval, updateInterval);
            RandomGridSeeder.Seed(lifeGrid.Value);
        }

        public static GameOfLifeExecutor Instance
        {
            get { return instance.Value; }
        }

        private void UpdateLivingCells(Object state)
        {
            lock (updateLivingCellsLock)
            {
                if (!updatingLivingCells)
                {
                    updatingLivingCells = true;
                    lifeGrid.Value.Tick();
                    clients.All.updateLivingCells(lifeGrid.Value.GetLivingCells());
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
                    RandomGridSeeder.Seed(lifeGrid.Value);
                    updatingLivingCells = false;
                }
            }
        }
    }
}