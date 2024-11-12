using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOSGame;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Myra;
using Myra.Graphics2D.UI;

using Moq;

namespace SOSTests
{
    [TestClass]
    public class SOSTests
    {

        // Acceptance Criteria 2.1, 2.2: Simple and General Game Selection
        [TestMethod]
        public void TestGameModeSelection()
        {
            // ARRANGE

            // create simulated game instances
            GameLogicHandler testGameLogicHandlerSimple;
            GameLogicHandler testGameLogicHandlerGeneral;

            // ACT

            // simulate the player starting games with the simple game and general game radio buttons pressed
            testGameLogicHandlerSimple = new GameLogicHandler(isSimpleGame: true);
            testGameLogicHandlerGeneral = new GameLogicHandler(isSimpleGame: false);
            
            bool testValueSimple = testGameLogicHandlerSimple.IsSimpleGame();
            bool expectedValueSimple = true;

            bool testValueGeneral = testGameLogicHandlerGeneral.IsSimpleGame();
            bool expectedValueGeneral = false;

            // ASSERT

            Assert.AreEqual(testValueSimple, expectedValueSimple);
            Assert.AreEqual(expectedValueGeneral, testValueGeneral);
        }


        // Acceptance Criteria 1.1 & 3.1, 3.2: Choosing Board Size, Starting Game and Board Initialization
        // Made By ChatGPT
        [TestClass]
        public class GameLogicHandlerTests
        {
            // 1. Test that the internalBoardState is completely "EMPTY" upon initialization
            [TestMethod]
            public void TestBoardIsEmptyOnInitialization()
            {
                int boardSize = 5; // Example board size
                GameLogicHandler game = new GameLogicHandler(boardSize: boardSize);

                List<List<String>> board = game.GetInternalBoardState();

                // Assert that each cell in the board is "EMPTY"
                foreach (var row in board)
                {
                    foreach (var cell in row)
                    {
                        Assert.AreEqual("EMPTY", cell, "The board should be initialized with 'EMPTY' in every cell.");
                    }
                }
            }
        }


        // Acceptance Criteria 1.1 & 3.1, 3.2: Choosing Board Size, Starting Game and Board Initialization
        // Made By ChatGPT
        [TestMethod]
        public void TestBoardDimensionsMatchBoardSize()
        {
            int boardSize = 5; // Example board size
            GameLogicHandler game = new GameLogicHandler(boardSize: boardSize);

            List<List<String>> board = game.GetInternalBoardState();

            // Assert that the number of rows is equal to boardSize
            Assert.AreEqual(boardSize, board.Count, "The number of rows should match the board size.");

            // Assert that the number of columns in each row is equal to boardSize
            foreach (var row in board)
            {
                Assert.AreEqual(boardSize, row.Count, "Each row should have the same number of columns as the board size.");
            }
        }


        // Acceptance Criteria N/A
        [TestMethod]
        public void TestOutOfBoundsCellReference()
        {
            // ARRANGE

            // create a game with a 3 x 3 board
            int size = 3;
            GameLogicHandler gameLogicHandler = new GameLogicHandler(boardSize: size);

            // ACT

            String testValue = gameLogicHandler.internalBoardStateRef(100, 100);  // refrence a cell that is obviously out of the bounds set by the boardSize
            String testValue2 = gameLogicHandler.internalBoardStateRef(3, 3);     // possible off-by-one error a user could make within reason
            String expectedValue = "OOB";

            // ASSERT

            Assert.AreEqual(testValue, expectedValue);
            Assert.AreEqual(testValue2, expectedValue);
        }


        // Acceptance Criteria 4.1: Make Move in Simple Game
        [TestMethod]
        public void TestMakingMoves()
        {
            // ARRANGE

            // create a game with a 3 x 3 board
            int size = 3;
            GameLogicHandler gameLogicHandler = new GameLogicHandler(boardSize: size);

            // ACT

            // simulate making some moves
            gameLogicHandler.UpdateInternalBoardState(0, 0, "S");
            gameLogicHandler.UpdateInternalBoardState(1, 1, "O");
            gameLogicHandler.UpdateInternalBoardState(2, 2, "S");

            // create cell references to test that the board state was properly updated
            String testValue1 = gameLogicHandler.internalBoardStateRef(0, 0);
            String testValue2 = gameLogicHandler.internalBoardStateRef(1, 1);
            String testValue3 = gameLogicHandler.internalBoardStateRef(2, 2);

            String expectedValue1 = "S";
            String expectedValue2 = "O";

            // ASSERT

            Assert.AreEqual(testValue1, expectedValue1);
            Assert.AreEqual(testValue2, expectedValue2);
            Assert.AreEqual(testValue3, expectedValue1);
        }


