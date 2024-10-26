using Castle.Components.DictionaryAdapter;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class Line
    {
        // instance variables
        public Vector2 StartCoords { get; set; }
        public Vector2 EndCoords { get; set; }
        public Color LineColor { get; set; }


        // constructor
        public Line(Vector2 startCoords, Vector2 endCoords, Color lineColor)
        {
            this.StartCoords = startCoords;
            this.EndCoords = endCoords;
            this.LineColor = lineColor;
        }


        // ToString override for testing
        public override string ToString()
        {
            return $"Start Point: {this.StartCoords}\nEnd Point: {this.EndCoords}";
        }

    }
}
