using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Media;

namespace MazeSolver
{
    public class MazeContent
    {
        private MazeCell[,] MazeGrid;
        private Random RandomNumber;
        Dispatcher Dispatcher;

        public MazeContent(MazeCell[,] MazeGrid)
        {
            this.Dispatcher = Dispatcher.CurrentDispatcher;
            this.MazeGrid = MazeGrid;
            RandomNumber = new Random();
        }

        public void GenerateMaze(int xCells, int yCells)
        {
            Stack<MazeCell> oldCells = new Stack<MazeCell>();
            MazeCell currentCell = MazeGrid[0, 0];
            MazeCell nextCell;

            while (true)
            {
                Direction[] directions = (Direction[])Enum.GetValues(typeof(Direction));
                while (directions.Length > 0)
                {
                    Direction move = GetRandomDirection(directions);
                    nextCell = null;

                    switch (move)
                    {
                        case Direction.Down:
                            if (currentCell.Y + 1 < yCells)
                            {
                                nextCell = MazeGrid[currentCell.X, currentCell.Y + 1];
                            }
                            break;
                        case Direction.Left:
                            if (currentCell.X - 1 >= 0)
                            {
                                nextCell = MazeGrid[currentCell.X - 1, currentCell.Y];
                            }
                            break;
                        case Direction.Right:
                            if (currentCell.X + 1 < xCells)
                            {
                                nextCell = MazeGrid[currentCell.X + 1, currentCell.Y];
                            }
                            break;
                        case Direction.Up:
                            if (currentCell.Y - 1 >= 0)
                            {
                                nextCell = MazeGrid[currentCell.X, currentCell.Y - 1];
                            }
                            break;
                    }

                    if (nextCell != null && !nextCell.Touched)
                    {
                        currentCell.OpenWall(move);
                        nextCell.OpenWall(OpositeDirection(move));

                        oldCells.Push(currentCell);
                        currentCell = nextCell;
                        break;
                    }
                    else
                    {
                        List<Direction> tmpDirections = directions.ToList();
                        tmpDirections.Remove(move);
                        directions = tmpDirections.ToArray();
                    }
                }

                if (directions.Length == 0)
                {
                    if (oldCells.Count == 0)
                    {
                        break;
                    }
                    currentCell = oldCells.Pop();
                }
            }
        }

        private Direction OpositeDirection(Direction oldDirection)
        {
            switch (oldDirection)
            {
                case Direction.Down:
                    return Direction.Up;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Left:
                    return Direction.Right;
                default:
                    return 0;
            }
        }

        private Direction GetRandomDirection(Array possibleDirections)
        {
            return (Direction)possibleDirections.GetValue(RandomNumber.Next(possibleDirections.Length));
        }

        //Assumes that the maze can be solved
        public bool SolveMaze(int startX, int startY, int endX, int endY, int oldX, int oldY)
        {
            MazeCell currentCell = MazeGrid[startX, startY];

            this.Dispatcher.Invoke(delegate ()
            {
                currentCell.CellContent.Background = Brushes.Red;
            });

            System.Threading.Thread.Sleep(10);
            if (startX == endX && startY == endY)
            {
                return true;
            }

            bool foundPath = false;

            if (currentCell.OpenDirections.Contains(Direction.Right) && oldX - 1 != startX)
            {
                foundPath = SolveMaze(startX + 1, startY, endX, endY, startX, startY);
            }

            if (currentCell.OpenDirections.Contains(Direction.Down) && oldY - 1 != startY && !foundPath)
            {
                foundPath = SolveMaze(startX, startY + 1, endX, endY, startX, startY);
            }

            if (currentCell.OpenDirections.Contains(Direction.Left) && oldX != startX - 1 && !foundPath)
            {
                foundPath = SolveMaze(startX - 1, startY, endX, endY, startX, startY);
            }

            if (currentCell.OpenDirections.Contains(Direction.Up) && startY - 1 != oldY && !foundPath)
            {
                foundPath = SolveMaze(startX, startY - 1, endX, endY, startX, startY);
            }

            if (!foundPath)
            {
                this.Dispatcher.Invoke(delegate ()
                {
                    currentCell.CellContent.Background = Brushes.White;
                });
            }

            return foundPath;
        }
    }
}