        // Acceptance Criteria 4.1: Making Move in Simple Game
        [TestMethod]
        public void TestChangingTurns()
        {
            // ARRANGE

            // create a game where it is Red's turn
            GameLogicHandler gameLogicHandler = new GameLogicHandler(isRedTurn: true, isBlueTurn: false);

            // ACT

            String testValue1 = gameLogicHandler.GetPlayerTurnColorName();

            gameLogicHandler.ChangeTurns();  // change turn to blue's turn
            String testValue2 = gameLogicHandler.GetPlayerTurnColorName();

            String expectedValue1 = "Red";
            String expectedValue2 = "Blue";

            // Assert

            Assert.AreEqual(testValue1, expectedValue1);
            Assert.AreEqual(testValue2, expectedValue2);
        }


        // Acceptance Criteria
        // Made by ChatGPT
        [TestMethod]
        public void TestSOSCreationWithO()
        {
            // ARRANGE: Create a 3x3 GameLogicHandler instance
            var gameLogic = new GameLogicHandler(boardSize: 3);

            // Setup an SOS configuration with 'S-O-S' horizontally
            gameLogic.UpdateInternalBoardState(1, 0, "S");  // Place 'S' at (1, 0)
            gameLogic.UpdateInternalBoardState(1, 1, "O");  // Place 'O' at (1, 1)
            gameLogic.UpdateInternalBoardState(1, 2, "S");  // Place 'S' at (1, 2)

            // ACT: Check for SOS in this configuration
            var moveInfo = new MoveInfo(1, 1, "O");  // Centered on the 'O'
            var sosInfo = gameLogic.GetSOSInfo(moveInfo);

            // ASSERT: Confirm SOS was detected
            Assert.AreEqual(1, sosInfo.NumSOS, "Expected 1 SOS pattern with central 'O'");
        }


        // Acceptance Criteria
        // Made by ChatGPT
        [TestMethod]
        public void TestSOSCreationWithS()
        {
            // ARRANGE: Create a 3x3 GameLogicHandler instance
            var gameLogic = new GameLogicHandler(boardSize: 3);

            // Setup an SOS configuration with 'S-O-S' vertically
            gameLogic.UpdateInternalBoardState(0, 1, "S");  // Place 'S' at (0, 1)
            gameLogic.UpdateInternalBoardState(1, 1, "O");  // Place 'O' at (1, 1)
            gameLogic.UpdateInternalBoardState(2, 1, "S");  // Place 'S' at (2, 1)

            // ACT: Check for SOS in this configuration
            var moveInfo = new MoveInfo(0, 1, "S");  // Starting point 'S'
            var sosInfo = gameLogic.GetSOSInfo(moveInfo);

            // ASSERT: Confirm SOS was detected
            Assert.AreEqual(1, sosInfo.NumSOS, "Expected 1 SOS pattern with starting 'S'");
        }


        // Acceptance Criteria
        [TestMethod]
        public void TestGetLineEndpointsIndices()
        {

            // ARRANGE

            GameLogicHandler game = new GameLogicHandler(boardSize: 3);

            // create a 3x3 board with 'S's in all spaces except center
            game.UpdateInternalBoardState(0, 0, "S");
            game.UpdateInternalBoardState(0, 1, "S");
            game.UpdateInternalBoardState(0, 2, "S");
            game.UpdateInternalBoardState(1, 0, "S");
            game.UpdateInternalBoardState(1, 2, "S");
            game.UpdateInternalBoardState(2, 0, "S");
            game.UpdateInternalBoardState(2, 1, "S");
            game.UpdateInternalBoardState(2, 2, "S");

            // ACT

            // place 'O' in the middle to make 4 SOSs
            MoveInfo move = new MoveInfo(1, 1, "O");
            SOSInfo sosInfo = game.GetSOSInfo(move);

            // extract the endpoint indices through which lines are drawn
            List<Vector2> vertcialSOSEndpoints = sosInfo.StartEndCoords[0];
            List<Vector2> horizontalSOSEndpoints = sosInfo.StartEndCoords[1];
            List<Vector2> upLeftSOSEndpoints = sosInfo.StartEndCoords[2];
            List<Vector2> upRightSOSEndpoints = sosInfo.StartEndCoords[3];

            // ASSERT

            // vertical pair
            Assert.AreEqual(vertcialSOSEndpoints[0], new Vector2(2, 1));
            Assert.AreEqual(vertcialSOSEndpoints[1], new Vector2(0, 1));

            // horizontal pair
            Assert.AreEqual(horizontalSOSEndpoints[0], new Vector2(1, 0));
            Assert.AreEqual(horizontalSOSEndpoints[1], new Vector2(1, 2));

            // diagonal up left pair
            Assert.AreEqual(upLeftSOSEndpoints[0], new Vector2(2, 2));
            Assert.AreEqual(upLeftSOSEndpoints[1], new Vector2(0, 0));

            // diagonal up right pair
            Assert.AreEqual(upRightSOSEndpoints[0], new Vector2(2, 0));
            Assert.AreEqual(upRightSOSEndpoints[1], new Vector2(0, 2));


        }


