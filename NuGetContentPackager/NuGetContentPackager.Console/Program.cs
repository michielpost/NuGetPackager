using System;
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
          //var fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);
          //string ccontentFileName = string.Format(_contentFileNameTemplate, fileName);

          var contentNode = new TreeNode("Content");
          var ns = PackageService.LoadFile(selectedFileName, ContentFileNameTemplate, contentNode);

          if (fileInfo.Directory != null)
          {

            if (!ConsoleExtensions.IsOutputRedirected)
              System.Console.CursorVisible = false; //Do not modify cursor visibility in Visual Studio build

            PackageService.ExportFiles(args[1], ns, contentNode, fileInfo.Directory.FullName, true);

            if (!ConsoleExtensions.IsOutputRedirected)
              System.Console.CursorVisible = true; //Do not modify cursor visibility in Visual Studio build
          }
          else
          {
            System.Console.Error.WriteLine("{0} does not exist in a directory", fileInfo.FullName);
            Environment.ExitCode = 1; //Indicates error
          }
        }
        else
        {
          System.Console.Error.WriteLine("{0} does not exist", fileInfo.FullName);
          Environment.ExitCode = 1; //Indicates error
        }
      }
      else
      {
        System.Console.Error.WriteLine("Input nupp file as source and nupkg or nuspec file as target.");
        System.Console.Error.WriteLine("NuGetContentPackager.Console.exe {nupp} {nuspec}");
        Environment.ExitCode = 1; //Indicates error
      }


      //Console.ReadLine();
    }
  }
}
