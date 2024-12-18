﻿using Castle.Components.DictionaryAdapter;
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
        public List<List<Vector2>> StartEndCoords { get; set; }      // starting + ending indices of
                                                                     // cells through which to draw line
        public int NumSOS { get; set; }                              // number of SOSs

        public bool IsRedTurn { get; }


        // constructor
        public SOSInfo(bool isRedTurn) 
        {
            this.NumSOS = 0;
            this.StartEndCoords = new List<List<Vector2>>();

            this.IsRedTurn = isRedTurn;
        }


        // methods

        // method for adding an SOS
        public void AddSOS(Vector2 startingPos, Vector2 endingPos)
        {

            // increment the number of SOSs
            this.NumSOS++;

            // define and add the endpoint pair to draw the line
            List<Vector2> coordDirectionPair = new List<Vector2>();
            coordDirectionPair.Add(startingPos);
            coordDirectionPair.Add(endingPos);

            this.StartEndCoords.Add(coordDirectionPair);


        }

    }
}
