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
    public class UnitTest1
    {

        // Acceptance Criteria 2.1 & 2.2: Simple and General Game Selection
        [TestMethod]
        public void TestGameModeSelection()
        {
            // ARRANGE

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


        // Acceptance Criteria 3.1 & 3.2: Starting Game and Board Initialization
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


        // Acceptance Criteria 3.1 & 3.2: Starting Game and Board Initialization
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


        // Acceptance Criteria
        [TestMethod]
        public void TestBoardSize()
        {

        }
    }
}