using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeperGameEngine;

namespace MineSweeperGameEngineTests
{
    /// <summary>
    /// Tests a game in which the game is aborted because there are no
    /// subscribers to required events.
    /// </summary>
    [TestClass]
    public class TestGameAborted
    {
        /// <summary>
        /// Tests playing the game.
        /// </summary>
        [TestMethod]
        public void TestPlayGame()
        {
            // Initialize board size.
            var n = 1;

            // Initialize hard coded bomb list.
            var bombFlagsList = new List<bool>();
            for (int i = 0; i < n * n; i++)
                bombFlagsList.Add(false);

            // Initialize bombs.
            bombFlagsList[0] = true;

            // Initialize game.
            var game = new Game(n, bombFlagsList);

            // Play the game.
            Assert.AreEqual(Game.GameResult.GameAborted, game.Play());
        }
    }
}
