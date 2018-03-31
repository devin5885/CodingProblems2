using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeperGameEngine
{
    /// <summary>
    /// Represents the game board.
    /// </summary>
    public class Board
    {
        /// <summary>
        /// The cells of the game board.
        /// </summary>
        private Cell[,] cells;

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        /// <param name="boardSize">The size of the board.</param>
        /// <param name="bombCount">The count of bombs on the board.</param>
        public Board(int boardSize, int bombCount)
        {
            BoardSize = boardSize;
            BombCount = bombCount;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Board"/> class.
        /// </summary>
        /// <param name="boardSize">The size of the board.</param>
        /// <param name="bombFlagsList">The list describing where bombs should be placed.</param>
        public Board(int boardSize, List<bool> bombFlagsList)
        {
            BoardSize = boardSize;
            BombCount = bombFlagsList.Count(b => b);
            BombFlagsList = bombFlagsList;
        }

        /// <summary>
        ///  Gets the size of the board (n x n)
        /// </summary>
        /// <value>The size of the board (n x n)</value>
        public int BoardSize { get; }

        /// <summary>
        ///  Gets the count of bombs on the board.
        /// </summary>
        /// <value>The count of bombs on the board.</value>
        public int BombCount { get; }

        /// <summary>
        ///  Gets or sets the specified BombFlagsList.
        /// </summary>
        /// <value>A list describing where to place bombs.</value>
        private List<bool> BombFlagsList { get; set; }

        /// <summary>
        /// Initializes the board (in prep for a new game).
        /// </summary>
        public void InitBoard()
        {
            // Add cells to the boards.
            cells = new Cell[BoardSize, BoardSize];
            for (var col = 0; col < BoardSize; col++)
                for (var row = 0; row < BoardSize; row++)
                    cells[col, row] = new Cell();

            // Initialize bomb cells.
            InitBombCells();

            // Initialize bomb counts for cells.
            InitBombCounts();
        }

        /// <summary>
        /// Handler for selecting a cell.
        /// </summary>
        /// <param name="op">The operation.</param>
        /// <param name="row">The row.</param>
        /// <param name="col">The column.</param>
        /// <returns>The result of the selection.</returns>
        public TurnResultEventArgs.TurnResult OnTurn(TurnEventArgs.TurnOp op, int row, int col)
        {
            // Get the cell.
            var cell = GetCell(row, col);

            // Check for null.
            if (cell == null)
                return TurnResultEventArgs.TurnResult.InvalidCellSelected;

            // Unflag the cell.
            switch (op)
            {
                // Unflag the cell.
                case TurnEventArgs.TurnOp.Unflag:

                    // Can't unflag a non-flagged cell.
                    if (!cell.IsFlagged)
                        return TurnResultEventArgs.TurnResult.CellIsNotFlagged;

                    cell.IsFlagged = false;
                    return TurnResultEventArgs.TurnResult.CellUnflagged;

                // Flag the cell.
                case TurnEventArgs.TurnOp.Flag:

                    // Can't flag a visible cell.
                    if (cell.IsExposed)
                        return TurnResultEventArgs.TurnResult.CellIsAlreadyExposed;

                    // Check for already flagged cell.
                    if (cell.IsFlagged)
                        return TurnResultEventArgs.TurnResult.CellIsAlreadyFlagged;

                    cell.IsFlagged = true;
                    return TurnResultEventArgs.TurnResult.CellFlagged;

                // Expose the cell if requested.
                case TurnEventArgs.TurnOp.Expose:

                    // Check for already visible cell.
                    if (cell.IsExposed)
                        return TurnResultEventArgs.TurnResult.CellIsAlreadyExposed;

                    // Handle bomb case.
                    if (cell.IsBomb)
                    {
                        cell.IsExposed = true;
                        return TurnResultEventArgs.TurnResult.PlayerLoses;
                    }

                    // Expose the adjacent cells.
                    ShowAdjacentCells(row, col);

                    // Check for win.
                    if (CheckForPlayerWin())
                        return TurnResultEventArgs.TurnResult.PlayerWins;

                    // Cell shown.
                    return TurnResultEventArgs.TurnResult.CellExposed;

                // Handle error.
                default:

                    return TurnResultEventArgs.TurnResult.UnknownError;
            }
        }

        /// <summary>
        /// Returns a particular cell.
        /// </summary>
        /// <param name="row">The row of the cell.</param>
        /// <param name="col">The column of the cell.</param>
        /// <returns>The cell or null if no such cell.</returns>
        public Cell GetCell(int row, int col)
        {
            // Check for invalid cell indexes.
            if (!IsValidRowIndex(row) || !IsValidColumnIndex(col))
                return null;

            // Return cell.
            return cells[row, col];
        }

        /// <summary>
        /// Returns the count of bombs minus the number of flagged cells.
        /// </summary>
        /// <returns>The count of bombs minus the number of flagged cells.</returns>
        public int GetUnFlaggedBombCountRemaining()
        {
            // Get the count of flagged cells.
            int result = 0;
            for (var col = 0; col < BoardSize; col++)
                for (var row = 0; row < BoardSize; row++)
                {
                    var cell = GetCell(row, col);
                    if (cell.IsFlagged)
                        result++;
                }

            // Return the count of non-flagged bombs remaining.
            return BombCount - result;
        }

        /// <summary>
        /// Initializes the bomb cells.
        /// </summary>
        private void InitBombCells()
        {
            // If not specified in constructor create a list & initialize
            // all to false.
            if (BombFlagsList == null)
            {
                BombFlagsList = new List<bool>();

                for (int i = 0; i < BoardSize * BoardSize; i++)
                    BombFlagsList.Add(false);

                // Initialize first # entries to Bombs.
                for (int i = 0; i < BombCount; i++)
                    BombFlagsList[i] = true;

                // Initialize random number generator.
                var random = new Random();

                // Swap the bomb entries.
                for (int i = 0; i < BombCount; i++)
                {
                    // Get random.
                    var to = random.Next(i, BombFlagsList.Count - 1);

                    // Do swap.
                    if (to != i)
                    {
                        var temp = BombFlagsList[i];
                        BombFlagsList[i] = BombFlagsList[to];
                        BombFlagsList[to] = temp;
                    }
                }
            }

            // Now apply bombs to grid.
            for (int i = 0; i < BoardSize * BoardSize; i++)
            {
                if (BombFlagsList[i])
                {
                    // Get row, col.
                    var row = i / BoardSize;
                    var col = (i - (row * BoardSize)) % BoardSize;
                    GetCell(row, col).IsBomb = true;
                }
            }
        }

        /// <summary>
        /// Initialize the bomb counts for the cells.
        /// </summary>
        private void InitBombCounts()
        {
            for (var col = 0; col < BoardSize; col++)
                for (var row = 0; row < BoardSize; row++)
                {
                    // For bomb cells update adjacent cells.
                    var cell = GetCell(row, col);
                    if (cell.IsBomb)
                        IncrementBombCount(row, col);
                }
        }

        /// <summary>
        /// Increments the bomb count for the specified cell and adjacent cells.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="col">The column.</param>
        private void IncrementBombCount(int row, int col)
        {
            // Adjacent cells in row above.
            Cell cell = GetCell(row - 1, col - 1);
            if (cell != null)
                cell.BombCount++;

            cell = GetCell(row - 1, col);
            if (cell != null)
                cell.BombCount++;

            cell = GetCell(row - 1, col + 1);
            if (cell != null)
                cell.BombCount++;

            // Adjacent cells in same row.
            cell = GetCell(row, col - 1);
            if (cell != null)
                cell.BombCount++;

            cell = GetCell(row, col + 1);
            if (cell != null)
                cell.BombCount++;

            // Adjacent cells in row below.
            cell = GetCell(row + 1, col - 1);
            if (cell != null)
                cell.BombCount++;

            cell = GetCell(row + 1, col);
            if (cell != null)
                cell.BombCount++;

            cell = GetCell(row + 1, col + 1);
            if (cell != null)
                cell.BombCount++;
        }

        /// <summary>
        /// Shows adjacent cells for the specified cell row/col.
        /// (Recursively).
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="col">The col.</param>
        private void ShowAdjacentCells(int row, int col)
        {
            // Get this cell.
            var cell = GetCell(row, col);

            // Ignore invalid cells.
            if (cell == null)
                return;

            // Skip already exposed cells.
            if (cell.IsExposed)
                return;

            // Expose this cell.
            cell.IsExposed = true;

            // If this is an empty cell we need to show it's neighbors.
            if (cell.BombCount == 0)
            {
                // Row above.
                ShowAdjacentCells(row - 1, col - 1);
                ShowAdjacentCells(row - 1, col);
                ShowAdjacentCells(row - 1, col + 1);

                // Same row.
                ShowAdjacentCells(row, col - 1);
                ShowAdjacentCells(row, col + 1);

                // Row below.
                ShowAdjacentCells(row + 1, col - 1);
                ShowAdjacentCells(row + 1, col);
                ShowAdjacentCells(row + 1, col + 1);
            }
        }

        /// <summary>
        /// Shows all remaining cells.
        /// </summary>
        private void ShowAllCells()
        {
            // Handle all cells.
            for (var col = 0; col < BoardSize; col++)
                for (var row = 0; row < BoardSize; row++)
                    GetCell(row, col).IsExposed = true;
        }

        /// <summary>
        /// Checks whether the player has won.
        /// </summary>
        /// <returns>True if the player has won, false otherwise.</returns>
        private bool CheckForPlayerWin()
        {
            // Check whether all remaining hidden cells are marked.
            for (var col = 0; col < BoardSize; col++)
                for (var row = 0; row < BoardSize; row++)
                {
                    // Get the cell.
                    var cell = GetCell(row, col);

                    // If we find a non-exposed cell that is not a bomb, the player has not yet won.
                    if (!cell.IsExposed && !cell.IsBomb)
                        return false;
                }

            // Player won, show all cells & return true.
            ShowAllCells();
            return true;
        }

        /// <summary>
        /// Checks whether the specified row index is valid.
        /// </summary>
        /// <param name="rowIndex">Th row index.</param>
        /// <returns>True if the row index is valid, false otherwise.</returns>
        private bool IsValidRowIndex(int rowIndex)
        {
            return rowIndex >= 0 && rowIndex < BoardSize;
        }

        /// <summary>
        /// Checks whether the specified column index is valid.
        /// </summary>
        /// <param name="columnIndex">Th column index.</param>
        /// <returns>True if the column index is valid, false otherwise.</returns>
        private bool IsValidColumnIndex(int columnIndex)
        {
            return columnIndex >= 0 && columnIndex < BoardSize;
        }
    }
}
