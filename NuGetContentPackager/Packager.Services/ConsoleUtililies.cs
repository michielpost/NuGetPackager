using System;

namespace Packager.Services
{
    /// <summary>
    /// Utilities and extensions for console messages
    /// </summary>
    public static class ConsoleUtililies
    {
        #region Progress indicator - taken from http://www.bytechaser.com/en/articles/ckcwh8nsyt/display-progress-bar-in-console-application-in-c.aspx

        /// <summary>
        /// Overwrites the console message.
        /// </summary>
        /// <param name="message">The message.</param>
        public static void OverwriteConsoleMessage(string message)
        {
            Console.CursorLeft = 0;
            var maxCharacterWidth = Console.WindowWidth - 1;
            if (message.Length > maxCharacterWidth)
            {
                message = message.Substring(0, maxCharacterWidth - 3) + "...";
            }
            message = message + new string(' ', maxCharacterWidth - message.Length);
            Console.Write(message);
        }

        /// <summary>
        /// Renders the console progress.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <param name="progressBarCharacter">The progress bar character.</param>
        /// <param name="message">The message.</param>
        public static void RenderConsoleProgress(int percentage,
            char progressBarCharacter = '\u2590',
            string message = "")
        {
            RenderConsoleProgress(percentage, null, progressBarCharacter, message);
        }

        /// <summary>
        /// Renders the console progress.
        /// </summary>
        /// <param name="percentage">The percentage.</param>
        /// <param name="progressBarCharacter">The progress bar character.</param>
        /// <param name="color">The color.</param>
        /// <param name="message">The message.</param>
        public static void RenderConsoleProgress(
            int percentage,
            ConsoleColor? color,
            char progressBarCharacter = '\u2590',
            string message = "")
        {
            //Remember console settings
            var originalCursorVisible = Console.CursorVisible;
            var originalColor = Console.ForegroundColor;

            //Change console settings
            Console.CursorVisible = false;
            Console.ForegroundColor = color.GetValueOrDefault(Console.ForegroundColor);

            //Calculate width of progress bar
            var width = Console.WindowWidth - 1;
            var newWidth = (int)((width * percentage) / 100d);
            var progBar = new string(progressBarCharacter, newWidth) +
                          new string(' ', width - newWidth);

            //Write output
            Console.CursorLeft = 0;
            Console.Write(progBar);
            if (string.IsNullOrEmpty(message)) message = "";
            Console.CursorTop++;
            OverwriteConsoleMessage(message);
            Console.CursorTop--;

            //Restore console settings
            Console.ForegroundColor = originalColor;
            Console.CursorVisible = originalCursorVisible;
        }

        #endregion
    }
}