        [TestMethod]
        public void TestFillingBoard()
        {
            // ARRANGE

            GameLogicHandler game = new GameLogicHandler(boardSize: 3);

            // create a 3x3 board with '0's in all spaces except center
            game.UpdateInternalBoardState(0, 0, "0");
            game.UpdateInternalBoardState(0, 1, "0");
            game.UpdateInternalBoardState(0, 2, "0");
            game.UpdateInternalBoardState(1, 0, "0");
            game.UpdateInternalBoardState(1, 2, "0");
            game.UpdateInternalBoardState(2, 0, "0");
            game.UpdateInternalBoardState(2, 1, "0");
            game.UpdateInternalBoardState(2, 2, "0");

            // assert the board is not full
            Assert.AreEqual(game.IsBoardFull(), false);

            // ACT

            // simulate placing an 'O' in the remaining cell, filling the board
            game.UpdateInternalBoardState(1, 1, "O");

            // ASSERT

            // assert that the board is now full
            Assert.AreEqual(game.IsBoardFull(), true);
        }


        [TestMethod]
        public void TestSOSMovePredicate()
        {
            // ARRANGE

            // create a 3x3 board and place an S at indices (0, 0) and (0, 2)
            GameLogicHandler game = new GameLogicHandler(boardSize: 3);

            game.UpdateInternalBoardState(0, 0, "S");
            game.UpdateInternalBoardState(0, 2, "S");

            // create a move with letter "O" at index (0, 1)
            MoveInfo move = new MoveInfo(0, 1, "O");

            // ACT

            // invoke the predicate
            bool testValue = game.SOSMovePredicate(move);

            // ASSERT

            // assert that the game's SOSMovePredicate() function returns true when passing the move to it
            Assert.IsTrue(testValue);
        }


        [TestMethod]
        public void TestSmartMovePredicate()
        {
            // ARRANGE

            // create a 5x5 board and populate it with an 'S' at index (0, 0)
            GameLogicHandler game = new GameLogicHandler(boardSize: 5);

            game.UpdateInternalBoardState(0, 0, "S");

            // create a "smart" move and a "dumb" move
            // smart move will not allow the next turn's player to create an SOS
            // dumb move will
            MoveInfo smartMove = new MoveInfo(0, 1, "S");
            MoveInfo dumbMove = new MoveInfo(0, 1, "O");

            // ACT

            // invoke the predicate
            bool smartTestValue = game.SmartMovePredicate(smartMove);
            bool dumbTestValue = game.SmartMovePredicate(dumbMove);

            // ASSERT

            // assert the invocations of the predicate function returned true and false for the smart move
            // and the dumb move respectively
            Assert.IsTrue(smartTestValue);
            Assert.IsFalse(dumbTestValue);
        }


        [TestMethod]
        public void TestComputerSOSPlacement()
        {
            // ARRANGE

            // create a 5x5 board and populate it with 'S's and 'O's such that there are
            // 4 possible SOSs:

            // S S S _ _
            // S _ S _ _
            // S S S _ _
            // _ _ _ _ _
            // _ _ _ _ _

            GameLogicHandler game = new GameLogicHandler(boardSize: 5);
            game.UpdateInternalBoardState(0, 0, "S");
            game.UpdateInternalBoardState(0, 1, "S");
            game.UpdateInternalBoardState(0, 2, "S");
            game.UpdateInternalBoardState(1, 0, "S");
            game.UpdateInternalBoardState(1, 2, "S");
            game.UpdateInternalBoardState(2, 0, "S");
            game.UpdateInternalBoardState(2, 1, "S");
            game.UpdateInternalBoardState(2, 2, "S");

            // ACT

            // invoke the GenerateComputerMove() function
            MoveInfo testMove = game.GenerateComputerMove();

            // ASSERT

            // the computer-generated move should be guaranteed to be an 'O' at index (1, 1)
            Assert.AreEqual(testMove.MoveLetter, "O");
            Assert.AreEqual(testMove.RowIndex, 1);
            Assert.AreEqual(testMove.ColumnIndex, 1);
        }
    }
}