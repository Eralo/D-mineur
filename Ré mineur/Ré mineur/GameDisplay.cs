using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Ré_mineur {
    public class GameDisplay {
        private Grid gameGrid; //canvas from WPF used to display scene
        private GameBoard gameBoard; //game logic
        private Image[,] cellImages; //generates sprites

        public GameDisplay(Grid grid, GameBoard board) {
            gameGrid = grid;
            gameBoard = board;
            cellImages = new Image[board.rows, board.cols];
        }

        public void DrawGrid() {

            gameGrid.RowDefinitions.Clear();
            gameGrid.ColumnDefinitions.Clear();
            gameGrid.Children.Clear();  //clear grid

            //create grid lines and columns

            for (int i = 0; i < gameBoard.rows; i++) {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(1, GridUnitType.Star);  //every line is in star mode
                gameGrid.RowDefinitions.Add(row);
            }
            for (int j = 0; j < gameBoard.cols; j++) {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(1, GridUnitType.Star);  //every column is in star mode
                gameGrid.ColumnDefinitions.Add(col);
            }

            //add the images

            for (int i = 0; i < gameBoard.rows; i++) {
                for (int j = 0; j < gameBoard.cols; j++) {
                    Image cellImage = new Image();
                    cellImage.Stretch = Stretch.Uniform;    //make sure the cell stays in Uniform. might not be useful

                    Cell currentCell = gameBoard[i, j];  //get cell from board
                    string imagePath = currentCell.CurrentImageUri; //get image to use from the cell. Change sprites in Cell.cs

                    cellImage.Source = new BitmapImage(new Uri(imagePath));

                    cellImages[i, j] = cellImage;

                    Grid.SetRow(cellImage, i);
                    Grid.SetColumn(cellImage, j);

                    gameGrid.Children.Add(cellImage);
                }
            }
        }

        public void UpdateCell(int row, int col) {
            //change sprite ------------------------------
            Cell currentCell = gameBoard[row, col];
            cellImages[row, col].Source = new BitmapImage(new Uri(currentCell.CurrentImageUri));

            //add or change number if revealed and not 0 bomb --------------------------------
            if (currentCell.IsRevealed() == true && currentCell.bombNumber != 0) {
                TextBlock cellNum = new TextBlock();
                cellNum.Text = currentCell.bombNumber.ToString();

                Grid.SetRow(cellNum, row);
                Grid.SetColumn(cellNum, col);

                //center on the cell
                cellNum.HorizontalAlignment = HorizontalAlignment.Center;
                cellNum.VerticalAlignment = VerticalAlignment.Center;

                //cell color
                cellNum.Foreground = new SolidColorBrush(Colors.Blue);

                gameGrid.Children.Add(cellNum);
            }


        }

        public void Reveal(int row, int col) {
            //check if still in grid
            if (row < 0 || row >= gameBoard.rows || col < 0 || col >= gameBoard.cols) {
                return;
            }

            //If cell is already revealed or a bomb, return
            if (!gameBoard[row, col].IsRevealable()) {
                return;
            }

            //reveal cell
            gameBoard[row, col].Reveal();
            UpdateCell(row, col); //update UI

            //if cell is 0, reveal adjacent cells
            if (gameBoard[row, col].bombNumber == 0) {
                for (int i = -1; i <= 1; i++) {
                    for (int j = -1; j <= 1; j++) {
                        Reveal(row + i, col + j);
                    }
                }
            }
        }
    }
}
