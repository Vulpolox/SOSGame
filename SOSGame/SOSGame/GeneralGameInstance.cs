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

        // instance variables
        private int blueScore;
        private int redScore;

        // constructor
        public GeneralGameInstance(GUIHandler GUIRef, GameLogicHandler gameLogicHandler) : base(GUIRef, gameLogicHandler)
        {
            this.blueScore = 0; 
            this.redScore = 0;

            this.GUIRef.UpdateScoreLabel(blueScore, redScore);
        }


        public override void HandleSOS(SOSInfo sosInfo)
        {
            // unpack score into variable for readability
            int moveScore = sosInfo.NumSOS;

            // if no points were earned, change turns
            if (moveScore == 0) { this.ChangeTurns(); }

            // otherwise, add the score to the player who made the move and let them go again
            else
            {
                if (gameLogicHandler.GetPlayerTurnColorName() == "Blue") { this.blueScore += moveScore; }
                else if (gameLogicHandler.GetPlayerTurnColorName() == "Red") { this.redScore += moveScore; }
                else { Console.WriteLine("Error in General Game HandleSOS()"); }
            }

            // update the ScoreLabel of the GUI
            this.GUIRef.UpdateScoreLabel(blueScore, redScore);

            // when the board fills up, call HandleFullBoard
            if (this.gameLogicHandler.IsBoardFull()) { this.HandleFullBoard(); }
        }


        public override void HandleFullBoard()
        {
            bool isRedWon;

            if (this.blueScore == this.redScore) { this.Draw(); }
            else 
            { 
                isRedWon = this.redScore > this.blueScore ? true : false;
                this.Win(isRedWon);
            }

        }

    }
}
