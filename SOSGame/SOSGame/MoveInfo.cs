using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class MoveInfo
    {
        public int RowIndex { get; set; }       // the row index of the move
        public int ColumnIndex { get; set; }    // the column index of the move
        public String MoveLetter { get; set; }  // "S" or "O"

        public MoveInfo(int rowIndex, int columnIndex, String moveLetter)
        {
            this.RowIndex = rowIndex;
            this.ColumnIndex = columnIndex;
            this.MoveLetter = moveLetter;
        }
    }
}
