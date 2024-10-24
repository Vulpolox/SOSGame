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
    public class SimpleGameInstance : GameInstance
    {
        public SimpleGameInstance(GUIHandler GUIRef, GameLogicHandler gameLogicHandler) : base(GUIRef, gameLogicHandler)
        {

        }

        public override void HandleSOS(SOSInfo sosInfo)
        {
            this.gameLogicHandler.ChangeTurns();
            Console.WriteLine("Todo: handle sos for simple game");
        }

        public override void HandleFullBoard()
        {
            throw new NotImplementedException();
        }

    }
}
