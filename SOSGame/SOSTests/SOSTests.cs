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
    }
}