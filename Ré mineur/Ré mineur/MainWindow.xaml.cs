using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace Ré_mineur {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private GameBoard gameBoard;    //game logic
        private GameDisplay gameDisplay;    //game display

        public static bool debug = false;   //if true, debug mode

        public MainWindow() {
            InitializeComponent();

            gameBoard = new GameBoard(16, 30, 99); //set size of grid with x and y

            gameDisplay = new GameDisplay(GameGrid, gameBoard);

            Loaded += (sender, e) => {
                GameGrid.PreviewMouseDown += CellClickEvent; //add event for leeft click
                gameBoard.start();
                gameDisplay.DrawGrid(); //display the board
            };

        }
        private void CellClickEvent(object sender, MouseButtonEventArgs e) {

            Image clickedImage = e.OriginalSource as Image;

            if (clickedImage != null) {
                int row = Grid.GetRow(clickedImage);
                int col = Grid.GetColumn(clickedImage);

                if (debug == true) System.Diagnostics.Debug.WriteLine("row:" + row +" col: " + col);

                if (e.ChangedButton == MouseButton.Left) {
                    gameDisplay.Reveal(row, col);
                }

                else if (e.ChangedButton == MouseButton.Right) {
                   gameBoard.flag(row, col);   //flag logic
                    gameDisplay.UpdateCell(row, col); // Update the cell sprite
                }
            }
        }
    }
}
