using Castle.Components.DictionaryAdapter;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class GameLogicHandler
    {
        // flags
        private bool isSimpleGame;
        private bool isRedComputer;
        private bool isBlueComputer;
        private bool isRedTurn = false;
        private bool isBlueTurn = true;

        // board variables
        private int boardSize;
        private List<List<String>> internalBoardState;

        // score counters
        private int blueScore;
        private int redScore;


        // constructor using information from the GUI
        public GameLogicHandler(GUIHandler GUIRef)
        {
            this.isSimpleGame = GUIRef.IsSimpleGame();
            this.isRedComputer = GUIRef.IsRedComputer();
            this.isBlueComputer = GUIRef.IsBlueComputer();
            this.boardSize = GUIRef.GetBoardSize();
            this.internalBoardState = InitializeBoardState();
        }


        // overloaded constructor for creating testable instances of the class that don't rely on GUI
        public GameLogicHandler (
            bool isSimpleGame = false,
            bool isRedComputer = false,
            bool isBlueComputer = false,
            bool isRedTurn = false,
            bool isBlueTurn = true,
            int boardSize = 3)
        {
            this.isSimpleGame = isSimpleGame;
            this.isRedComputer = isRedComputer;
            this.isBlueComputer = isBlueComputer;
            this.isRedTurn = isRedTurn;
            this.isBlueTurn = isBlueTurn;
            this.boardSize = boardSize;

            this.internalBoardState = InitializeBoardState();
        }


        // accessors and mutators
        public bool IsSimpleGame() { return isSimpleGame; }
        public bool IsRedComputer() { return isRedComputer; }
        public bool IsBlueComputer() { return isBlueComputer; }
        public bool IsRedTurn() { return isRedTurn; }
        public bool IsBlueTurn() { return isBlueTurn; }  
        public int GetBoardSize() { return boardSize; }
        public List<List<String>> GetInternalBoardState() { return internalBoardState; }


        // methods
        public String GetPlayerTurnColorName()  { return this.isRedTurn ? "Red" : "Blue"; }

        public void ChangeTurns()
        {
            this.isRedTurn = !this.isRedTurn;
            this.isBlueTurn = !this.isBlueTurn;
        }

        public void HandleSOS(int previousMoveRowIndex, int previousMoveColumnIndex)
        {
            // TODO
            this.ChangeTurns();
        }


        // method for initilizing the internalBoardState ; all elements start as "EMPTY"
        public List<List<String>> InitializeBoardState()
        {
            List<List<String>> returnList = new List<List<String>>();
            List<String> rowToAdd;

            for (int r = 0; r < this.boardSize; r++)
            {
                rowToAdd = new List<String>();

                for (int c = 0; c < this.boardSize; c++)
                {
                    rowToAdd.Add("EMPTY");
                }
                returnList.Add(rowToAdd);
            }

            return returnList;
        }


        // method for updating the internal board state at index [r][c]
        public void UpdateInternalBoardState(int r, int c, String playerSOChoice)
        {
            if (_IsOutOfBounds(r, c))  { Console.WriteLine("Tried to access internal state at invalid index"); }
            else                       { this.internalBoardState[r][c] = playerSOChoice; }
        }


        // method for checking whether internalBoardState[r][c] is OOB
        private bool _IsOutOfBounds(int r, int c)  { return (Math.Max(r, c) >= this.boardSize); }


        // method for returning the contents of internalBoardState[r][c] ; also returns "OOB" if passed indices correspond to OOB index
        public String internalBoardStateRef(int r, int c)
        {
            if (_IsOutOfBounds(r, c)) { return "OOB"; }
            else { return this.internalBoardState[r][c]; }
        }



        // ************************************************************************************************* //



        // algorithms
        public bool CheckForSOS(int previousMoveR, int previousMoveC)
        {
            return false;
            // TODO
        }

        private void _DrawLine(Vector2 startPos, Vector2 direction, Color color)
        {
            // TODO
        }
    }
}