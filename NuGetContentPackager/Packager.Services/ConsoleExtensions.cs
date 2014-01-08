using System;
using System.Runtime.InteropServices;

namespace NuGetContentPackagerConsole
{
    /// <summary>
    /// Console extensions
    /// </summary>
    /// <remarks>Taken from http://stackoverflow.com/a/3453272/201019 </remarks>
    public static class ConsoleExtensions
    {
        /// <summary>
        /// Gets a value indicating whether output is redirected
        /// </summary>
        /// <value>
        /// returns <c>true</c> if output is redirected; otherwise, <c>false</c>.
        /// </value>
        public static bool IsOutputRedirected
        {
            get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdout)); }
        }

        /// <summary>
        /// Gets a value indicating whether input is redirected
        /// </summary>
        /// <value>
        /// returns <c>true</c> if input is redirected; otherwise, <c>false</c>.
        /// </value>
        public static bool IsInputRedirected
        {
            get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stdin)); }
        }

        /// <summary>
        /// Gets a value indicating whether errors are redirected.
        /// </summary>
        /// <value>
        /// <c>true</c> if errors are redirected; otherwise, <c>false</c>.
        /// </value>
        public static bool IsErrorRedirected
        {
            get { return FileType.Char != GetFileType(GetStdHandle(StdHandle.Stderr)); }
        }

        // P/Invoke:
        private enum FileType { Unknown, Disk, Char, Pipe };
        private enum StdHandle { Stdin = -10, Stdout = -11, Stderr = -12 };
        [DllImport("kernel32.dll")]
        private static extern FileType GetFileType(IntPtr hdl);
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(StdHandle std);
    }
}