using System;
using System.Collections.Generic;

namespace Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            MovingObjectData[] movingObjects = {
                new MovingObjectData(new Vector2(5, 5)),
                new MovingObjectData(new Vector2(10, 10)),
                new MovingObjectData(new Vector2(15, 15)) };

            TrajectorySimulator trajectorySimulator = new TrajectorySimulator(movingObjects);
            trajectorySimulator.Simulate();
        }
    }


    public class TrajectorySimulator
    {
        private MovingObjectData[] movingObjects;
        private ConsoleWriter consoleWriter;
        private Random random;

        public TrajectorySimulator(MovingObjectData[] objects)
        {
            movingObjects = objects;
            consoleWriter = new ConsoleWriter();
            random = new Random();
        }

        public void Simulate()
        {
            while (true)
            {
                ValidateAndSetObjectsStates();
                ShiftObjectsPosition();
                SendPositionsToConsole();
            }
        }

        private void ValidateAndSetObjectsStates()
        {
            for (int i = 0; i < movingObjects.Length; i++)
            {
                int j = i + 1;

                while (j < movingObjects.Length)
                {
                    Vector2 pos1 = movingObjects[i].Position;
                    Vector2 pos2 = movingObjects[j].Position;

                    if (pos1.x == pos2.x && pos1.y == pos2.y)
                    {
                        movingObjects[i].KillObject();
                        movingObjects[j].KillObject();
                    }

                    j++;
                }
            }
        }

        private void ShiftObjectsPosition()
        {
            foreach (var obj in movingObjects)
            {
                if (obj.IsAlive)
                    obj.ShiftPosition(new Vector2(random.Next(-1, 1), random.Next(-1, 1)));
            }
        }

        private void SendPositionsToConsole()
        {
            for (int i = 0; i < movingObjects.Length; i++)
            {
                if (movingObjects[i].IsAlive)
                    consoleWriter.WritePositionToConsole(movingObjects[i].Position, i + 1);
            }
        }
    }

    public class MovingObjectData
    {
        public Vector2 Position { get; private set; }
        public bool IsAlive { get; private set; } = true;

        public MovingObjectData(Vector2 startPosition)
        {
            Position = ValidateCoordinates(startPosition);
        }

        public void ShiftPosition(Vector2 shift)
        {
            Position = ValidateCoordinates(Position + shift);
        }

        public void KillObject()
        {
            IsAlive = false;
        }

        private Vector2 ValidateCoordinates(Vector2 position)
        {
            if (position.x < 0)
                position.x = 0;

            if (position.y < 0)
                position.y = 0;

            return position;
        }
    }

    public class ConsoleWriter
    {
        public void WritePositionToConsole(Vector2 position, int number)
        {
            Console.SetCursorPosition(position.x, position.y);
            Console.Write(number);
        }
    }

    public struct Vector2
    {
        public int x;
        public int y;

        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 f1, Vector2 f2) { return new Vector2(f1.x + f2.x, f1.y + f2.y); }
    }
}