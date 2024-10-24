using Castle.Components.DictionaryAdapter;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    // class that holds information about SOSs
    public class SOSInfo
    {
        // instance variables
        public List<List<Vector2>> CoordsAndDirections { get; set; } // directions and starting positions of
                                                                     // all SOSs for drawing lines
        public int NumSOS { get; set; }                              // number of SOSs


        // constructor
        public SOSInfo() 
        {
            this.NumSOS = 0;
            this.CoordsAndDirections = new List<List<Vector2>>();
        }


        // methods

        // method for adding an SOS
        public void AddSOS(Vector2 startingPos, Vector2 direction)
        {

            // increment the number of SOSs
            this.NumSOS++;

            // define and return the startingPos-direction pair to draw the line
            List<Vector2> coordDirectionPair = new List<Vector2>();
            coordDirectionPair.Add(startingPos);
            coordDirectionPair.Add(direction);

            this.CoordsAndDirections.Add(coordDirectionPair);


        }

    }
}
