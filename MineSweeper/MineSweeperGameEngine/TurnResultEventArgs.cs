using System;

namespace MineSweeperGameEngine
{
    /// <summary>
    /// Custom arguments for the TurnResult event.
    /// </summary>
    public class TurnResultEventArgs : EventArgs
    {
        /// <summary>
        /// Defines the result of selecting a cell.
        /// </summary>
        public enum TurnResult
        {
            /// <summary>
            /// Indicates that an error occurred selecting the cell.
            /// </summary>
            InvalidCellSelected,

            /// <summary>
            /// Indicates that an unknown error occurred.
            /// </summary>
            UnknownError,

            /// <summary>
            /// Indicates that the selected cell is already exposed.
            /// </summary>
            CellIsAlreadyExposed,

            /// <summary>
            /// Cell is already flagged.
            /// </summary>
            CellIsAlreadyFlagged,

            /// <summary>
            /// Cell is not flagged.
            /// </summary>
            CellIsNotFlagged,

            /// <summary>
            /// Indicates that the selected cell was exposed
            /// </summary>
            CellExposed,

            /// <summary>
            /// Indicates that the player loses.
            /// </summary>
            PlayerLoses,

            /// <summary>
            /// Indicates that the player wins.
            /// </summary>
            PlayerWins,

            /// <summary>
            /// The cell was flagged.
            /// </summary>
            CellFlagged,

            /// <summary>
            /// The cell was unflagged.
            /// </summary>
            CellUnflagged
        }

        /// <summary>
        /// Gets or sets the result of the turn.
        /// </summary>
        /// <value>The result of the turn.</value>
        public TurnResult Result { get; set; }

        /// <summary>
        /// Gets or sets the row index.
        /// </summary>
        /// <value>The row index.</value>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the column index.
        /// </summary>
        /// <value>The column index.</value>
        public int ColumnIndex { get; set; }
    }
}
