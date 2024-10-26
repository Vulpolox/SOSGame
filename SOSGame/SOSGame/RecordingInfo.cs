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

        public bool IsEmpty { get; private set; }

        public bool IsSimpleGame { get; private set; }

        // constructor
        public RecordingInfo(int recordedBoardSize, bool isSimpleGame) 
        {
            this.RecordedMoves = new List<MoveInfo>();
            this.RecordedBoardSize = recordedBoardSize;
            this.IsSimpleGame = isSimpleGame;

            this.IsEmpty = true;
        }

        // method for adding a move to the recording
        public void RecordMove(MoveInfo move) 
        {
            this.IsEmpty = false;
            this.RecordedMoves.Add(move); 
        }
    }
}
