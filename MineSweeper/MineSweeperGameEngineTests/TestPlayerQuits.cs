using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeperGameEngine;

namespace MineSweeperGameEngineTests
{
    /// <summary>
    /// Tests a game in which the player quits.
    /// </summary>
    [TestClass]
    public class TestPlayerQuits
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

            // Subscribe to events.
            game.PlayersTurn += PlayersTurn;
            game.TurnResult += TurnResult;

            // Play the game.
            Assert.AreEqual(Game.GameResult.PlayerQuits, game.Play());
        }

        /// <summary>
        /// Handler for the TurnResult event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void TurnResult(object sender, TurnResultEventArgs e)
        {
        }

        /// <summary>
        /// Handler for the PlayersTurn event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void PlayersTurn(object sender, TurnEventArgs e)
        {
            e.RowIndex = 0;
            e.ColumnIndex = 0;
            e.Op = TurnEventArgs.TurnOp.Quit;
        }
    }
}
