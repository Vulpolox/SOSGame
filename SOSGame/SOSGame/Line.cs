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
        public Vector2 StartPoint { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Length { get; set; }
        public Color LineColor { get; set; }


        // constructor
        public Line(Vector2 startPoint, Vector2 direction, Vector2 length, Color lineColor)
        {
            this.StartPoint = startPoint;
            this.Direction = direction;
            this.Length = length;
            this.LineColor = lineColor;
        }

    }
}
