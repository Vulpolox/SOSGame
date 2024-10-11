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
    public class GUIHandler
    {
        private Desktop _desktop;   // Myra Desktop object for drawing UI elements to the screen
        private Grid outerGrid;     // Outer grid for holding the main UI elements of the SOS game


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

        // game manager
        private GameInstance gameInstance;


        // Constructor
        public GUIHandler(Game game)
        {
            MyraEnvironment.Game = game;
        }


        public void InitializeGUIElements()
        {

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

            Label sosLabel = new Label
            {
                Text = "SOS",
                TextColor = Color.Purple,
            };

            // set the label's location and add it to the outerGrid
            Grid.SetColumn(sosLabel, 0);
            Grid.SetRow(sosLabel, 0);
            outerGrid.Widgets.Add(sosLabel);

        // Initialize gameTypeButtonPane and add it to the outerGrid at (0, 1)

            gameTypeButtonPane = new Grid
            {
                RowSpacing = 8,
                ColumnSpacing = 15
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
                    TextColor = Color.Black,
                }
            };

            generalGameButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "General Game",
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
                TextColor = Color.Black
            };

            // initialize the board size selector spin button
            boardSizeButton = new SpinButton
            {
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
                ColumnSpacing = 20
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
                    TextColor = Color.Blue
                }
            };

            blueOButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "O",
                    TextColor = Color.Blue
                }
            };

            blueIsHumanButton = new RadioButton

            {
                IsPressed = true,
                Content = new Label
                {
                    Text = "Human",
                    TextColor = Color.Blue
                }
            };

            blueIsComputerButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "Computer",
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
                ColumnSpacing = 20
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
                    TextColor = Color.Red
                }
            };

            redOButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "O",
                    TextColor = Color.Red
                }
            };

            redIsHumanButton = new RadioButton

            {
                IsPressed = true,
                Content = new Label
                {
                    Text = "Human",
                    TextColor = Color.Red
                }
            };

            redIsComputerButton = new RadioButton
            {
                Content = new Label
                {
                    Text = "Computer",
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
                ColumnSpacing = 0
            };

            // add two rows to the pane
            for (int i = 0; i < 2; i++)
            {
                operationsPane.RowsProportions.Add(new Proportion(ProportionType.Auto));
            }

            // initialize the operation buttons
            newGameButton = new Button
            {
                Content = new Label
                {
                    Text = "New Game",
                    TextColor = Color.LightBlue
                }
            };

            replayButton = new Button
            {
                Content = new Label
                {
                    Text = "Replay Recording",
                    TextColor = Color.LightBlue
                }
            };

            // subscribe buttons to action listeners
            newGameButton.Click += OnNewGameClick;
            replayButton.Click += OnReplayClick;

            // set the positions of the buttons
            Grid.SetRow(newGameButton, 0);
            Grid.SetColumn(newGameButton, 0);
            Grid.SetRow(replayButton, 1);
            Grid.SetColumn(replayButton, 0);

            // set the position of the pane
            Grid.SetRow(operationsPane, 2);
            Grid.SetColumn(operationsPane, 2);

            // add the buttons to the pane
            operationsPane.Widgets.Add(newGameButton);
            operationsPane.Widgets.Add(replayButton);

            // add the operations pane to the outerGrid
            outerGrid.Widgets.Add(operationsPane);

            // initialize the game
            this.gameInstance = new GameInstance(this);

        // initialize _desktop

            _desktop = new Desktop
            {
                Root = outerGrid
            };

            Console.WriteLine("Finished Initializing GUI"); // to disable console go to Project -> SOSGame Properties -> Change Output type

        }

        // action listeners for buttons
        public void OnNewGameClick(object sender, EventArgs e)
        {
            if (this.gameInstance != null)
            {
                gameInstance.ClearBoard();
            }

            this.gameInstance = new GameInstance(this);
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
        public int GetBoardSize() { return (int)boardSizeButton.Value; }
        public Grid GetOuterGrid()  { return this.outerGrid; }


        // method for updating the text and color of the player turn label
        public void UpdateTurnLabel(bool isRedTurn)
        {
            String playerColorName = isRedTurn ? "Red" : "Blue";

            this.currentTurnLabel.Text = String.Format("Current Turn: {0}", playerColorName);
            this.currentTurnLabel.TextColor = isRedTurn ? Color.Red : Color.Blue;
        }


        // method for drawing GUI to screen
        public void Draw()
        {
            _desktop.Render();
        }
    }
}
