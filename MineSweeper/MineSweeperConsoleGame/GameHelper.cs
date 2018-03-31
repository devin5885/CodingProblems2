using System;
using System.Linq;
using MineSweeperGameEngine;

namespace MinesweeperConsoleGame
{
    /// <summary>
    /// Helper class for Minesweeper console game.
    /// </summary>
    public class GameHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameHelper"/> class.
        /// </summary>
        public GameHelper()
        {
            // Hard code for now.
            BoardSize = 9;
            BombCount = 10;
        }

        /// <summary>
        /// Gets or sets the board size.
        /// </summary>
        /// <value>The board size.</value>
        public int BoardSize { get; set; }

        /// <summary>
        /// Gets or sets the bomb count.
        /// </summary>
        /// <value>The bomb count.</value>
        public int BombCount { get; set; }

        /// <summary>
        /// Gets or sets the game object.
        /// </summary>
        /// <value>The game object.</value>
        public Game Game { get; set; }

        /// <summary>
        /// Plays the Minesweeper console game.
        /// </summary>
        public void Play()
        {
            // Play.
            Game = new Game(BoardSize, BombCount);

            // Subscribe to events.
            Game.UpdateGameBoard += UpdateGameBoard;
            Game.PlayersTurn += PlayersTurn;
            Game.TurnResult += TurnResult;

            // Continue playing.
            var playing = true;
            while (playing)
            {
                // Clear the console.
                Console.Clear();

                // Play the game.
                Game.Play();

                // Prompt.
                Console.WriteLine("Play Again? (y/n)");
                var resp = Console.ReadLine();
                if (!string.IsNullOrEmpty(resp))
                {
                    resp = resp.ToUpper();
                    if (resp != "Y")
                        playing = false;
                }
            }
        }

        /// <summary>
        /// Handler for the UpdateGameBoard event. (Displays the game board).
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void UpdateGameBoard(object sender, EventArgs e)
        {
            // Build spacer string.
            var spacer = new string('-', ((BoardSize + 1) * 4) + 1);

            // Expose rows & cols.
            for (var row = -1; row < BoardSize; row++)
            {
                Console.WriteLine(spacer);

                var rowDisplayString = string.Empty;
                for (var col = -1; col < BoardSize; col++)
                {
                    var indicator = GetCellIndicator(row, col);
                    rowDisplayString += "| " + indicator + " ";
                }
                rowDisplayString += "|";
                Console.WriteLine(rowDisplayString);
            }
            Console.WriteLine(spacer);
            Console.WriteLine($"Hidden Bombs Remaining: {Game.GetUnFlaggedBombCountRemaining()}");
        }

        /// <summary>
        /// Handler for the TurnResult event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void TurnResult(object sender, TurnResultEventArgs e)
        {
            Console.Beep();

            if (e.Result == TurnResultEventArgs.TurnResult.CellFlagged)
                Console.WriteLine($"The cell ({e.RowIndex}, {e.ColumnIndex}) was flagged.");

            if (e.Result == TurnResultEventArgs.TurnResult.CellIsAlreadyFlagged)
                Console.WriteLine($"The cell ({e.RowIndex}, {e.ColumnIndex}) was already flagged.");

            if (e.Result == TurnResultEventArgs.TurnResult.CellIsAlreadyExposed)
                Console.WriteLine($"The cell ({e.RowIndex}, {e.ColumnIndex}) was already visible.");

            if (e.Result == TurnResultEventArgs.TurnResult.CellExposed)
                Console.WriteLine($"The cell ({e.RowIndex}, {e.ColumnIndex}) is now visible.");

            if (e.Result == TurnResultEventArgs.TurnResult.CellUnflagged)
                Console.WriteLine($"The cell ({e.RowIndex}, {e.ColumnIndex}) was unflagged.");

            if (e.Result == TurnResultEventArgs.TurnResult.InvalidCellSelected ||
                e.Result == TurnResultEventArgs.TurnResult.UnknownError)
                Console.WriteLine("Internal error.");

            if (e.Result == TurnResultEventArgs.TurnResult.PlayerWins)
                Console.WriteLine("You Win !!");

            if (e.Result == TurnResultEventArgs.TurnResult.PlayerLoses)
            {
                Console.WriteLine($"The cell ({e.RowIndex}, {e.ColumnIndex}) is a BOMB!");
                Console.WriteLine("You Lose !!");
            }
        }

        /// <summary>
        /// Handler for the PlayersTurn event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event arguments.</param>
        public void PlayersTurn(object sender, TurnEventArgs e)
        {
            while (true)
            {
                // Prompt.
                Console.WriteLine("Specify Operation(S=Expose F=Flag U=Unflag Q=Quit),Row Index,Column Index");

                // Get the input.
                var input = Console.ReadLine();

                // Parse the input.
                if (!string.IsNullOrEmpty(input))
                {
                    // Convert to upper case.
                    input = input.ToUpper();

                    if (input.IndexOf("Q") != -1)
                    {
                        e.Op = TurnEventArgs.TurnOp.Quit;
                        return;
                    }
                    else
                    {
                        var split = input.Split(',').ToList();
                        if (split.Count == 3)
                        {
                            // Determine the op.
                            if (split[0] == "F")
                                e.Op = TurnEventArgs.TurnOp.Flag;

                            // Determine the op.
                            if (split[0] == "U")
                                e.Op = TurnEventArgs.TurnOp.Unflag;

                            if (split[0] == "S")
                                e.Op = TurnEventArgs.TurnOp.Expose;

                            // If we were able to determine the op.
                            if (e.Op != TurnEventArgs.TurnOp.Unknown)
                            {
                                try
                                {
                                    e.RowIndex = Convert.ToInt32(split[1]) - 1;
                                    e.ColumnIndex = Convert.ToInt32(split[2]) - 1;
                                    return;
                                }
                                catch (FormatException)
                                {
                                    // Ignore format exception.
                                }
                            }
                        }
                    }

                    // Try again.
                    Console.WriteLine("Unable to parse input, please retry.");
                }
            }
        }

        /// <summary>
        /// Gets the cell indicator for the cell.
        /// </summary>
        /// <param name="rowIndex">The rowIndex index.</param>
        /// <param name="columnIndex">The column index.</param>
        /// <returns>The indicator.</returns>
        private string GetCellIndicator(int rowIndex, int columnIndex)
        {
            // Handle Id columns.
            if (rowIndex == -1 && columnIndex == -1)
                return " ";
            else if (rowIndex == -1)
                return (columnIndex + 1).ToString();
            else if (columnIndex == -1)
                return (rowIndex + 1).ToString();

            // Handle grid columns.

            // Get the cell.
            Cell cell = Game.GetCell(rowIndex, columnIndex);
            if (cell == null)
                throw new InvalidOperationException("Invalid row/column index");

            // Return appropriate indicator.
            if (!cell.IsExposed)
                return "?";

            if (cell.IsFlagged)
                return "*";

            if (cell.IsBomb)
                return "B";

            // Use bomb count if available.
            var bombCount = cell.BombCount;
            if (bombCount > 0)
                return bombCount.ToString();

            // Default to blank.
            return " ";
        }
    }
}
