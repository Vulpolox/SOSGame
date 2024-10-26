﻿using Castle.Components.DictionaryAdapter;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOSGame
{
    public class GameLogicHandler
    {
        // flags
        private bool isSimpleGame;
        private bool isRedComputer;
        private bool isBlueComputer;
        private bool isRedTurn = false;
        private bool isBlueTurn = true;

        // board variables
        private int boardSize;
        private List<List<String>> internalBoardState;


        // constructor using information from the GUI
        public GameLogicHandler(GUIHandler GUIRef)
        {
            this.isSimpleGame = GUIRef.IsSimpleGame();
            this.isRedComputer = GUIRef.IsRedComputer();
            this.isBlueComputer = GUIRef.IsBlueComputer();
            this.boardSize = GUIRef.GetBoardSize();
            this.internalBoardState = InitializeBoardState();
        }


        // overloaded constructor for creating testable instances of the class that don't rely on GUI
        public GameLogicHandler (
            bool isSimpleGame = false,
            bool isRedComputer = false,
            bool isBlueComputer = false,
            bool isRedTurn = false,
            bool isBlueTurn = true,
            int boardSize = 3)
        {
            this.isSimpleGame = isSimpleGame;
            this.isRedComputer = isRedComputer;
            this.isBlueComputer = isBlueComputer;
            this.isRedTurn = isRedTurn;
            this.isBlueTurn = isBlueTurn;
            this.boardSize = boardSize;

            this.internalBoardState = InitializeBoardState();
        }


        // accessors and mutators
        public bool IsSimpleGame() { return isSimpleGame; }
        public bool IsRedComputer() { return isRedComputer; }
        public bool IsBlueComputer() { return isBlueComputer; }
        public bool IsRedTurn() { return isRedTurn; }
        public bool IsBlueTurn() { return isBlueTurn; }  
        public int GetBoardSize() { return boardSize; }
        public List<List<String>> GetInternalBoardState() { return internalBoardState; }


        // methods
        public String GetPlayerTurnColorName()  { return this.isRedTurn ? "Red" : "Blue"; }

        public void ChangeTurns()
        {
            this.isRedTurn = !this.isRedTurn;
            this.isBlueTurn = !this.isBlueTurn;
        }


        // method for initilizing the internalBoardState; all elements start as "EMPTY"
        public List<List<String>> InitializeBoardState()
        {
            List<List<String>> returnList = new List<List<String>>();
            List<String> rowToAdd;

            for (int r = 0; r < this.boardSize; r++)
            {
                rowToAdd = new List<String>();

                for (int c = 0; c < this.boardSize; c++)
                {
                    rowToAdd.Add("EMPTY");
                }
                returnList.Add(rowToAdd);
            }

            return returnList;
        }

        
        // method for testing if all cells in board are filled; short circuits on the first found "EMPTY"
        public bool IsBoardFull()
        {
            foreach (var internalRow in this.internalBoardState)
            {
                foreach (var cell in internalRow)
                {
                    if (cell == "EMPTY") { return false; }
                }
            }

            return true;
        }



        // method for updating the internal board state at index [r][c]
        public void UpdateInternalBoardState(int r, int c, String playerSOChoice)
        {
            if (_IsOutOfBounds(r, c))  { Console.WriteLine("Tried to access internal state at invalid index"); }
            else                       { this.internalBoardState[r][c] = playerSOChoice; }
        }


        // method for checking whether internalBoardState[r][c] is OOB
        private bool _IsOutOfBounds(int r, int c)  { return (Math.Max(r, c) >= this.boardSize) || (Math.Min(r, c) < 0); }


        // method for returning the contents of internalBoardState[r][c] ; also returns "OOB" if passed indices correspond to OOB index
        public String internalBoardStateRef(int r, int c)
        {
            if (_IsOutOfBounds(r, c)) { return "OOB"; }
            else { return this.internalBoardState[r][c]; }
        }


        // overloaded internalBoardStateRef method for compatibility with Vector2s
        public String internalBoardStateRef(Vector2 coordinates)  { return internalBoardStateRef((int)coordinates.X, (int)coordinates.Y); }



        // ************************************************************************************************* //



        // algorithms

        // sos-finding algorithm using linear algebra
        public SOSInfo GetSOSInfo(MoveInfo move)
        {
            // initialize an SOS counter to 0 and create a Vector2 with move indices
            SOSInfo sosInfo = new SOSInfo(this.isRedTurn);
            Vector2 moveCoords = new Vector2(move.RowIndex, move.ColumnIndex);

            // if move letter is an 'S'
            if (move.MoveLetter == "S")
            {
                // check 2 cells in each of the 8 directions for an 'O' and an 'S' in that order
                foreach (Vector2 direction in DirectionVectors.GetAllDirections())
                {
                    if (internalBoardStateRef(moveCoords + direction) == "O" &&
                        internalBoardStateRef(moveCoords + (2 * direction)) == "S")
                    {
                        // each time these conditions are met, add an SOS to the SOSInfo object
                        sosInfo.AddSOS(startingPos: moveCoords, endingPos: moveCoords + (2 * direction));
                    }
                }
            }
            
            // if the move letter is an 'O'
            else if (move.MoveLetter == "O")
            {
               // check the 4 pairs of opposite directions for two 'S's
               
                // UP and DOWN
                if (internalBoardStateRef(moveCoords + DirectionVectors.Up) == "S" &&
                   internalBoardStateRef(moveCoords + DirectionVectors.Down) == "S")
                {
                    sosInfo.AddSOS(startingPos: (moveCoords + DirectionVectors.Down),
                                   endingPos: (moveCoords + DirectionVectors.Up));
                }

                // LEFT and RIGHT
                if (internalBoardStateRef(moveCoords + DirectionVectors.Left) == "S" &&
                   internalBoardStateRef(moveCoords + DirectionVectors.Right) == "S")
                {
                    sosInfo.AddSOS(startingPos: (moveCoords + DirectionVectors.Left),
                                   endingPos: (moveCoords + DirectionVectors.Right));
                }

                // UP LEFT and DOWN RIGHT
                if (internalBoardStateRef(moveCoords + DirectionVectors.UpLeft) == "S" &&
                   internalBoardStateRef(moveCoords + DirectionVectors.DownRight) == "S")
                {
                    sosInfo.AddSOS(startingPos: (moveCoords + DirectionVectors.DownRight),
                                   endingPos: (moveCoords + DirectionVectors.UpLeft));
                }

                // UP RIGHT and DOWN LEFT
                if (internalBoardStateRef(moveCoords + DirectionVectors.UpRight) == "S" &&
                   internalBoardStateRef(moveCoords + DirectionVectors.DownLeft) == "S")
                {
                    sosInfo.AddSOS(startingPos: (moveCoords + DirectionVectors.DownLeft),
                                   endingPos: (moveCoords + DirectionVectors.UpRight));
                }


            }

            // otherwise print error message
            else
            {
                Console.WriteLine($"Invalid Move Letter {move.MoveLetter}");
            }

            Console.WriteLine($"Found {sosInfo.NumSOS} SOSs");

            return sosInfo;
        }


        public MoveInfo GenerateComputerMove()
        {

            // initialize variables
            List<MoveInfo> possibleMoves = new List<MoveInfo>();
            List<MoveInfo> sosMoves = new List<MoveInfo>();
            List<MoveInfo> smartMoves = new List<MoveInfo>();
            Random rand = new Random();

            // DEFINE LISTS

            // create a list of all possible moves that result in SOS creation

            for (int r = 0; r < this.boardSize; r++)
            {
                for (int c = 0; c < this.boardSize; c++)
                {
                    if (this.internalBoardState[r][c] == "EMPTY")
                    {
                        MoveInfo sMove = new MoveInfo(r, c, "S");
                        MoveInfo oMove = new MoveInfo(r, c, "O");
                        possibleMoves.Add(sMove);
                        possibleMoves.Add(oMove);
                    }
                }
            }


            // create a list of all possible moves that result in SOS creation

            sosMoves = possibleMoves.Where(move => SOSMovePredicate(move)).ToList();

            // create a list of all possible moves which will not lead
            // to an SOS being possible for the next player

            smartMoves = possibleMoves.Where(move => SmartMovePredicate(move)).ToList();


            // FOR LIST OF POSSIBLE SOS MOVES

            // generate a random number 1-10; foreach element in
            // possible SOS moves, if number is > 5, return that move;
            // if it is not, increment the generated number by 1
            // (more possible SOSs = higher likelihood CPU takes one)

            int odds = rand.Next(1, 11);

            foreach (var sosMove in sosMoves)
            {
                if (odds > 5) { return sosMove; }
                else { odds++; }
            }


            // FOR LIST OF MOVES THAT WILL NOT ALLOW NEXT PLAYER TO
            // GET SOS

            // if CPU player doesn't take any moves that could make an SOS,
            // select a random move from this list and return it

            if (smartMoves.Count > 0) { return smartMoves[rand.Next(0, possibleMoves.Count)]; }


            // FOR LIST OF ALL POSSIBLE MOVES

            // if the previous list is empty, take a random move from
            // all possible moves list

            else { return possibleMoves[rand.Next(0, possibleMoves.Count)]; }


            // PREDICATE HELPER FUNCTIONS

            bool SOSMovePredicate(MoveInfo move)
            {
                return this.GetSOSInfo(move).NumSOS > 0;
            }


            bool SmartMovePredicate(MoveInfo move)
            {
                Vector2 moveCoords = new Vector2(move.RowIndex, move.ColumnIndex);

                // if move letter is an 'S'
                if (move.MoveLetter == "S")
                {
                    // check 2 cells in each of the 8 directions for an 'O' and an 'EMPTY' in that order
                    // or an 'EMPTY' and an 'S' in that order
                    foreach (Vector2 direction in DirectionVectors.GetAllDirections())
                    {
                        if (internalBoardStateRef(moveCoords + direction) == "O" &&
                            internalBoardStateRef(moveCoords + (2 * direction)) == "EMPTY"
                            ||
                            internalBoardStateRef(moveCoords + direction) == "EMPTY" &&
                            internalBoardStateRef(moveCoords + (2 * direction)) == "S")
                        {
                            // move is not smart, return false
                            return false;
                        }
                    }
                }

                // if the move letter is an 'O'
                else if (move.MoveLetter == "O")
                {
                    // check the 4 pairs of opposite directions for one 'S' and one 'EMPTY'

                    if (internalBoardStateRef(moveCoords + DirectionVectors.Up) == "S" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.Down) == "EMPTY"
                       ||
                       internalBoardStateRef(moveCoords + DirectionVectors.Up) == "EMPTY" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.Down) == "S"
                       ||
                       internalBoardStateRef(moveCoords + DirectionVectors.Left) == "S" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.Right) == "EMPTY"
                       ||
                       internalBoardStateRef(moveCoords + DirectionVectors.Left) == "EMPTY" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.Right) == "S"
                       ||
                       internalBoardStateRef(moveCoords + DirectionVectors.UpLeft) == "S" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.DownRight) == "EMPTY"
                       ||
                       internalBoardStateRef(moveCoords + DirectionVectors.UpLeft) == "EMPTY" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.DownRight) == "S"
                       ||
                       internalBoardStateRef(moveCoords + DirectionVectors.UpRight) == "S" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.DownLeft) == "EMPTY"
                       ||
                       internalBoardStateRef(moveCoords + DirectionVectors.UpRight) == "EMPTY" &&
                       internalBoardStateRef(moveCoords + DirectionVectors.DownLeft) == "S")
                    {
                        // move is not smart, return false
                        return false;
                    }
                }

                return true;
            }

        }
    }
}