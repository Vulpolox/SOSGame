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
        public SimpleGameInstance(GUIHandler GUIRef, GameLogicHandler gameLogicHandler, int recordingSize = -1) : base(GUIRef, gameLogicHandler, recordingSize)
        {

        }

        public override void HandleSOS(SOSInfo sosInfo)
        {
            // if the player didn't make an SOS in their turn
            if (sosInfo.NumSOS == 0)
            {
                // switch turns
                this.ChangeTurns();

                // if board is full, call HandleFullBoard() to delcare a tie
                if (this.gameLogicHandler.IsBoardFull()) { HandleFullBoard(); }
            }
            
            // if the player makes an SOS, declare them the winner
            else if (sosInfo.NumSOS > 0)
            {
                // handle drawing lines
                HandleLines(sosInfo);

                // initialize a boolean for whether red or blue won and pass it to the Win() function
                bool isRedWon = sosInfo.IsRedTurn;
                this.Win(isRedWon);
            }

        }

        // HandleFullBoard should only be called if the board is filled
        // by a move that won't create an SOS in a simple game
        public override void HandleFullBoard()  { this.Draw(); }

    }
}
