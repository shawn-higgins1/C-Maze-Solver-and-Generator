using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MazeSolver
{
    public class MazeCell
    {
        public Border CellContent { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool Touched { get; private set; }
        public List<Direction> OpenDirections;

        public MazeCell(int x, int y)
        {
            CellContent = new Border
            {
                BorderThickness = new System.Windows.Thickness(1, 1, 1, 1),
                BorderBrush = Brushes.Black
            };
            OpenDirections = new List<Direction>();
            X = x;
            Y = y;
            Touched = false;
        }

        public void OpenWall(Direction direction)
        {
            Touched = true;
            Thickness oldThickness = CellContent.BorderThickness;
            switch (direction)
            {
                case Direction.Down:
                    OpenDirections.Add(Direction.Down);
                    CellContent.BorderThickness = new Thickness(oldThickness.Left, oldThickness.Top, oldThickness.Right, 0);
                    return;
                case Direction.Up:
                    OpenDirections.Add(Direction.Up);
                    CellContent.BorderThickness = new Thickness(oldThickness.Left, 0, oldThickness.Right, oldThickness.Bottom);
                    return;
                case Direction.Left:
                    OpenDirections.Add(Direction.Left);
                    CellContent.BorderThickness = new Thickness(0, oldThickness.Top, oldThickness.Right, oldThickness.Bottom);
                    return;
                case Direction.Right:
                    OpenDirections.Add(Direction.Right);
                    CellContent.BorderThickness = new Thickness(oldThickness.Left, oldThickness.Top, 0, oldThickness.Bottom);
                    return;
            }

        }
    }
}
