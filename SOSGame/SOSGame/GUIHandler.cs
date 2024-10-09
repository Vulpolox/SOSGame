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


        // Record Game Pane
        private CheckButton recordGameButton;  
        private Grid recordGamePane;


        // Label for Current Turn
        private Label currentTurnLabel;


        // Board Size Pane
        private SpinButton boardSizeButton;
        private Grid boardSizePane;


        // Operation Button Pane
        private Button replayButton;
        private Button newGameButton;
        private Grid operationPane;


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
                ColumnSpacing = 150,
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
                TextColor = Color.Purple
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
                Content = new Label
                {
                    Text = "Simple Game",
                    TextColor = Color.Black
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
                Minimum = 0,
                Maximum = 10
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
                RowSpacing = 20,
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


            // initialize _desktop

            _desktop = new Desktop
            {
                Root = outerGrid
            };

        }


        // method for drawing GUI to screen
        public void Draw()
        {
            _desktop.Render();
        }
    }
}
