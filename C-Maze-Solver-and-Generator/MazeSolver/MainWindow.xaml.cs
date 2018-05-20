using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MazeSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MazeInfo MazeInfo;
        private MazeContent maze;

        public MainWindow()
        {
            InitializeComponent();
            MazeInfo = new MazeInfo();
            MazeInfo.MazeWidth = "20";
            MazeInfo.MazeHeight = "20";
            this.DataContext = MazeInfo;
            MazeCell[,] grid = CreateGrid(Convert.ToInt32( MazeInfo.MazeWidth), Convert.ToInt32(MazeInfo.MazeHeight));
            maze = new MazeContent(grid);
            maze.GenerateMaze(Convert.ToInt32( MazeInfo.MazeWidth), Convert.ToInt32(MazeInfo.MazeHeight));
        }

        private MazeCell[,] CreateGrid(int xCells, int yCells)
        {
            MazeCell[,]  MazeGrid = new MazeCell[xCells,yCells];

            for(int i = 0; i < yCells; i++)
            {
                Maze.RowDefinitions.Add(new RowDefinition());

                for (int k = 0; k < xCells; k++)
                {
                    if(i == 0)
                    {
                        Maze.ColumnDefinitions.Add(new ColumnDefinition());
                    }

                    MazeGrid[k, i] = new MazeCell(k, i);


                    if (i == 0 && k == 0)
                    {
                        MazeGrid[k, i].CellContent.Background = Brushes.Blue;
                    } else if(i == yCells - 1 && k == xCells - 1)
                    {
                        MazeGrid[k, i].CellContent.Background = Brushes.Green;
                    }

                    MazeGrid[k, i].CellContent.SetValue(Grid.RowProperty, i);
                    MazeGrid[k, i].CellContent.SetValue(Grid.ColumnProperty, k);

                    Maze.Children.Add(MazeGrid[k, i].CellContent);
                }
            }

            return MazeGrid;
        }

        private void Generate_Maze(object sender, RoutedEventArgs e)
        {
            Maze.Children.Clear();
            Maze.RowDefinitions.Clear();
            Maze.ColumnDefinitions.Clear();
            MazeCell[,] grid = CreateGrid(Convert.ToInt32( MazeInfo.MazeWidth), Convert.ToInt32(MazeInfo.MazeHeight));
            maze = new MazeContent(grid);
            maze.GenerateMaze(Convert.ToInt32( MazeInfo.MazeWidth), Convert.ToInt32(MazeInfo.MazeHeight));
        }

        private void Solve_Maze(object sender, RoutedEventArgs e)
        {
            new Task(() => { maze.SolveMaze(0, 0, Convert.ToInt32( MazeInfo.MazeWidth) - 1, Convert.ToInt32(MazeInfo.MazeHeight) - 1, -1, -1); }).Start();
        }
    }
}
