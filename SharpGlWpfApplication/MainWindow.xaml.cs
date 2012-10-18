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
using System.Timers;

namespace GameOfLife.SharpGlWpfApplication
{
    public partial class MainWindow : Window
    {
        private const Int32 NumberOfRows = 150;
        private const Int32 NumberOfColumns = 150;
        private const Int32 TickDelay = 250;
        private const Int32 ArrowKeySensitivity = 2;        
        private const Double CameraRotationSpeed = 10;

        private Timer timer;
        private OpenGL gl;
        private LifeGrid lifeGrid;
        private Vector3 cameraPosition;
        private Vector3 cameraForward;
        private Double cameraAngle = 90;
        private Random random = new Random();
        private Double lastMouseX;
        private Double cameraRadius = NumberOfRows * 2;

        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = TickDelay;
            timer.Elapsed += (s, a) => lifeGrid.Tick();
            cameraPosition = new Vector3(0, 0, 0);
            cameraForward = new Vector3(0, 0, 0);
        }

        private void StartSimulation()
        {
            lifeGrid = new LifeGrid(NumberOfRows, NumberOfColumns);
            RandomGridSeeder.Seed(lifeGrid);
            timer.Start();
        }

        private void openGLControl_OpenGLDraw(Object sender, OpenGLEventArgs args)
        {
            gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            DrawLiveCells();
        }

        private void DrawLiveCells()
        {
            var liveCells = lifeGrid.GetLiveCellCoordinates();

            foreach (var liveCell in liveCells)
                DrawCube(liveCell.X, liveCell.Y);
        }

        private Double ToRadians(Double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        private void DrawCube(Single x, Single y)
        {
            x -= NumberOfColumns / 2;
            y -= NumberOfRows / 2;
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

        private void openGLControl_OpenGLInitialized(Object sender, OpenGLEventArgs args)
        {
            gl = openGLControl.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
            StartSimulation();
        }

        private void openGLControl_Resized(Object sender, OpenGLEventArgs args)
        {
            UpdateCamera();
        }

        private void mainWindow_KeyDown(Object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                StartSimulation();
            else if (e.Key == Key.Down)
                cameraPosition.Y -= ArrowKeySensitivity;
            else if (e.Key == Key.Up)
                cameraPosition.Y += ArrowKeySensitivity;

            UpdateCamera();
        }

        private void UpdateCamera()
        {
            cameraPosition.X = cameraRadius * Math.Cos(ToRadians(cameraAngle));
            cameraPosition.Z = cameraRadius * Math.Sin(ToRadians(cameraAngle));

            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(60.0f, (Double)Width / (Double)Height, 0.01, 1000.0);
            gl.LookAt(cameraPosition.X, cameraPosition.Y, cameraPosition.Z, 0, 0, 0, 0, 1.0, 0);
            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void mainWindow_MouseMove(Object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(this);
            var deltaX = mousePosition.X - lastMouseX;

            cameraAngle += deltaX;
            UpdateCamera();
            lastMouseX = mousePosition.X;
        }

        private void mainWindow_MouseWheel(Object sender, MouseWheelEventArgs e)
        {
            if (e.Delta != 0)
                cameraRadius += 3 * e.Delta / Math.Abs(e.Delta);

            UpdateCamera();
        }
    }
}
