using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Packager.Services;
using System.IO;
using System.Windows.Forms;

namespace NuGetContentPackagerConsole
{
    class Program
    {
        static string _contentFileNameTemplate = "{0}.nupp";

        static void Main(string[] args)
        {
            if (args != null && args.Count() > 1)
            {
                string selectedFileName = args[0];

                FileInfo fileInfo = new FileInfo(selectedFileName);

                if (fileInfo.Exists)
                {
                    string fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                    string ccontentFileName = string.Format(_contentFileNameTemplate, fileName);

                    TreeNode _contentNode = new TreeNode("Content");
                    string ns = PackageService.LoadFile(selectedFileName, _contentFileNameTemplate, _contentNode);


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
