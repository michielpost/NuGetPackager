using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Packager.Services
{
    public class PackageService
    {
        
        public static string LoadFile(string selectedFileName, string contentFileNameTemplate, TreeNode contentNode)
        {
            FileInfo fileInfo = new FileInfo(selectedFileName);

            string ns = string.Empty;

            if (fileInfo.Exists)
            {
                XDocument xdoc = new XDocument();

                string fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);



                string filePath = Path.Combine(fileInfo.Directory.FullName, string.Format(contentFileNameTemplate, fileName));
                if (new FileInfo(filePath).Exists)
                {
                    xdoc = XDocument.Load(filePath);

                    ns = xdoc.Element("nugetpp").Element("namespace").Value;
                }


                PopulateContentTreeView(contentNode, fileInfo.Directory);

                CheckNodes(xdoc, contentNode);
            }

            return ns;
        }

        public static void FillPaths(List<string> paths, TreeNode contentNode)
        {
            if (contentNode.Checked)
            {
                paths.Add(FullPath(contentNode, false));
            }

            foreach (var n in contentNode.Nodes)
            {
                TreeNode node = n as TreeNode;
                FillPaths(paths, node);
            }
        }


        public static void ExportFiles(string fileName, string ns, TreeNode contentNode, string originalDir)
        {
            FileInfo saveInfo = new FileInfo(fileName);

            if (saveInfo.Exists)
            {
                string targetDir = saveInfo.DirectoryName;

                List<string> paths = new List<string>();

                FillPaths(paths, contentNode);

                foreach (string filePath in paths)
                {
                    FileInfo fileInfo = new FileInfo(Path.Combine(originalDir, filePath.Substring(1)));

                    if (fileInfo.Exists)
                    {
                        ProcessFile(fileInfo, filePath, targetDir, ns);

                        Console.WriteLine("Export finished.");

                    }
                }


            }
            else
            {
                Console.WriteLine("NuGet package file does not exist");
            }
        }
        
        
        private static void PopulateContentTreeView(TreeNode parent, DirectoryInfo directory)
        {
            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                if (dir.Name.ToLower() != "bin"
                    && dir.Name.ToLower() != "obj")
                {
                    TreeNode node = new TreeNode(dir.Name);

                    node.Tag = dir;
                    PopulateContentTreeView(node, dir);

                    if (node.Nodes.Count > 0)
                        parent.Nodes.Add(node);
                }

            }

            foreach (FileInfo f in directory.GetFiles())
            {
                TreeNode n = new TreeNode(f.Name);

                if (!f.Name.ToLower().EndsWith(".dll"))
                {

                    parent.Nodes.Add(n);

                }
                //(n.Tag as NugetPackageFile).PackageFile = n.FullPath;
            }
        }
        
        private static void CheckNodes(XDocument xdoc, TreeNode contentNode)
        {
            if (xdoc != null && xdoc.Element("nugetpp").Element("files").Elements("file").Where(x => x.Value == FullPath(contentNode, false)).Any())
            {
                contentNode.Checked = true;

                if (contentNode.Parent != null)
                    contentNode.Parent.Expand();


            }
            else
            {
            }

            foreach (var n in contentNode.Nodes)
            {
                TreeNode node = n as TreeNode;
                CheckNodes(xdoc, node);
            }
        }

        private static string FullPath(TreeNode node, bool includeParent)
        {
            if (node.Parent != null)
            {
                return FullPath(node.Parent, includeParent) + "\\" + node.Text;
            }
            else
            {
                if (includeParent)
                    return "\\" + node.Text;
                else
                    return string.Empty;
            }
        }

        private static void ProcessFile(FileInfo fileInfo, string filePath, string targerPath, string ns)
        {
            targerPath = Path.Combine(targerPath, "content");

            string directory = new FileInfo(Path.Combine(targerPath, filePath.Substring(1) + ".pp")).Directory.FullName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            FileInfo copy = fileInfo.CopyTo(Path.Combine(targerPath, filePath.Substring(1) + ".pp"), true);

            //Process code files
            if (fileInfo.Extension == ".cs"
                || fileInfo.Extension == ".vb"
                || fileInfo.Extension == ".aspx"
                || fileInfo.Extension == ".cshtml"
                || fileInfo.Extension == ".vbhtml")
            {

                string orignal = File.ReadAllText(copy.FullName);

                string modified = orignal.Replace(ns, "$rootnamespace$");

                File.WriteAllText(copy.FullName, modified);
            }




        }
        
       

       
    }
}
