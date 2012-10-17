using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SharpGL.SceneGraph;
using SharpGL;
using GameOfLife.Core;
using System.Diagnostics;

namespace GameOfLife.SharpGlWpfApplication
{
    public partial class MainWindow : Window
    {
        private const Int32 NumberOfRows = 200;
        private const Int32 NumberOfColumns = 200;
        private const Int32 TickDelay = 250;
        private const Int32 ArrowKeySensitivity = 2;

        private Stopwatch stopwatch;
        private OpenGL gl;
        private LifeGrid grid;
        private Double cameraRotation;
        private Double cameraX;
        private Double cameraY;
        private Double cameraZ;
        private Random random = new Random();

        public MainWindow()
        {
            InitializeComponent();
            grid = new LifeGrid(NumberOfRows, NumberOfColumns);
            stopwatch = new Stopwatch();
            StartSimulation();
        }

        private void StartSimulation()
        {
            RandomGridSeeder.Seed(grid);
            stopwatch.Start();
        }

        private void openGLControl_OpenGLDraw(object sender, OpenGLEventArgs args)
        {
            gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();

            if (stopwatch.ElapsedMilliseconds >= TickDelay)
            {
                grid.Tick();
                stopwatch.Restart();
            }

            var liveCells = grid.GetLiveCellCoordinates();

            foreach (var liveCell in liveCells)
                DrawCube(liveCell.X, liveCell.Y);
        }

        private void DrawCube(Single x, Single y)
        {
            gl.Color(0.0f, 0.0f, 1.0f);

            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(x, y, 0.0f);
            gl.Vertex(x + 1, y, 0.0f);
            gl.Vertex(x + 1, y + 1, 0.0f);
            gl.Vertex(x, y + 1, 0.0f);
            gl.End();

            gl.Color(1.0f, 0.0f, 0.0);
            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(x + 1, y, 0.0f);
            gl.Vertex(x + 1, y + 1, 0.0f);
            gl.Vertex(x + 1, y + 1, -1.0f);
            gl.Vertex(x + 1, y, -1.0f);
            gl.End();

            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(x, y + 1, 0.0f);
            gl.Vertex(x, y + 1, -1.0f);
            gl.Vertex(x, y, -1.0f);
            gl.Vertex(x, y, 0.0f);
            gl.End();

            gl.Color(0.0f, 0.0f, 1.0f);
            // Top
            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(x, y + 1, -1.0f);
            gl.Vertex(x + 1, y + 1, -1.0f);
            gl.Vertex(x + 1, y + 1, 0.0f);
            gl.Vertex(x, y + 1, 0.0f);
            gl.End();

            // Bottom
            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(x, y, -1.0f);
            gl.Vertex(x + 1, y, -1.0f);
            gl.Vertex(x + 1, y, 0.0f);
            gl.Vertex(x, y, 0.0f);
            gl.End();

            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(x + 1, y, -1.0f);
            gl.Vertex(x + 1, y + 1, -1.0f);
            gl.Vertex(x, y + 1, -1.0f);
            gl.Vertex(x, y, -1.0f);
            gl.End();
        }

        private void openGLControl_OpenGLInitialized(object sender, OpenGLEventArgs args)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
        }

        private void openGLControl_Resized(object sender, OpenGLEventArgs args)
        {
            gl = openGLControl.OpenGL;
            cameraX = NumberOfColumns / 2;
            cameraY = NumberOfRows / 2;
            cameraZ = (NumberOfColumns / 2) * Math.Tan(45);
            UpdateCamera();
        }

        private void mainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                StartSimulation();
            else if (e.Key == Key.Down)
                cameraY -= ArrowKeySensitivity;
            else if (e.Key == Key.Up)
                cameraY += ArrowKeySensitivity;
            else if (e.Key == Key.Left)
                cameraX -= ArrowKeySensitivity;
            else if (e.Key == Key.Right)
                cameraX += ArrowKeySensitivity;

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 1000.0);
            gl.LookAt(cameraX, cameraY, cameraZ, cameraX, cameraY, cameraZ - 1f, 0, 1.0, 0);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void mainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            cameraZ += e.Delta * 3 / Math.Abs(e.Delta);
            UpdateCamera();
        }
    }
}
