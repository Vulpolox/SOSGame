using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Myra;
using Myra.Graphics2D.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;
using System.IO;

namespace SOSGame
{
    public class GUIHandler
    {
        private Desktop _desktop;           // Myra Desktop object for drawing UI elements to the screen
        private Desktop _messageboxDesktop; // Myra Desktop for drawing message boxes on the screen
        private Grid outerGrid;             // Outer grid for holding the main UI elements of the SOS game


        // FontStashSharp Font System
        private FontSystem fontSystem;


        // Human or Computer Radio Button Panes
        private Grid redPlayerTypePane;        
        private RadioButton redIsHumanButton;
        private RadioButton redIsComputerButton;

        private Grid bluePlayerTypePane;       
        private RadioButton blueIsHumanButton;
        private RadioButton blueIsComputerButton;


        // S & O Radio Button Panes
        private Grid redPlayerButtonPane;      
        private RadioButton redSButton;
        private RadioButton redOButton;

        private Grid bluePlayerButtonPane;     
        private RadioButton blueSButton;
        private RadioButton blueOButton;


        // Game Type Radio Button Pane
        private Grid gameTypeButtonPane;
        private RadioButton simpleGameButton;
        private RadioButton generalGameButton;


        // Label for keeping track of score in General Game
        public Label ScoreLabel { get; set; }


        // Record Game Checkbox
        private CheckButton recordGameButton;


        // Label for Current Turn
        private Label currentTurnLabel;


        // Board Size Pane
        private SpinButton boardSizeButton;
        private Grid boardSizePane;


        // Operation Button Pane
        private Button replayButton;
        private Button newGameButton;
        private Grid operationsPane;

        // game instance
        private GameInstance gameInstance;
        private GameLogicHandler gameLogicHandler;


        // Constructor
        public GUIHandler(Game game)
        {
            MyraEnvironment.Game = game;
        }


        public void InitializeGUIElements()
        {

        // Initialize font system

            //Console.WriteLine(Directory.GetCurrentDirectory());

            // read in the font
            byte[] ttfData;
            try
            {
                ttfData = File.ReadAllBytes(@"..\..\..\Content\Commodore Angled v1.2.ttf"); // figuring out the relative path was a pain in the ass
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Font file not found. Make sure the path is correct.");
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the font file: {ex.Message}");
                return;
            }

            // initialize the font system and add the loaded font
            this.fontSystem = new FontSystem();
            fontSystem.AddFont(ttfData);

            // Initialize outerGrid

            outerGrid = new Grid
            {
                RowSpacing = 150,
                ColumnSpacing = 100,
                HorizontalAlignment = HorizontalAlignment.Center,
            };

            // Add 3 columns and 3 rows to outerGrid
            for (int i = 0; i < 3; i++)
            {
                outerGrid.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
                outerGrid.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // Create 'SOS' Label and add it to outerGrid at (0, 0)

            this.ScoreLabel = new Label
            {
                Text = "",
                Font = this.fontSystem.GetFont(14),
                TextColor = Color.Purple,
            };

            // set the label's location and add it to the outerGrid
            Grid.SetColumn(this.ScoreLabel, 0);
            Grid.SetRow(this.ScoreLabel, 0);
            outerGrid.Widgets.Add(this.ScoreLabel);

        // Initialize gameTypeButtonPane and add it to the outerGrid at (0, 1)

            gameTypeButtonPane = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 15,
                Height = 50
            };

            // Add two columns to the pane for the radio buttons
            for (int i = 0; i < 2; i++)
            {
                gameTypeButtonPane.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // initialize the radio buttons
            simpleGameButton = new RadioButton
            {
                IsPressed = true,
                Content = new Label
                {
                    Text = "Simple Game",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Black,
                }
            };

            generalGameButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "General Game",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Black
                }
            };

            // set radio button locations in the panel and add them to it
            Grid.SetColumn(simpleGameButton, 0);
            Grid.SetRow(simpleGameButton, 0);
            Grid.SetColumn(generalGameButton, 1);
            Grid.SetRow(generalGameButton, 0);
            gameTypeButtonPane.Widgets.Add(simpleGameButton);
            gameTypeButtonPane.Widgets.Add(generalGameButton);

            // set the panel's location in the outerGrid and add it
            Grid.SetRow(gameTypeButtonPane, 0);
            Grid.SetColumn(gameTypeButtonPane, 1);
            outerGrid.Widgets.Add(gameTypeButtonPane);

        // Initialize boardSizePane and add it to outerGrid at (0, 2)

            boardSizePane = new Grid
            {
                RowSpacing = 4,
                ColumnSpacing = 4
            };

