using System;
using System.Windows.Forms;

namespace NuGetContentPackager
{
    /// <summary>
    /// Initiates the startup of <see cref="Form1"/>
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData));
        }
        
    }
}
