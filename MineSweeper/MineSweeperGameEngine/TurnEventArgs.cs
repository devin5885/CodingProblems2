using System;

namespace MineSweeperGameEngine
{
    /// <summary>
    /// Custom arguments for the players turn event.
    /// </summary>
    public class TurnEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TurnEventArgs"/> class.
        /// </summary>
        public TurnEventArgs()
        {
            Op = TurnOp.Unknown;
        }

        /// <summary>
        /// Custom players turn operations.
        /// </summary>
        public enum TurnOp
        {
            /// <summary>
            /// Unknown operation.
            /// </summary>
            Unknown,

            /// <summary>
            /// Player wants to quit the game.
            /// </summary>
            Quit,

            /// <summary>
            /// Player wants to expose a cell.
            /// </summary>
            Expose,

            /// <summary>
            /// Player wants to flag a cell.
            /// </summary>
            Flag,

            /// <summary>
            /// Player wants to unflag a cell.
            /// </summary>
            Unflag
        }

        /// <summary>
        /// Gets or sets the operation.
        /// </summary>
        /// <value>The operation.</value>
        public TurnOp Op { get; set; }

        /// <summary>
        /// Gets or sets the row index.
        /// </summary>
        /// <value>The row.</value>
        public int RowIndex { get; set; }

        /// <summary>
        /// Gets or sets the column index.
        /// </summary>
        /// <value>The column.</value>
        public int ColumnIndex { get; set; }
    }
}
