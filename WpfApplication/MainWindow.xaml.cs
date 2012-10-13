using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameOfLife.Core;

namespace GameOfLife.WpfApplication
{
    public partial class MainWindow : Window
    {
        private const Int32 NumberOfRows = 40;
        private const Int32 NumberOfColumns = 40;
        private const Int32 TickDelay = 250;

        private BackgroundWorker backgroundWorker;
        private Boolean restart = false;

        public MainWindow()
        {
            InitializeComponent();
            CreateGrid();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void CreateGrid()
        {
            dynamicGrid.ColumnDefinitions.Clear();
            dynamicGrid.RowDefinitions.Clear();

            var rowHeight = dynamicGrid.Height / (Double)NumberOfRows;
            var columnHeight = dynamicGrid.Width / (Double)NumberOfColumns;

            for (var i = 0; i < NumberOfRows; i++)
            {
                var row = new RowDefinition();
                row.Height = new GridLength(rowHeight);
                dynamicGrid.RowDefinitions.Add(row);

                for (var j = 0; j < NumberOfColumns; j++)
                {
                    var column = new ColumnDefinition();
                    column.Width = new GridLength(columnHeight);
                    dynamicGrid.ColumnDefinitions.Add(column);
                }
            }
        }

        protected void replayButton_Click(Object sender, RoutedEventArgs e)
        {
            if (backgroundWorker.WorkerSupportsCancellation)
            {
                backgroundWorker.CancelAsync();
                restart = true;
            }
        }

        protected void backgroundWorker_RunWorkerCompleted(Object sender, RunWorkerCompletedEventArgs e)
        {
            if (restart)
            {
                backgroundWorker.RunWorkerAsync();
                restart = false;
            }
        }

        protected void backgroundWorker_ProgressChanged(Object sender, ProgressChangedEventArgs e)
        {
            var liveCells = e.UserState as IEnumerable<Coordinate>;
            UpdateDisplay(liveCells);
        }

        protected void backgroundWorker_DoWork(Object sender, DoWorkEventArgs e)
        {
            var grid = new LifeGrid(NumberOfRows, NumberOfColumns);
            RandomGridSeeder.Seed(grid);

            while (true)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    break;
                }

                grid.Tick();

                var cells = grid.GetLiveCells();
                backgroundWorker.ReportProgress(0, cells);
                Thread.Sleep(TickDelay);
            }
        }
        
        private void UpdateDisplay(IEnumerable<Coordinate> liveCells)
        {
            dynamicGrid.Children.Clear();

            foreach (var liveCell in liveCells)
            {
                var panel = new DockPanel();
                panel.Background = new SolidColorBrush(Colors.Black);

                Grid.SetColumn(panel, liveCell.X);
                Grid.SetRow(panel, liveCell.Y);

                dynamicGrid.Children.Add(panel);
            }
        }
    }
}
