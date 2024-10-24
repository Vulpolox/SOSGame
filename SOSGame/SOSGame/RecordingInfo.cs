using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class RecordingInfo
    {
        // List for holding moves for playback
        public List<MoveInfo> RecordedMoves { get; }
        public int RecordedBoardSize { get; }

        // constructor
        public RecordingInfo(int recordedBoardSize) 
        {
            this.RecordedMoves = new List<MoveInfo>();
            this.RecordedBoardSize = recordedBoardSize;
        }

        // method for adding a move to the recording
        public void RecordMove(MoveInfo move) { this.RecordedMoves.Add(move); }
    }
}
