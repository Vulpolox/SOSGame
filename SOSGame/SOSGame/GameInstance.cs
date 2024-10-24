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
    public abstract class GameInstance
    {
        // reference of GUI
        protected GUIHandler GUIRef;

        // reference to Game
        protected Game GameRef;

        // information from the GUI
        protected int size;             // dimensions of the board
        protected bool isRedS;          // flag for whether the red player has selected an S
        protected bool isBlueS;         // flag for whether the blue player has selected an S
        protected bool isSimpleGame;    // flag for whether game mode is set to simple
        protected bool isRedComputer;   // flag for whether player is computer
        protected bool isBlueComputer;  // flag for whether player is computer

        // player information
        protected bool isRedTurn = false;     // flag for red player's turn
        protected bool isBlueTurn = true;     // flag for blue player's turn

        protected GameLogicHandler gameLogicHandler;

        protected Grid buttonGrid;                     // The board
        protected List<List<GridButton>> buttonArray;  // Array for holding references to the cells ("GridButtons") on the board

        // board location information
        Grid outerGrid;                 // the outer grid on which the board will be displayed
        protected const int _r = 1;     // the row index at which the board will be displayed on the outer grid
        protected const int _c = 1;     // the column index at which the board will be displayed on the outer grid


        // constructor
        public GameInstance(GUIHandler GUIRef, GameLogicHandler gameLogicHandler, int recordedSize = -1)
        {
            // create reference to the running Game
            this.GameRef = MyraEnvironment.Game;

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
            int finalSize = recordedSize != -1 ? recordedSize : this.size;
            this.buttonGrid = InitializeBoard(finalSize);
            Grid.SetRow(buttonGrid, _r);
            Grid.SetColumn(buttonGrid, _c);
            this.outerGrid.Widgets.Add(buttonGrid);

            // if the player who is up first is a computer and it is not a game recording, generate and handle its move
            if (this.isBlueComputer && recordedSize != -1)  { HandleComputerMove(); }
        }


        // creates and returns a reference to the grid of buttons used for the board
        public Grid InitializeBoard(int size)
        {
            Grid returnGrid = new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 1,
                Width = 300,
                Height = 300
            };

            this.buttonArray.Clear();

            List<GridButton> rowToAdd = new List<GridButton>();

            // add columns and rows to the return grid
            for (int i = 0; i < this.size; i++)
            {
                returnGrid.RowsProportions.Add(new Proportion(ProportionType.Part, 1));
                returnGrid.ColumnsProportions.Add(new Proportion(ProportionType.Part, 1));
            }

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {

                    // initialize the GridButton that will be added to the board and subscribe it to action listener
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

        public void ClearBoard()  { buttonGrid.Widgets.Clear(); }

        public void ChangeTurns()  { this.gameLogicHandler.ChangeTurns(); }


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

            // create and process a move
            MoveInfo move = new MoveInfo(rowIndex, columnIndex, playerSOChoice);
            move.TurnBit = this.gameLogicHandler.IsBlueTurn() ? 0 : 1;
            HandleMove(move);

            // get SOS information and handle it
            SOSInfo sosInfo = this.gameLogicHandler.GetSOSInfo(move);
            HandleSOS(sosInfo);

            // if the board is full, handle whether to call a draw or to tally points
            if (gameLogicHandler.IsBoardFull()) { HandleFullBoard(); }

            // update the GUI turn label
            this.GUIRef.UpdateTurnLabel(this.gameLogicHandler.IsRedTurn());

            // if the next player up is a computer, have it move
            if (IsComputerTurnNext()) { HandleComputerMove(); }

            Console.WriteLine(String.Format("Clicked Button At r = {0}, c = {1}", pressedButton.GetRowIndex(), pressedButton.GetColumnIndex()));
            Console.WriteLine(this.GetCell(pressedButton.GetRowIndex() , pressedButton.GetColumnIndex()));

        }


        // method for generating/handling computer-made moves
        public void HandleComputerMove()
        {
            // TODO: add a slight delay using coroutines so computer moves don't happen instantly

            // generate and handle move made by computer
            MoveInfo computerMove = this.gameLogicHandler.GenerateComputerMove();
            HandleMove(computerMove);

            // get SOS information and handle it
            SOSInfo sosInfo = this.gameLogicHandler.GetSOSInfo(computerMove);
            HandleSOS(sosInfo);

            // if the board is full, handle whether to call a draw or to tally points
            if (gameLogicHandler.IsBoardFull()) { HandleFullBoard(); }

            // update the GUI turn label
            this.GUIRef.UpdateTurnLabel(this.gameLogicHandler.IsRedTurn());

            // if the next player up is a computer, have it move (this should only happen during fully computer games)
            if (IsComputerTurnNext()) { HandleComputerMove(); }
        }


        // abstract methods
        public abstract void HandleSOS(SOSInfo sosInfo);
        public abstract void HandleFullBoard();


        // method for winning game
        public void Win(bool isRedWon)
        {
            // disable the remaining buttons
            foreach (var buttonRow in this.buttonArray)
            {
                foreach (var gridButton in buttonRow)
                {
                    gridButton.Enabled = false;
                }
            }
        }


        // method for handling moves made by computers, humans, or recordings thereof
        public void HandleMove(MoveInfo moveInfo)
        {
            // unpack variables
            int columnIndex = moveInfo.ColumnIndex;
            int rowIndex = moveInfo.RowIndex;
            String moveLetter = moveInfo.MoveLetter;

            // get reference to button on which the move was performed
            GridButton pressedButton = this.buttonArray[rowIndex][columnIndex];

            // update the button: set the text to moveLetter and disable it
            pressedButton.SetText(moveLetter);
            pressedButton.Enabled = false;

            // update the internal board state
            this.gameLogicHandler.UpdateInternalBoardState(rowIndex, columnIndex, moveLetter);
        }


        // method for determining if the next turn is to be made by a computer
        public bool IsComputerTurnNext()
        {
            String playerTurn = gameLogicHandler.GetPlayerTurnColorName();

            if (playerTurn == "Red" && this.isBlueComputer) { return true; }
            else if (playerTurn == "Blue" && this.isRedComputer) { return true; }
            else { return false; }
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
