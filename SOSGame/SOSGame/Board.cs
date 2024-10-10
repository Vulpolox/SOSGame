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
    public class Board
    {
        private int size;             // dimensions of the board
        private bool isRedComputer;   // flag for whether red player is computer
        private bool isBlueComputer;  // flag for whether blue player is computer
        private bool isSimpleGame;    // flag for whether game mode is simple or general

        private Grid buttonGrid;                     // Grid for displaying all of the cells of the board
        private List<List<GridButton>> buttonArray;  // Array for holding references to the cells ("GridButtons") on the board

        Grid outerGrid;               // the outer grid on which the board will be displayed
        private const int _r = 1;     // the row index in which the board will be displayed on the outer grid
        private const int _c = 1;     // the column index in which the board will be displayed on the outer grid


        public Board(GUIHandler GUIRef)
        {
            this.size = GUIRef.GetBoardSize();
            this.isRedComputer = GUIRef.IsRedComputer();
            this.isBlueComputer = GUIRef.IsBlueComputer();
            this.isSimpleGame = GUIRef.IsSimpleGame();
            this.outerGrid = GUIRef.GetOuterGrid();
            this.buttonArray = new List<List<GridButton>>();

            this.buttonGrid = InitializeBoard();
            Grid.SetRow(buttonGrid, _r);
            Grid.SetColumn(buttonGrid, _c);
            this.outerGrid.Widgets.Add(buttonGrid);
        }

        public Board()
        {

        }


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
                buttonArray.Add(rowToAdd);
                rowToAdd.Clear();
            }

            return returnGrid;
        }

        public void ClearBoard()
        {
            buttonGrid.Widgets.Clear();
        }

        public void OnGridButtonClick(object sender, EventArgs e)
        {
            Console.WriteLine(String.Format("Clicked Button At"));
        }
    }
}
