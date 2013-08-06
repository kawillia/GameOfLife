using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using GameOfLife.Core;
using GameOfLife.SharpGlWpfApplication.Cameras;
using SharpGL;
using SharpGL.SceneGraph;

namespace GameOfLife.SharpGlWpfApplication
{
    public partial class MainWindow : Window
    {
        private const Int32 NumberOfRows = 60;
        private const Int32 NumberOfColumns = 60;
        private const Int32 TickDelay = 250;
        private const Int32 CameraMoveSensitivity = 10;        
        private const Int32 CameraRotationSensitivity = 240;
        private const Int32 GridOffsetX = NumberOfColumns / 2;
        private const Int32 GridOffsetY = NumberOfRows / 2;

        private Timer timer;
        private OpenGL gl;
        private LifeGrid lifeGrid;
        private Vector3D lastMousePosition;
        private FreeFlyingCamera camera;

        public MainWindow()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = TickDelay;
            timer.Elapsed += (s, a) => lifeGrid.Tick();
            camera = new FreeFlyingCamera(new Vector3D(0, 0, 1), new Vector3D(0, 0, -1), new Vector3D(0, 0, 0));
        }

        private void StartSimulation()
        {
            lifeGrid = new LifeGrid(NumberOfRows, NumberOfColumns);
            var randomGridSeeder = new RandomGridSeeder();
            randomGridSeeder.Seed(lifeGrid);
            timer.Start();
        }

        private void openGLControl_OpenGLDraw(Object sender, OpenGLEventArgs args)
        {
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            gl.LoadIdentity();
            DrawLiveCells();
            SetCameraView();
        }

        private void DrawLiveCells()
        {
            var liveCells = lifeGrid.GetLivingCells();

            foreach (var liveCell in liveCells)
                DrawCube(liveCell.X - GridOffsetX, liveCell.Y - GridOffsetY);
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
            gl.Begin(OpenGL.GL_POLYGON);
            gl.Vertex(x, y + 1, -1.0f);
            gl.Vertex(x + 1, y + 1, -1.0f);
            gl.Vertex(x + 1, y + 1, 0.0f);
            gl.Vertex(x, y + 1, 0.0f);
            gl.End();

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

        private void SetCameraView()
        {
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            gl.LoadIdentity();
            gl.Perspective(60.0f, (Double)Width / (Double)Height, 0.01, 1000.0);
            gl.LookAt(
                camera.Position.X,
                camera.Position.Y,
                camera.Position.Z,
                camera.Target.X,
                camera.Target.Y,
                camera.Target.Z,
                camera.Up.X,
                camera.Up.Y,
                camera.Up.Z);

            gl.MatrixMode(OpenGL.GL_MODELVIEW);
        }

        private void openGLControl_OpenGLInitialized(Object sender, OpenGLEventArgs args)
        {
            camera.SetZPosition(NumberOfRows);
            gl = openGLControl.OpenGL;
            gl.ClearColor(0, 0, 0, 0);
            StartSimulation();
        }

        private void openGLControl_Resized(Object sender, OpenGLEventArgs args)
        {
            SetCameraView();
        }

        private void mainWindow_KeyDown(Object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter))
                StartSimulation();

            if (Keyboard.IsKeyDown(Key.W))
                camera.MoveForward(CameraMoveSensitivity);

            if (Keyboard.IsKeyDown(Key.S))
                camera.MoveForward(-CameraMoveSensitivity);

            if (Keyboard.IsKeyDown(Key.A))
                camera.MoveSide(-CameraMoveSensitivity);

            if (Keyboard.IsKeyDown(Key.D))
                camera.MoveSide(CameraMoveSensitivity);

            if (Keyboard.IsKeyDown(Key.Left))
                camera.Rotate(new Vector3D(CameraRotationSensitivity, 0, 0));

            if (Keyboard.IsKeyDown(Key.Right))
                camera.Rotate(new Vector3D(-CameraRotationSensitivity, 0, 0));

            if (Keyboard.IsKeyDown(Key.Up))
                camera.Rotate(new Vector3D(0, CameraRotationSensitivity, 0));

            if (Keyboard.IsKeyDown(Key.Down))
                camera.Rotate(new Vector3D(0, -CameraRotationSensitivity, 0));
        }
        
        private void mainWindow_MouseMove(Object sender, MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(this);

            //if (lastMousePosition != null)
            //{
            //    var deltaX = mousePosition.X - lastMousePosition.X;
            //    var deltaY = mousePosition.Y - lastMousePosition.Y;

            //    deltaX /= deltaX;
            //    deltaY /= deltaY;

            //    RotateCamera(new Vector3D(deltaX, deltaY, 0));
            //}

            lastMousePosition = new Vector3D(mousePosition.X, mousePosition.Y, 0d);
        }
    }
}
