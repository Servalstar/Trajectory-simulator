using System;
using System.Collections.Generic;

namespace Task
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<MovingObject> movingObjects = new List<MovingObject>() {
                new MovingObject(new Vector2(5, 5)),
                new MovingObject(new Vector2(10, 10)),
                new MovingObject(new Vector2(15, 15)) };

            TrajectorySimulator trajectorySimulator = new TrajectorySimulator(movingObjects);
            trajectorySimulator.Simulate();
        }
    }


    public class TrajectorySimulator
    {
        private List<MovingObject> movingObjects;
        private ConsoleWriter consoleWriter;
        private Random random;

        public TrajectorySimulator(List<MovingObject> objects)
        {
            movingObjects = objects;
            consoleWriter = new ConsoleWriter();
            random = new Random();
        }

        public void Simulate()
        {
            while (true)
            {
                RemoveCrossedObjects();
                SimulateMovement();
                SendPositionsToConsole();
            }
        }

        private void RemoveCrossedObjects()
        {
            List<MovingObject> nonCrossedObjects = new List<MovingObject>();

            foreach (var movingObject in movingObjects)
                if (CheckCrossingObjects(movingObject, movingObjects) == false)
                    nonCrossedObjects.Add(movingObject);

            movingObjects = nonCrossedObjects;
        }

        private bool CheckCrossingObjects(MovingObject checkedObject, List<MovingObject> objects)
        {
            for (int i = objects.IndexOf(checkedObject) + 1; i < objects.Count; i++)
                if (checkedObject.Position == objects[i].Position)
                    return true;

            return false;
        }

        private void SimulateMovement()
        {
            foreach (var obj in movingObjects)
                obj.ShiftPosition(new Vector2(random.Next(-1, 1), random.Next(-1, 1)));
        }

        private void SendPositionsToConsole()
        {
            for (int i = 0; i < movingObjects.Count; i++)
                consoleWriter.WritePositionToConsole(movingObjects[i].Position, i + 1);
        }
    }

    public class MovingObject
    {
        public Vector2 Position { get; private set; }

        public MovingObject(Vector2 startPosition)
        {
            Position = ClampCoordinates(startPosition);
        }

        public void ShiftPosition(Vector2 shift)
        {
            Position = ClampCoordinates(Position + shift);
        }

        private Vector2 ClampCoordinates(Vector2 position)
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
        public static bool operator ==(Vector2 f1, Vector2 f2) { return f1.x == f2.x && f1.y == f2.y; }
        public static bool operator !=(Vector2 f1, Vector2 f2) { return f1.x != f2.x && f1.y != f2.y; }
    }
}