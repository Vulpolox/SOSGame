using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class GameRecording : GameInstance
    {
        private RecordingInfo moves;

        public GameRecording(GUIHandler GUIRef, GameLogicHandler gameLogicHandler, int recordedSize) : base(GUIRef, gameLogicHandler, recordedSize)
        {

        }

        public override void HandleSOS(SOSInfo sosInfo)
        {
            throw new NotImplementedException();
        }

        public override void HandleFullBoard()
        {
            throw new NotImplementedException();
        }
    }
}
