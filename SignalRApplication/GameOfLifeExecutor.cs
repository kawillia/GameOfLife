using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
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
        private readonly static Lazy<LifeGrid> lifeGrid = new Lazy<LifeGrid>(() => new LifeGrid(NumberOfRows, NumberOfColumns));

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