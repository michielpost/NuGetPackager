using System;
using System.Linq;
using Packager.Services;
using System.IO;
using System.Windows.Forms;

namespace NuGetContentPackagerConsole
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

                    PackageService.ExportFiles(args[1], ns, _contentNode, fileInfo.Directory.FullName);
                }
            }
            else
            {
                Console.WriteLine("Input nupp file as source and nupkg or nuspec file as target.");
                Console.WriteLine("NuGetContentPackager.Console.exe {nupp} {nuspec}");
            }

            Console.ReadLine();
        }
    }
}
