using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class MoveInfo
    {
        public int RowIndex { get; set; }      // the row index of the move
        public int ColumnIndex { get; set; }   // the column index of the move
        public String MoveLetter { get; set; } // "S" or "O"
        public int TurnBit { get; set; }       // the player (red or blue) that made the move ; 0 = blue, 1 = red

        public MoveInfo(int rowIndex, int columnIndex, String moveLetter)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.MoveLetter = moveLetter;
        }
    }
}