            // Add two columns to the pane
            for (int i = 0; i < 2; i++)
            {
                boardSizePane.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // initialize a label
            Label boardSizeLabel = new Label
            {
                Text = "Board Size: ",
                Font = this.fontSystem.GetFont(14),
                TextColor = Color.Black
            };

            // initialize the board size selector spin button
            boardSizeButton = new SpinButton
            {
                Width = 75,
                Minimum = 3,
                Maximum = 10,
                Value = 3
            };

            // set the label and the spin button's positions and add them to the pane
            Grid.SetRow(boardSizeLabel, 0);
            Grid.SetColumn(boardSizeLabel, 0);
            Grid.SetRow(boardSizeButton, 0);
            Grid.SetColumn(boardSizeButton, 1);
            boardSizePane.Widgets.Add(boardSizeLabel);
            boardSizePane.Widgets.Add(boardSizeButton);

            // set the pane's position and add it to the outerGrid
            Grid.SetRow(boardSizePane, 0);
            Grid.SetColumn(boardSizePane, 2);
            outerGrid.Widgets.Add(boardSizePane);

        // initialize the blue player's human/computer and s/o button panes, arrange them into a 1 x 2 grid and add them to the outerGrid at (1, 0)

            // 1 x 2 outer pane for holding the two button groups
            Grid combinedPane = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 20,
                VerticalAlignment = VerticalAlignment.Center,
            };
            for (int i = 0; i < 2; i++)
            {
                combinedPane.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // 2 x 1 pane for holding radio buttons for determining whether player is human/computer
            bluePlayerTypePane = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 8
            };
            for (int i = 0; i < 2; i++)
            {
                bluePlayerTypePane.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // 2 x 1 pane for holding radio buttons for determining if player is placing s/o
            bluePlayerButtonPane = new Grid
            {
                RowSpacing = 20,
                ColumnSpacing = 8
            };
            for (int i = 0; i < 2; i++)
            {
                bluePlayerButtonPane.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // initialiaze the radio buttons
            blueSButton = new RadioButton
            {
                IsPressed = true,
                Content = new Label
                {
                    Text = "S",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Blue
                }
            };

            blueOButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "O",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Blue
                }
            };

            blueIsHumanButton = new RadioButton

            {
                IsPressed = true,
                Content = new Label
                {
                    Text = "Human",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Blue
                }
            };

            blueIsComputerButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "Computer",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Blue
                }
            };

            // set everthing's positions
            Grid.SetRow(bluePlayerTypePane, 0);
            Grid.SetColumn(bluePlayerTypePane, 0);
            Grid.SetRow(bluePlayerButtonPane, 0);
            Grid.SetColumn(bluePlayerButtonPane, 1);
            Grid.SetRow(blueSButton, 0);
            Grid.SetColumn(blueSButton, 0);
            Grid.SetRow(blueOButton, 1);
            Grid.SetColumn(blueOButton, 0);
            Grid.SetRow(blueIsHumanButton, 0);
            Grid.SetColumn(blueIsHumanButton, 0);
            Grid.SetRow(blueIsComputerButton, 1);
            Grid.SetColumn(blueIsComputerButton, 0);

            // add s/o buttons to button pane
            bluePlayerButtonPane.Widgets.Add(blueSButton);
            bluePlayerButtonPane.Widgets.Add(blueOButton);

            // add human/computer buttons to player type pane
            bluePlayerTypePane.Widgets.Add(blueIsComputerButton);
            bluePlayerTypePane.Widgets.Add(blueIsHumanButton);

            // add inner panes to outer pane
            combinedPane.Widgets.Add(bluePlayerButtonPane);
            combinedPane.Widgets.Add(bluePlayerTypePane);

            // set the position of the outer pane and add it to the outerGrid
            Grid.SetRow(combinedPane, 1);
            Grid.SetColumn(combinedPane, 0);
            outerGrid.Widgets.Add(combinedPane);

        // initialize the red player's human/computer and s/o button panes, arrange them into a 1 x 2 grid and add them to the outerGrid at (1, 2)

            // 1 x 2 outer pane for holding the two button groups
            Grid redCombinedPane = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 20,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center
            };
            for (int i = 0; i < 2; i++)
            {
                redCombinedPane.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // 2 x 1 pane for holding radio buttons for determining whether player is human/computer
            redPlayerTypePane = new Grid
            {
                RowSpacing = 0,
                ColumnSpacing = 8
            };
            for (int i = 0; i < 2; i++)
            {
                redPlayerTypePane.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // 2 x 1 pane for holding radio buttons for determining if player is placing s/o
            redPlayerButtonPane = new Grid
            {
                RowSpacing = 20,
                ColumnSpacing = 8
            };
            for (int i = 0; i < 2; i++)
            {
                redPlayerButtonPane.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // initialiaze the radio buttons
            redSButton = new RadioButton
            {
                IsPressed = true,
                Content = new Label
                {
                    Text = "S",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Red
                }
            };

            redOButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "O",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Red
                }
            };

            redIsHumanButton = new RadioButton

            {
                IsPressed = true,
                Content = new Label
                {
                    Text = "Human",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Red
                }
            };

            redIsComputerButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "Computer",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Red
                }
            };

            // set everthing's positions
            Grid.SetRow(redPlayerTypePane, 0);
            Grid.SetColumn(redPlayerTypePane, 1);
            Grid.SetRow(redPlayerButtonPane, 0);
            Grid.SetColumn(redPlayerButtonPane, 0);
            Grid.SetRow(redSButton, 0);
            Grid.SetColumn(redSButton, 0);
            Grid.SetRow(redOButton, 1);
            Grid.SetColumn(redOButton, 0);
            Grid.SetRow(redIsHumanButton, 0);
            Grid.SetColumn(redIsHumanButton, 0);
            Grid.SetRow(redIsComputerButton, 1);
            Grid.SetColumn(redIsComputerButton, 0);

            // add s/o buttons to button pane
            redPlayerButtonPane.Widgets.Add(redSButton);
            redPlayerButtonPane.Widgets.Add(redOButton);

            // add human/computer buttons to player type pane
            redPlayerTypePane.Widgets.Add(redIsComputerButton);
            redPlayerTypePane.Widgets.Add(redIsHumanButton);

            // add inner panes to outer pane
            redCombinedPane.Widgets.Add(redPlayerButtonPane);
            redCombinedPane.Widgets.Add(redPlayerTypePane);

            // set the position of the outer pane and add it to the outerGrid
            Grid.SetRow(redCombinedPane, 1);
            Grid.SetColumn(redCombinedPane, 2);
            outerGrid.Widgets.Add(redCombinedPane);

        // initialize the checkbox for recording games and add it to the outerGrid at position (2, 0)

            // initialize checkbox
            recordGameButton = new CheckButton
            {
                Content = new Label
                {
                    Text = "  Record Next Game",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.Green
                }
            };

            // set checkbox's position
            Grid.SetRow(recordGameButton, 2);
            Grid.SetColumn(recordGameButton, 0);

            // add checkbox to the outerGrid
            outerGrid.Widgets.Add(recordGameButton);

        // initialize label for keeping track of current color's turn and add it to outerGrid at position (2, 1)


            // initialize the label
            currentTurnLabel = new Label
            {
                Text = "Current Turn: Blue",
                Font = this.fontSystem.GetFont(14),
                TextColor = Color.Blue,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            // set the position of the label
            Grid.SetRow(currentTurnLabel, 2);
            Grid.SetColumn(currentTurnLabel, 1);

            // add the label to the outerGrid
            outerGrid.Widgets.Add(currentTurnLabel);

        // initialize the operation button pane and add it to the outerGrid at position (2, 2)

            // initialize the pane
            operationsPane = new Grid
            {
                RowSpacing = 10,
                ColumnSpacing = 10
            };

            // add two columns to the pane
            for (int i = 0; i < 2; i++)
            {
                operationsPane.ColumnsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // initialize the operation buttons
            newGameButton = new Button
            {
                Width = 100,
                Height = 50,
                Content = new Label
                {
                    Text = "New\nGame",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.LightBlue,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            replayButton = new Button
            {
                Width = 100,
                Height = 50,
                Content = new Label
                {
                    Text = "Replay\nRecording",
                    Font = this.fontSystem.GetFont(14),
                    TextColor = Color.LightBlue,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            // subscribe buttons to action listeners
            newGameButton.Click += OnNewGameClick;
            replayButton.Click += OnReplayClick;

            // set the positions of the buttons
            Grid.SetRow(newGameButton, 0);
            Grid.SetColumn(newGameButton, 0);
            Grid.SetRow(replayButton, 0);
            Grid.SetColumn(replayButton, 1);

            // set the position of the pane
            Grid.SetRow(operationsPane, 2);
            Grid.SetColumn(operationsPane, 2);

            // add the buttons to the pane
            operationsPane.Widgets.Add(newGameButton);
            operationsPane.Widgets.Add(replayButton);

            // add the operations pane to the outerGrid
            outerGrid.Widgets.Add(operationsPane);

            // initialize the game and the logic handler
            this.gameLogicHandler = new GameLogicHandler(this);
            this.gameInstance = new SimpleGameInstance(this, this.gameLogicHandler);
            

        // initialize _desktop

            _desktop = new Desktop
            {
                Root = this.outerGrid
            };

            _messageboxDesktop = new Desktop
            {
                Root = this.gameTypeButtonPane
            };

            Console.WriteLine("Finished Initializing GUI"); // to disable console go to Project -> SOSGame Properties -> Change Output type

        }

        // action listeners for buttons
        public void OnNewGameClick(object sender, EventArgs e)
        {
            // clean up the previous instance
            if (this.gameInstance != null)
            {
                this.ResetScoreLabel();
                gameInstance.ClearBoard();
                (MyraEnvironment.Game as Game1)?.ClearLines();
            }

            this.gameLogicHandler = new GameLogicHandler(this);                 // create new GameLogicHandler with updated flags from the GUI

            // initialize game based on specified settings
            if (!IsRecordedGame()) { this.gameInstance = IsSimpleGame() ? 
                                                         new SimpleGameInstance(this, this.gameLogicHandler) : 
                                                         new GeneralGameInstance(this, this.gameLogicHandler); }
            else { this.gameInstance = IsSimpleGame() ? 
                                                         new SimpleRecordedGameInstance(this, this.gameLogicHandler) : 
                                                         new GeneralRecordedGameInstance(this, this.gameLogicHandler); }
        }

        public void OnReplayClick(object sender, EventArgs e)
        {
            Console.WriteLine("TODO");
        }

        // GUI state getters
        public bool IsRedComputer()  { return redIsComputerButton.IsPressed; }
        public bool IsBlueComputer()  { return blueIsComputerButton.IsPressed; }
        public bool IsRedS() { return redSButton.IsPressed; }
        public bool IsBlueS() {  return blueSButton.IsPressed; }
        public bool IsSimpleGame()  { return simpleGameButton.IsPressed; }
        public bool IsRecordedGame() {  return recordGameButton.IsPressed; }
        public int GetBoardSize() { return (int)boardSizeButton.Value; }
        public Grid GetOuterGrid()  { return this.outerGrid; }


        // Widget getters (for testing)
        public RadioButton GetRedIsHumanButton() { return this.redIsHumanButton; }
        public RadioButton GetRedIsComputerButton() { return this.redIsComputerButton; }
        public RadioButton GetBlueIsHumanButton() { return this.blueIsHumanButton; }
        public RadioButton GetBlueIsComputerButton() {return this.blueIsComputerButton; }
        public RadioButton GetRedSButton() { return this.redSButton; }
        public RadioButton GetBlueSButton() {return this.blueSButton; }
        public RadioButton GetRedOButton() { return this.redOButton; }
        public RadioButton GetBlueOButton() { return this.blueOButton; }
        public RadioButton GetSimpleGameButton() { return this.simpleGameButton; }
        public RadioButton GetGeneralGameButton() { return this.generalGameButton; }
        public SpinButton GetBoardSizeButton() { return this.boardSizeButton; }
        public Button GetNewGameButton() { return this.newGameButton; }
        public Button GetReplayButton() { return this.replayButton; }
        public CheckButton GetRecordGameButton() { return this.recordGameButton; }
        public GameInstance GetGameInstance() { return this.gameInstance; }




        // method for updating the text and color of the player turn label
        public void UpdateTurnLabel(bool isRedTurn)
        {
            String playerColorName = this.gameLogicHandler.GetPlayerTurnColorName();

            this.currentTurnLabel.Text = $"Current Turn: {playerColorName, 4}";
            this.currentTurnLabel.TextColor = isRedTurn ? Color.Red : Color.Blue;
        }


        // method for drawing GUI to screen
        public void Draw()
        {
            _desktop.Render();
        }

        // method for drawing message boxes to screen
        public void DrawMessageBoxes()
        {
            _messageboxDesktop.Render();
        }


        // method for displaying message boxes
        public void DisplayMessage(String message)
        {
            var messageBox = Dialog.CreateMessageBox("SOS Message", message);
            messageBox.ShowModal(_messageboxDesktop);
        }

        // method for resetting the ScoreLabel
        public void ResetScoreLabel()  { this.ScoreLabel.Text = ""; }

        // method for updating the ScoreLabel
        public void UpdateScoreLabel(int blueScore, int redScore)
        {
            String scoreString = $"Scores\n   Blue: {blueScore, 2}\n    Red: {redScore, 2}";
            this.ScoreLabel.Text = scoreString;
        }
    }
}
