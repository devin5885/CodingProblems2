using System;
using System.Collections.Generic;

namespace MineSweeperGameEngine
{
    /// <summary>
    /// Represents a game.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="boardSize">The board size.</param>
        /// <param name="bombCount">The bomb count.</param>
        public Game(int boardSize, int bombCount)
        {
            Board = new Board(boardSize, bombCount);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class.
        /// </summary>
        /// <param name="boardSize">The board size.</param>
        /// <param name="bombFlagsList">The list describing where bombs should be placed.</param>
        public Game(int boardSize, List<bool> bombFlagsList)
        {
            Board = new Board(boardSize, bombFlagsList);
        }

        /// <summary>
        /// Raised when the game board needs to be updated.
        /// </summary>
        public event EventHandler UpdateGameBoard;

        /// <summary>
        /// Called when it is the players turn.
        /// </summary>
        public event EventHandler<TurnEventArgs> PlayersTurn;

        /// <summary>
        /// Called when the players action has been processed.
        /// </summary>
        public event EventHandler<TurnResultEventArgs> TurnResult;

        /// <summary>
        /// The result of the game.
        /// </summary>
        public enum GameResult
        {
            /// <summary>
            /// The player won the game.
            /// </summary>
            PlayerWins,

            /// <summary>
            /// The player lost the game.
            /// </summary>
            PlayerLoses,

            /// <summary>
            /// The player quits.
            /// </summary>
            PlayerQuits,

            /// <summary>
            /// The game was aborted.
            /// </summary>
            GameAborted
        }

        /// <summary>
        /// Gets the game board.
        /// </summary>
        /// <value>The game board.</value>
        private Board Board { get; }

        /// <summary>
        /// Plays the game.
        /// </summary>
        /// <returns>The game result.</returns>
        public GameResult Play()
        {
            // Initialize a new board.
            Board.InitBoard();

            while (true)
            {
                // Display the updated game board.
                UpdateGameBoard?.Invoke(this, new EventArgs());

                // Check for error.
                if (PlayersTurn == null)
                    return GameResult.GameAborted;

                // Handle the players turn.
                var args = new TurnEventArgs();
                PlayersTurn.Invoke(this, args);

                // Check for quit.
                if (args.Op == TurnEventArgs.TurnOp.Quit)
                    return GameResult.PlayerQuits;

                // Check for error.
                if (args.Op == TurnEventArgs.TurnOp.Unknown)
                    return GameResult.GameAborted;

                // Carry out the player action.
                var result = Board.OnTurn(args.Op, args.RowIndex, args.ColumnIndex);

                // Make sure the game board is updated.
                if (result == TurnResultEventArgs.TurnResult.PlayerLoses ||
                    result == TurnResultEventArgs.TurnResult.PlayerWins)
                    UpdateGameBoard?.Invoke(this, new EventArgs());

                // Process the turn result.
                var argsTurnResult = new TurnResultEventArgs
                {
                    ColumnIndex = args.ColumnIndex,
                    RowIndex = args.RowIndex,
                    Result = result
                };
                TurnResult?.Invoke(this, argsTurnResult);

                // Handle loss.
                if (result == TurnResultEventArgs.TurnResult.PlayerLoses)
                    return GameResult.PlayerLoses;

                // Handle win.
                if (result == TurnResultEventArgs.TurnResult.PlayerWins)
                    return GameResult.PlayerWins;
            }
        }

        /// <summary>
        /// Gets the specified cell.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="col">The column index.</param>
        /// <returns>The cell.</returns>
        public Cell GetCell(int row, int col)
        {
            return Board.GetCell(row, col);
        }

        /// <summary>
        /// Returns the count of bombs minus the number of flagged cells.
        /// </summary>
        /// <returns>The count of bombs minus the number of flagged cells.</returns>
        public int GetUnFlaggedBombCountRemaining()
        {
            return Board.GetUnFlaggedBombCountRemaining();
        }
    }
}
