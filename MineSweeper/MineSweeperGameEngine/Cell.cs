using System;

namespace MineSweeperGameEngine
{
    /// <summary>
    /// Represents a cell on the game board.
    /// </summary>
    public class Cell
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        public Cell()
        {
            IsFlagged = false;
            IsBomb = false;
            IsExposed = false;
            BombCount = 0;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is exposed.
        /// </summary>
        /// <value>Whether the cell is exposed.</value>
        public bool IsExposed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell has been flagged by the player.
        /// </summary>
        /// <value>Whether the cell has been flagged by the player.</value>
        public bool IsFlagged { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the cell is a bomb cell.
        /// </summary>
        /// <value>Whether the cell is a bomb cell.</value>
        public bool IsBomb { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the bomb count of adjacent cells.
        /// (0 for bomb cell or for empty cell).
        /// </summary>
        /// <value>The bomb count of adjacent cells.</value>
        public int BombCount { get; set; }
    }
}
