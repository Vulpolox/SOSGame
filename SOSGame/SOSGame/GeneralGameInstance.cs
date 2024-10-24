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
    public class GeneralGameInstance : GameInstance
    {
        public GeneralGameInstance(GUIHandler GUIRef, GameLogicHandler gameLogicHandler) : base(GUIRef, gameLogicHandler)
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
