using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MineSweeperGameEngine;

namespace MineSweeperGameEngineTests
{
    /// <summary>
    /// Tests a game in which the player wins.
    /// </summary>
    [TestClass]
    public class TestPlayerWins
    {
        /// <summary>
        /// Keeps track of which turn it is.
        /// </summary>
        private int turnCount;

        /// <summary>
        /// Tests playing the game.
        /// </summary>
        [TestMethod]
        public void TestPlayGame()
        {
            // Initialize board size.
            var n = 3;

            // Initialize hard coded bomb list.
            var bombFlagsList = new List<bool>();
            for (int i = 0; i < n * n; i++)
                bombFlagsList.Add(false);

            // Initialize bombs.
            bombFlagsList[0] = true;
            bombFlagsList[8] = true;

            // Initialize game.
            var game = new Game(n, bombFlagsList);

            // Subscribe to events.
            game.PlayersTurn += PlayersTurn;
            game.TurnResult += TurnResult;

            // Play the game.
            Assert.AreEqual(Game.GameResult.PlayerWins, game.Play());
        }

        /// <summary>
        /// Handler for the TurnResult event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void TurnResult(object sender, TurnResultEventArgs e)
        {
            if (turnCount == 2)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellIsAlreadyExposed);
                return;
            }

            if (turnCount == 5)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellIsNotFlagged);
                return;
            }

            if (turnCount == 6)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellFlagged);
                return;
            }

            if (turnCount == 7)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellIsAlreadyFlagged);
                return;
            }

            if (turnCount == 8)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellUnflagged);
                return;
            }

            if (turnCount == 9)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellFlagged);
                return;
            }

            if (turnCount == 13)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellIsAlreadyExposed);
                return;
            }

            if (turnCount > 0 && turnCount < 14)
            {
                Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.CellExposed);
                return;
            }

            Assert.AreEqual(e.Result, TurnResultEventArgs.TurnResult.PlayerWins);
        }

        /// <summary>
        /// Handler for the PlayersTurn event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void PlayersTurn(object sender, TurnEventArgs e)
        {
            switch (turnCount)
            {
                // Test showing a cell.
                case 0:
                    e.RowIndex = 1;
                    e.ColumnIndex = 1;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test showing a cell.
                case 1:
                    e.RowIndex = 1;
                    e.ColumnIndex = 1;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test showing a cell.
                case 2:
                    e.RowIndex = 0;
                    e.ColumnIndex = 1;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test showing a cell.
                case 3:
                    e.RowIndex = 1;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test attempting to unflag a cell that is not flagged.
                case 4:
                    e.RowIndex = 0;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Unflag;
                    turnCount++;
                break;

                // Test flagging a cell.
                case 5:
                    e.RowIndex = 0;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Flag;
                    turnCount++;
                    break;

                // Test flagging already flagged cell.
                case 6:
                    e.RowIndex = 0;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Flag;
                    turnCount++;
                    break;

                // Test unflagging a cell.
                case 7:
                    e.RowIndex = 0;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Unflag;
                    turnCount++;
                    break;

                // Test re-flagging a cell.
                case 8:
                    e.RowIndex = 0;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Flag;
                    turnCount++;
                    break;

                // Test showing a cell.
                case 9:
                    e.RowIndex = 2;
                    e.ColumnIndex = 1;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test showing a cell.
                case 10:
                    e.RowIndex = 1;
                    e.ColumnIndex = 2;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test showing a cell.
                case 11:
                    e.RowIndex = 2;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test showing an already shown cell.
                case 12:
                    e.RowIndex = 2;
                    e.ColumnIndex = 0;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;

                // Test showing a cell.
                case 13:
                    e.RowIndex = 0;
                    e.ColumnIndex = 2;
                    e.Op = TurnEventArgs.TurnOp.Expose;
                    turnCount++;
                    break;
            }
        }
    }
}
