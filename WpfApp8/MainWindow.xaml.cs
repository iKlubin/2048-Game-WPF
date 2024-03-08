using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace WpfApp8
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int[,] gameGrid = new int[4, 4];
        private Random random = new Random();
        private int score = 0;
        private int highScore = 0;
        private int value = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
            UpdateScoreLabels();
        }

        private void UpdateScoreLabels()
        {
            scoreLbl.Content = score.ToString();
            highScoreLbl.Content = highScore.ToString();
        }

        private void AddScore(int value)
        {
            score += value;
            if (score > highScore)
            {
                highScore = score;
            }
            UpdateScoreLabels();
        }

        private void InitializeGame()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    gameGrid[i, j] = 0;
                }
            }
            AddNewTile();
            AddNewTile();
            UpdateGameGrid();
        }

        private void AddNewTile()
        {
            int emptyCellCount = CountEmptyCells();
            if (emptyCellCount == 0) return;

            int targetCell = random.Next(emptyCellCount);
            int cellIndex = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameGrid[i, j] == 0)
                    {
                        if (cellIndex == targetCell)
                        {
                            gameGrid[i, j] = random.Next(10) == 0 ? 4 : 2;
                            return;
                        }
                        cellIndex++;
                    }
                }
            }
        }

        private int CountEmptyCells()
        {
            int count = 0;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameGrid[i, j] == 0) count++;
                }
            }
            return count;
        }

        private void UpdateGameGrid()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Button button = (Button)GameGrid.Children[i * 4 + j];
                    if (gameGrid[i, j] == 0)
                    {
                        button.Style = (Style)FindResource("emp");
                        button.Content = "";
                    }
                    else
                    {
                        switch (gameGrid[i, j])
                        {
                            case 2:
                                button.Style = (Style)FindResource("cell2");
                                break;
                            case 4:
                                button.Style = (Style)FindResource("cell4");
                                break;
                            case 8:
                                button.Style = (Style)FindResource("cell8");
                                break;
                            case 16:
                                button.Style = (Style)FindResource("cell16");
                                break;
                            case 32:
                                button.Style = (Style)FindResource("cell32");
                                break;
                            case 64:
                                button.Style = (Style)FindResource("cell64");
                                break;
                            case 128:
                                button.Style = (Style)FindResource("cell128");
                                break;
                            default:
                                button.Style = (Style)FindResource("cell128");
                                break;
                        }

                        button.Content = gameGrid[i, j].ToString();
                    }
                }
            }
        }
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            bool moved = false;
            value = 0;

            switch (e.Key)
            {
                case Key.Up:
                    MoveUp();
                    moved = true;
                    break;
                case Key.Down:
                    MoveDown();
                    moved = true;
                    break;
                case Key.Left:
                    MoveLeft();
                    moved = true;
                    break;
                case Key.Right:
                    MoveRight();
                    moved = true;
                    break;
            }

            if (moved)
            {
                AddScore(value);

                if (!CanMove())
                {
                    MessageBox.Show("Game Over!");
                    value = 0;
                    score = 0;
                    UpdateScoreLabels();
                    InitializeGame();
                }
            }

            UpdateGameGrid();
        }

        private bool CanMove()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameGrid[i, j] == 0)
                    {
                        return true;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gameGrid[i, j] == gameGrid[i, j + 1] ||
                        gameGrid[j, i] == gameGrid[j + 1, i])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void MoveUp()
        {
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (gameGrid[i, j] != 0)
                    {
                        int k = i;
                        while (k > 0 && gameGrid[k - 1, j] == 0)
                        {
                            gameGrid[k - 1, j] = gameGrid[k, j];
                            gameGrid[k, j] = 0;
                            k--;
                            moved = true;
                        }
                        if (k > 0 && gameGrid[k - 1, j] == gameGrid[k, j])
                        {
                            gameGrid[k - 1, j] *= 2;
                            value += gameGrid[k - 1, j];
                            gameGrid[k, j] = 0;
                            moved = true;
                        }
                    }
                }
            }
            if (moved) AddNewTile();
        }

        private void MoveDown()
        {
            bool moved = false;
            for (int j = 0; j < 4; j++)
            {
                for (int i = 3; i >= 0; i--)
                {
                    if (gameGrid[i, j] != 0)
                    {
                        int k = i;
                        while (k < 3 && gameGrid[k + 1, j] == 0)
                        {
                            gameGrid[k + 1, j] = gameGrid[k, j];
                            gameGrid[k, j] = 0;
                            k++;
                            moved = true;
                        }
                        if (k < 3 && gameGrid[k + 1, j] == gameGrid[k, j])
                        {
                            gameGrid[k + 1, j] *= 2;
                            value += gameGrid[k + 1, j];
                            gameGrid[k, j] = 0;
                            moved = true;
                        }
                    }
                }
            }
            if (moved) AddNewTile();
        }

        private void MoveLeft()
        {
            bool moved = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (gameGrid[i, j] != 0)
                    {
                        int k = j;
                        while (k > 0 && gameGrid[i, k - 1] == 0)
                        {
                            gameGrid[i, k - 1] = gameGrid[i, k];
                            gameGrid[i, k] = 0;
                            k--;
                            moved = true;
                        }
                        if (k > 0 && gameGrid[i, k - 1] == gameGrid[i, k])
                        {
                            gameGrid[i, k - 1] *= 2;
                            value += gameGrid[i, k - 1];
                            gameGrid[i, k] = 0;
                            moved = true;
                        }
                    }
                }
            }
            if (moved) AddNewTile();
        }

        private void MoveRight()
        {
            bool moved = false;
            for (int i = 0; i < 4; i++)
            {
                for (int j = 3; j >= 0; j--)
                {
                    if (gameGrid[i, j] != 0)
                    {
                        int k = j;
                        while (k < 3 && gameGrid[i, k + 1] == 0)
                        {
                            gameGrid[i, k + 1] = gameGrid[i, k];
                            gameGrid[i, k] = 0;
                            k++;
                            moved = true;
                        }
                        if (k < 3 && gameGrid[i, k + 1] == gameGrid[i, k])
                        {
                            gameGrid[i, k + 1] *= 2;
                            value += gameGrid[i, k + 1];
                            gameGrid[i, k] = 0;
                            moved = true;
                        }
                    }
                }
            }
            if (moved) AddNewTile();
        }
    }
}
