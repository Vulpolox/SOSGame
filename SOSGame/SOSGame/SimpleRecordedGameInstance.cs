using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myra;
using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SOSGame
{
    public  class SimpleRecordedGameInstance : SimpleGameInstance
    {
        public SimpleRecordedGameInstance(GUIHandler GUIRef, GameLogicHandler gameLogicHandler) : base(GUIRef, gameLogicHandler)
        {

        }

        public override void HandleMove(MoveInfo moveInfo)
        {
            base.HandleMove(moveInfo);

            // record the move
            GUIRef.recordingInfo.RecordMove(moveInfo);
        }
    }
}
