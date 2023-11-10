using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ré_mineur {

    public enum CellState {
        Hidden,
        Revealed,
        Bomb,
        Flagged
    }
    public class Cell {
        public CellState cellState { get; private set; }
        public int bombNumber { get; private set; }

        public string CurrentImageUri {
            get {
                switch (cellState) {        //switch that links the state to the image to draw.
                    case CellState.Hidden:
                        return "Pack://siteoforigin:,,,/Assets/TileUnknown.png";
                    case CellState.Revealed:
                        return "Pack://siteoforigin:,,,/Assets/TileEmpty.png";
                    case CellState.Flagged:
                        return "Pack://siteoforigin:,,,/Assets/TileFlag.png";
                    default:
                        return "Pack://siteoforigin:,,,/Assets/TileUnknown.png";
                }
            }
        }

        public bool isBomb {get; private set;}

        public Cell(CellState state = CellState.Hidden, int bombNumber = 0) {
            this.cellState = state;
            this.bombNumber = bombNumber;
            this.isBomb = false;
        }

        public void Reveal() {

            if (this.cellState == CellState.Flagged) return; //safety if flagged tile

            this.cellState = CellState.Revealed;
        }

        public void Bomb() {
            this.isBomb = true;
            this.bombNumber = -1;
        }

        public void Hide() {
            this.cellState = CellState.Hidden;
        }

        public void ToggleFlag() {
            if (this.cellState == CellState.Hidden) {
                this.cellState = CellState.Flagged;
            }
            else if (this.cellState == CellState.Flagged) {
                this.cellState = CellState.Hidden;
            }
        }
        public bool IsRevealed() {
            return this.cellState == CellState.Revealed;
        }
        public bool IsBomb() {
            return this.isBomb;
        }

        public void SetBombNumber(int number) {
            // Logic to set the number of adjacent bombs
            this.bombNumber = number;
        }
    }
}

    