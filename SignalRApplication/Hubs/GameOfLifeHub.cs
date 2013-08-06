using System;
using System.Collections.Generic;
using GameOfLife.Core;
using Microsoft.AspNet.SignalR;

namespace GameOfLife.SignalRApplication.Hubs
{
    public class GameOfLifeHub : Hub
    {
        private readonly GameOfLifeExecutor gameOfLifeExecutor;

        public GameOfLifeHub() : this(GameOfLifeExecutor.Instance) { }

        public GameOfLifeHub(GameOfLifeExecutor gameOfLifeExecutor)
        {
            this.gameOfLifeExecutor = gameOfLifeExecutor;
        }

        public Dimensions GetDimensions()
        {
            return new Dimensions
            {
                NumberOfRows = GameOfLifeExecutor.NumberOfRows,
                NumberOfColumns = GameOfLifeExecutor.NumberOfColumns
            };
        }

        public void Restart()
        {
            gameOfLifeExecutor.Restart();
        }

        public class Dimensions
        {
            public Int32 NumberOfColumns { get; set; }
            public Int32 NumberOfRows { get; set; }            
        }
    }
}