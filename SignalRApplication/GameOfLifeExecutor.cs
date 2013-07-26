using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
        private ConcurrentBag<Coordinate> livingCells = new ConcurrentBag<Coordinate>();
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
            return livingCells;
        }

        private void UpdateLivingCells(Object state)
        {
            lock (updateLivingCellsLock)
            {
                if (!updatingLivingCells)
                {
                    updatingLivingCells = true;
                    lifeGrid.Tick();
                    livingCells = new ConcurrentBag<Coordinate>();

                    foreach (var livingCell in lifeGrid.GetLiveCellCoordinates())
                        livingCells.Add(livingCell);

                    var testList = new[] { new Coordinate(1, 2) };

                    Clients.All.updateLivingCells(testList);
                    updatingLivingCells = false;
                }
            }
        }
    }
}