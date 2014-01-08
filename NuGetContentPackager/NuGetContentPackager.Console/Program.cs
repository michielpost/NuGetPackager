using System.IO;
using System.Linq;
using System.Windows.Forms;
using NuGetContentPackagerConsole;
using Packager.Services;

namespace NuGetContentPackager.Console
{
    /// <summary>
    /// Console application for copying input files to a nuget package directory based on a .nupp configuration file.
    /// </summary>
    class Program
    {
        private const string ContentFileNameTemplate = "{0}.nupp";

        static void Main(string[] args)
        {
            if (args != null && args.Count() > 1)
            {
                var selectedFileName = args[0];

                var fileInfo = new FileInfo(selectedFileName);

                if (fileInfo.Exists)
                {
                    var fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                    //string ccontentFileName = string.Format(_contentFileNameTemplate, fileName);

                    var _contentNode = new TreeNode("Content");
                    var ns = PackageService.LoadFile(selectedFileName, ContentFileNameTemplate, _contentNode);

                    if (!ConsoleExtensions.IsOutputRedirected) System.Console.CursorVisible = false;

                    PackageService.ExportFiles(args[1], ns, _contentNode, fileInfo.Directory.FullName);

                    if (!ConsoleExtensions.IsOutputRedirected) System.Console.CursorVisible = true;
                }
            }
            else if (!ConsoleExtensions.IsOutputRedirected)
            {
                System.Console.WriteLine("Input nupp file as source and nupkg or nuspec file as target.");
                System.Console.WriteLine("NuGetContentPackager.Console.exe {nupp} {nuspec}");
            }


            //Console.ReadLine();
        }
    }
}
