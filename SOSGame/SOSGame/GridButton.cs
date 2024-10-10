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
    public class GridButton : Button
    {
        private int rowIndex;
        private int columnIndex;
        private Label textLabel;

        public GridButton(int r, int c) : base()
        {
            this.rowIndex = r;
            this.columnIndex = c;
            this.textLabel = new Label
            {
                Text = "S",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            this.Content = this.textLabel;
            this.Height = 100;
            this.Width = 100;
        }

        // accessor and mutator for the text on the GridButton
        public void SetText(String text)  { this.textLabel.Text = text; }
        public String GetText()  { return this.textLabel.Text; }

        // accessors for the row and column indices
        public int GetRowIndex() { return this.rowIndex; }
        public int GetColumnIndex() { return this.columnIndex; }

        // comparison method for the text of the button
        public bool CompareText(String comparison)  { return comparison == this.GetText(); }
    }
}
