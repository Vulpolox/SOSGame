using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myra;
using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection.Metadata.Ecma335;

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

        // flag for if game is over
        protected bool isGameOver = false;

        // recorded size
        protected int recordedSize;

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

        protected Vector2 boardLoc = new Vector2(390, 200);  // the x and y coordinates of the top left corner of the game board


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

            // set the recorded game's board size if applicable
            this.recordedSize = recordedSize;

            // initialize the board and place it in the outerGrid
            int finalSize = this.recordedSize != -1 ? this.recordedSize : this.size;
            this.buttonGrid = InitializeBoard(finalSize);
            Grid.SetRow(buttonGrid, _r);
            Grid.SetColumn(buttonGrid, _c);
            this.outerGrid.Widgets.Add(buttonGrid);

            // if the player who is up first is a computer and it is not a game recording, generate and handle its move
            if (this.isBlueComputer && this.recordedSize == -1)  { StartComputerMove(250); }

        }


        // creates and returns a reference to the grid of buttons used for the board
        public Grid InitializeBoard(int size)
        {
            Grid returnGrid = new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 1,
                Width = 300,
                Height = 300,
                HorizontalAlignment = HorizontalAlignment.Center,
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
            HandleMove(move);

            // get SOS information and handle it
            SOSInfo sosInfo = this.gameLogicHandler.GetSOSInfo(move);
            bool isComputerNext = this.IsComputerTurnNext(sosInfo);
            HandleSOS(sosInfo);

            // update the GUI turn label
            this.GUIRef.UpdateTurnLabel(this.gameLogicHandler.IsRedTurn());

            // if the next player up is a computer, have it move
            if (isComputerNext && !isGameOver) { StartComputerMove(500); }

            //Console.WriteLine(String.Format("Clicked Button At r = {0}, c = {1}", pressedButton.GetRowIndex(), pressedButton.GetColumnIndex()));
            //Console.WriteLine(this.GetCell(pressedButton.GetRowIndex() , pressedButton.GetColumnIndex()));

        }


        // method for creating a delay before invoking
        // a computer move
        public void StartComputerMove(int delay)
        {
            Timer timer = new Timer(delay);

            // disable unpressed buttons while the computer is moving
            this.ToggleUnpressedButtons(false);

            timer.Elapsed += (sender, e) =>
            {
                try
                {
                    // clean up after timer elapsed
                    timer.Stop();
                    timer.Dispose();

                    // invoke the computer move
                    HandleComputerMove();
                }

                catch (Exception ex)  { Console.WriteLine($"Error: {ex.Message}"); }
                
            };

            timer.Start();
        }


        // method for generating/handling computer-made moves
        public void HandleComputerMove()
        {

            // generate and handle move made by computer
            MoveInfo computerMove = this.gameLogicHandler.GenerateComputerMove();
            HandleMove(computerMove);

            // get SOS information and handle it
            SOSInfo sosInfo = this.gameLogicHandler.GetSOSInfo(computerMove);
            bool isComputerNext = this.IsComputerTurnNext(sosInfo);
            HandleSOS(sosInfo);

            // update the GUI turn label
            this.GUIRef.UpdateTurnLabel(this.gameLogicHandler.IsRedTurn());

            // if the next player up is a computer, have it move (this should only happen during fully computer games)
            if (isComputerNext && !isGameOver) { StartComputerMove(500); }

            // otherwise reenable the buttons for the human player
            else { this.ToggleUnpressedButtons(true); }
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

            // display a message of who won
            String winMessage = isRedWon ? "Red Won!" : "Blue Won!";
            GUIRef.DisplayMessage(winMessage);

            // update isGameOver flag
            this.isGameOver = true;
        }


        // method for draw games
        public void Draw()  
        { 
            // display message box
            GUIRef.DisplayMessage("Tie Game!");

            // update isGameOver flag
            this.isGameOver = true;
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
        public bool IsComputerTurnNext(SOSInfo sosInfo)
        {
            // holds the current turn in form "Red" or "Blue"
            String playerTurn = gameLogicHandler.GetPlayerTurnColorName();

            // if the current turn's player did not make an SOS, determine
            // if the other player is a computer as they will be up next
            if (sosInfo.NumSOS == 0)
            {
                if (playerTurn == "Red" && this.isBlueComputer) { return true; }
                else if (playerTurn == "Blue" && this.isRedComputer) { return true; }
                else { return false; }
            }

            // otherwise determine if the player who made the move is a
            // computer because they will get another turn
            else
            {
                if (playerTurn == "Red" && this.isRedComputer) { return true; }
                else if (playerTurn == "Blue" && this.isBlueComputer) { return true; }
                else { return false; }
            }
            
        }


        // method for finding the endpoints from which each line will be drawn
        // to mark newly formed SOSs
        public List<List<Vector2>> GetLineEndpoints(SOSInfo sosInfo)
        {
            List<List<Vector2>> endpointCoordinates = new List<List<Vector2>>();

            foreach (var coordinateSet in sosInfo.StartEndCoords)
            {
                // indices of the buttons between which the line will be drawn
                Vector2 startIndex = coordinateSet[0];
                Vector2 endIndex = coordinateSet[1];

                // find the positions of the centers of the two buttons that
                // define the bounds of the SOS so that a line can be drawn between them
                Vector2 startCoords = GetCenter(GetButtonPosition(startIndex));
                Vector2 endCoords = GetCenter(GetButtonPosition(endIndex));

                // add endpoints to temporary List
                List<Vector2> endpointsToAdd = new List<Vector2>
                {
                   startCoords, endCoords
                };

                // add temporary list to the one that will be returned
                endpointCoordinates.Add(endpointsToAdd);
            }

            return endpointCoordinates;
        }


        // method for finding the absolute position (top left corner) of a button
        // given its row and column indices
        public Vector2 GetButtonPosition(Vector2 buttonArrayIndices)
        {
            // calculate the side length of button by dividing board dimensions (300x300 px)
            // by number of buttons per side
            float buttonSideLength = 300 / this.size;

            // unpack row and column indices of the button into variables
            // for readability
            int rowIndex = (int)buttonArrayIndices.X;
            int columnIndex = (int)buttonArrayIndices.Y;

            // calculate and return the top left position of the button
            return new Vector2(this.boardLoc.X + (columnIndex * buttonSideLength),
                               this.boardLoc.Y + (rowIndex * buttonSideLength));
        }


        // method that takes the absolute position (top left) of a button
        // and returns the center position of said button
        public Vector2 GetCenter(Vector2 buttonPos)
        {
            // calculate the button side length
            float buttonSideLength = 300 / this.size;

            // return the position of the center of the button
            return new Vector2(buttonPos.X + (0.5f * buttonSideLength),
                               buttonPos.Y + (0.5f * buttonSideLength));
        }


        // method for creating lines from a set of endpoint pairs
        public List<Line> CreateLines(List<List<Vector2>> lineEndpoints)
        {
            List<Line> lines = new List<Line>();

            // calculate the color the lines should be based on the
            // current player's turn
            Color lineColor = gameLogicHandler.GetPlayerTurnColorName() == "Blue" ? Color.Blue : Color.Red;

            // create a line for each enpointPair
            foreach (var endpointPair in lineEndpoints)
            {
                Line lineToAdd = new Line(endpointPair[0], endpointPair[1], lineColor);
                lines.Add(lineToAdd);
            }

            return lines;
        }


        // sends line information to the Monogame Game
        // instance for drawing given an SOSInfo instance
        public void HandleLines(SOSInfo sosInfo)
        {
            List<Line> lines = CreateLines(GetLineEndpoints(sosInfo));

            foreach (var line in lines)
            {
                // call the AddLine function of the Game if MyraEnvironment.Game is set to it
                (MyraEnvironment.Game as Game1)?.AddLine(line);
            }
        }


        // method for disable/enabling all unpressed buttons
        // for use when computer is moving
        public void ToggleUnpressedButtons(bool enableDisable)
        {
            foreach (var buttonRow in this.buttonArray)
            {
                foreach (GridButton button in buttonRow)
                {
                    if (button.GetText() == " ")
                    {
                        button.Enabled = enableDisable;
                    }
                }
            }
        }
    }
}
