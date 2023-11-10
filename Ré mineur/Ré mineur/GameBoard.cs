using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace Ré_mineur {
    public class GameBoard {

        private Cell[,] grid;
        public int rows { get; }
        public int cols { get; }
        public int bombNumber { get; }

        //indexor, access grid using GameBoard[x,y]
        public Cell this[int x, int y] {
            get => grid[x, y];
            set => grid[x, y] = value;
        }

         //instantiate as a board of x and y size.
        public GameBoard(int x, int y, int bombs) { 
            rows= x;
            cols= y;
            grid= new Cell[rows, cols];

            //generate cells
            for (int i=0; i < rows; i++) {
                for(int j=0; j < cols; j++) {
                    grid[i,j] = new Cell();
                }
            }
            this.bombNumber= bombs;
        }

        public void start() {

            //BOMB PLACEMENT -------------------------------------------
            Random random = new Random();
            int bombRow, bombCol;

            for (int i = 0; i < bombNumber; i++) {

                bombRow = random.Next(rows);
                bombCol = random.Next(cols);
                this.bomb(bombRow, bombCol); 
            }
            //ADJACENT BOMB --------------------------------------------

            for (int i = 0; i < rows; i++) {
                for (int j = 0; j < cols; j++) {
                    if (grid[i, j].IsBomb() == false) {
                        int adjacentBombs = CountAdjacentBombs(i, j);
                        grid[i, j].SetBombNumber(adjacentBombs);
                    }
                }
            }
        }


        public int CountAdjacentBombs(int i, int j) {
            int count = 0;

            //search the 8 cells around
            for (int x = -1; x <= 1; x++) {
                for (int y = -1; y <= 1; y++) {
                    int newRow = i + x;
                    int newCol = j + y;

                    // verify if in limits
                    if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols) {
                        if (grid[newRow, newCol].IsBomb()) {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        public void reveal(int row, int col) {
            grid[row, col].Reveal();
        }

        public void flag(int row, int col) {

            grid[row, col].ToggleFlag();
        }

        public void bomb(int row, int col) {
            grid[row, col].Bomb();
        }
    }
}
