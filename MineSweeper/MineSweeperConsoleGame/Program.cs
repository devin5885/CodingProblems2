using System;

namespace MinesweeperConsoleGame
{
    /// <summary>
    /// Program class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            Play();
        }

        /// <summary>
        /// Plays the Minesweeper console game.
        /// </summary>
        private static void Play()
        {
            var gameHelper = new GameHelper();
            gameHelper.Play();
        }
    }
}
