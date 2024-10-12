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
    public class GameInstance
    {
        // reference of GUI
        private GUIHandler GUIRef;

        // information from the GUI
        private int size;             // dimensions of the board
        private bool isRedS;          // flag for whether the red player has selected an S
        private bool isBlueS;         // flag for whether the blue player has selected an S
        private bool isSimpleGame;    // flag for whether game mode is set to simple
        private bool isRedComputer;   // flag for whether player is computer
        private bool isBlueComputer;  // flag for whether player is computer

        private bool isRedTurn = false;     // flag for red player's turn
        private bool isBlueTurn = true;     // flag for blue player's turn

        private GameLogicHandler gameLogicHandler;

        private Grid buttonGrid;                     // The board
        private List<List<GridButton>> buttonArray;  // Array for holding references to the cells ("GridButtons") on the board

        Grid outerGrid;               // the outer grid on which the board will be displayed
        private const int _r = 1;     // the row index at which the board will be displayed on the outer grid
        private const int _c = 1;     // the column index at which the board will be displayed on the outer grid


        // constructor
        public GameInstance(GUIHandler GUIRef, GameLogicHandler gameLogicHandler)
        {
            // create references to the GUI and the gameLogicHandler
            this.gameLogicHandler = gameLogicHandler;
            this.GUIRef = GUIRef;

            // get game information from the gameLogicHandler
            this.size = gameLogicHandler.GetBoardSize();
            this.isSimpleGame = gameLogicHandler.IsSimpleGame();
            this.isRedComputer = gameLogicHandler.IsRedComputer();
            this.isBlueComputer = gameLogicHandler.IsBlueComputer();

            // get ref to container on which to display board from the GUI
            this.outerGrid = GUIRef.GetOuterGrid();

            // initialize the buttonArray
            this.buttonArray = new List<List<GridButton>>();

            // update the current turn label
            GUIRef.UpdateTurnLabel(isRedTurn: this.isRedTurn);

            // initialize the board and place it in the outerGrid
            this.buttonGrid = InitializeBoard();
            Grid.SetRow(buttonGrid, _r);
            Grid.SetColumn(buttonGrid, _c);
            this.outerGrid.Widgets.Add(buttonGrid);
        }


        // creates and returns a reference to the grid of buttons used for the board
        public Grid InitializeBoard()
        {
            Grid returnGrid = new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 1,
                Width = 300,
                Height = 300
            };

            buttonArray.Clear();

            List<GridButton> rowToAdd = new List<GridButton>();

            // add columns and rows to the return grid
            for (int i = 0; i < this.size; i++)
            {
                returnGrid.RowsProportions.Add(new Proportion(ProportionType.Part, 1));
                returnGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 1));
            }

            for (int r = 0; r < this.size; r++)
            {
                for (int c = 0; c < this.size; c++)
                {

                    // initialize the GridButton that will be added to the board
                    GridButton buttonToAdd = new GridButton(r, c);
                    buttonToAdd.Click += OnGridButtonClick;
                    
                    // add the grid button to the row vector (which will be appended to the buttonArray)
                    rowToAdd.Add(buttonToAdd);

                    // set the position of and add the GridButton to the buttonGrid
                    Grid.SetRow(buttonToAdd, r);
                    Grid.SetColumn(buttonToAdd, c);
                    returnGrid.Widgets.Add(buttonToAdd);
                }

                // add the temporary row vector to the button array and clear it for the next iteration of outer loop
                this.buttonArray.Add(rowToAdd);
                rowToAdd = new List<GridButton>();
            }
            return returnGrid;
        }

        public void ClearBoard()
        {
            buttonGrid.Widgets.Clear();
        }

        public void ChangeTurns()
        {
            this.isRedTurn = !this.isRedTurn;
            this.isBlueTurn = !this.isBlueTurn;
        }


        public String GetCell(int r, int c)
        {
            if (Math.Max(r, c) > this.buttonArray.Count - 1)
            {
                return "OOB";
            }

            else
            {
                GridButton buttonRef = this.buttonArray[r][c];

                return buttonRef.GetText() == " " ? "EMPTY" : buttonRef.GetText();
            }
        }

        // method that returns the 'S' or the 'O' the current turn's player has selected in the GUI
        public String GetSOChoice()
        {
            String playerSOChoice = "";

            if (this.gameLogicHandler.IsRedTurn()) { playerSOChoice = this.GUIRef.IsRedS() ? "S" : "O"; }
            else if (this.gameLogicHandler.IsBlueTurn()) { playerSOChoice = this.GUIRef.IsBlueS() ? "S" : "O"; }

            return playerSOChoice;
        }


        // action listener for GridButton clicks
        public void OnGridButtonClick(object sender, EventArgs e)
        {
            // find out whether the current turn's player has selected an 'S' or an 'O'
            // in GUI and return it as a String
            String playerSOChoice = this.GetSOChoice();

            // create a reference to the pressed button and its position
            GridButton pressedButton = sender as GridButton;
            int rowIndex = pressedButton.GetRowIndex();
            int columnIndex = pressedButton.GetColumnIndex();

            // set the button's text to the 'S' or 'O' that was returned and disable it
            pressedButton.SetText(playerSOChoice);            
            pressedButton.Enabled = false;

            // update the internal board state
            this.gameLogicHandler.UpdateInternalBoardState(rowIndex, columnIndex, playerSOChoice);

            // handle potential new SOSs
            this.gameLogicHandler.HandleSOS(previousMoveRowIndex: rowIndex,
                                            previousMoveColumnIndex: columnIndex);

            // update the GUI turn label
            this.GUIRef.UpdateTurnLabel(this.gameLogicHandler.IsRedTurn());

            Console.WriteLine(String.Format("Clicked Button At r = {0}, c = {1}", pressedButton.GetRowIndex(), pressedButton.GetColumnIndex()));
            Console.WriteLine(this.GetCell(pressedButton.GetRowIndex() , pressedButton.GetColumnIndex()));
        }


        // getters (for testing)
        public List<List<GridButton>> GetButtonArray() { return this.buttonArray; }
        public bool GetIsSimpleGame() { return this.isSimpleGame; }
        public bool GetIsRedS() { return this.isRedS; }
        public bool GetIsBlueS() { return this.isBlueS; }
        public bool GetIsRedComputer() { return this.isRedComputer; }
        public bool GetIsBlueComputer() { return this.isBlueComputer; }
        public bool GetIsRedTurn() { return this.isRedTurn; }
        public bool GetIsBlueTurn() { return this.isBlueTurn; }
        public int GetSize() { return this.size; }
        public Grid GetButtonGrid() { return this.buttonGrid; }
    }
}
