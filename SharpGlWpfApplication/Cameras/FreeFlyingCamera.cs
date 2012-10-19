using System.Windows.Media.Media3D;
using System;
using MathNet.Numerics;

namespace GameOfLife.SharpGlWpfApplication.Cameras
{
    public class FreeFlyingCamera
    {
        public Vector3D Position { get; private set; }
        public Vector3D Direction { get; private set; }
        public Vector3D Up { get; private set; }
        public Vector3D Target { get; private set; }
        public Vector3D Right { get; private set; }

        public FreeFlyingCamera()
        {
            Position = new Vector3D(0, 0, 1);
            Direction = new Vector3D(0, 0, -1);
            Target = new Vector3D(0, 0, -1);
            Up = new Vector3D(0, 1, 0);
            Right = new Vector3D(1, 0, 0);
        }

        public void SetZPosition(Double z)
        {
            Position = new Vector3D(Position.X, Position.Y, z);
        }

        public void MoveForward(Single velocity)
        {
            Position += Direction * velocity;
            UpdateSettings();
        }

        public void MoveSide(Single velocity)
        {
            Position += Right * velocity;
            UpdateSettings();
        }

        public void MoveUp(Single velocity)
        {
            Position += Up * velocity;
            UpdateSettings();
        }

        public void Rotate(Vector3D rotationAmount)
        {
            var rotationAroundX = new Matrix3D();
            rotationAroundX.Rotate(new Quaternion(Right, Trig.DegreeToRadian(rotationAmount.Y)));

            var rotationAroundY = new Matrix3D();
            rotationAroundY.Rotate(new Quaternion(new Vector3D(0, 1, 0), Trig.DegreeToRadian(rotationAmount.X)));

            var rotationMatrix = Matrix3D.Multiply(rotationAroundX, rotationAroundY);
            Direction = Vector3D.Multiply(Direction, rotationMatrix);
            Up = Vector3D.Multiply(Up, rotationMatrix);

            UpdateSettings();
        }

        private void UpdateSettings()
        {
            Target = Position + Direction;
            Direction = Target - Position;
            Direction.Normalize();

            Up.Normalize();

            Right = Vector3D.CrossProduct(Direction, Up);
            Right.Normalize();
        }
    }
}
