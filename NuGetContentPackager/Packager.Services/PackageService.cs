﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Packager.Services
{
    /// <summary>
    /// The main engine for copying source files to the NuGet target directory.
    /// Also create .nupp project files
    /// </summary>
    public class PackageService
    {

        /// <summary>
        /// Loads a project file and populates the content tree.
        /// </summary>
        /// <param name="selectedFileName">Name of the selected file.</param>
        /// <param name="contentFileNameTemplate">The content file name template.</param>
        /// <param name="contentNode">The content node.</param>
        /// <returns></returns>
        public static string LoadFile(string selectedFileName, string contentFileNameTemplate, TreeNode contentNode)
        {
            var fileInfo = new FileInfo(selectedFileName);

            var ns = string.Empty;

            if (!fileInfo.Exists) return ns;

            var xdoc = new XDocument();

            var fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);

            var filePath = Path.Combine(fileInfo.Directory.FullName, string.Format(contentFileNameTemplate, fileName));
            if (new FileInfo(filePath).Exists)
            {
                xdoc = XDocument.Load(filePath);

                var nugetppRootElement = xdoc.Element("nugetpp");

                if (nugetppRootElement == null)
                {
                    System.Diagnostics.Trace.TraceWarning("NugetPP file '{0}' did not contain root node 'nugetpp'.",
                      filePath);
                }
                else
                {
                    ns = nugetppRootElement.Element("namespace").Value;
                }
            }


            PopulateContentTreeView(contentNode, fileInfo.Directory);

            CheckNodes(xdoc, contentNode);

            return ns;
        }

        /// <summary>
        /// Fills the paths. To be documented.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <param name="contentNode">The content node.</param>
        public static void FillPaths(List<string> paths, TreeNode contentNode)
        {
            if (contentNode.Checked)
            {
                paths.Add(FullPath(contentNode, false));
            }

            foreach (var n in contentNode.Nodes)
            {
                var node = n as TreeNode;
                FillPaths(paths, node);
            }
        }


        /// <summary>
        /// Exports the files to the NuGet content directory.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="ns">The namespace to replace while creating nuget pre-processor files (.pp).</param>
        /// <param name="contentNode">The content node.</param>
        /// <param name="originalDir">The original dir.</param>
        public static void ExportFiles(string fileName, string ns, TreeNode contentNode, string originalDir)
        {
            var saveInfo = new FileInfo(fileName);

            if (saveInfo.Exists)
            {
                var targetDir = saveInfo.DirectoryName;

                var paths = new List<string>();

                FillPaths(paths, contentNode);

                foreach (var filePath in paths)
                {
                    var fileInfo = new FileInfo(Path.Combine(originalDir, filePath.Substring(1)));

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
            foreach (var dir in directory.GetDirectories())
            {
                if (dir.Name.ToLower() != "bin"
                    && dir.Name.ToLower() != "obj")
                {
                    var node = new TreeNode(dir.Name)
                    {
                        Tag = dir
                    };

                    PopulateContentTreeView(node, dir);

                    if (node.Nodes.Count > 0)
                        parent.Nodes.Add(node);
                }

            }

            foreach (var f in directory.GetFiles())
            {
                var n = new TreeNode(f.Name);

                if (!f.Name.ToLower().EndsWith(".dll"))
                {

                    parent.Nodes.Add(n);

                }
                //(n.Tag as NugetPackageFile).PackageFile = n.FullPath;
            }
        }

        private static void CheckNodes(XDocument xdoc, TreeNode contentNode)
        {
            //to: Cleanup or convert to XPath
            if (xdoc != null
                && xdoc.Element("nugetpp") != null
                && xdoc.Element("nugetpp").Element("files") != null
                && xdoc.Element("nugetpp").Element("files").Elements("file") != null
                && xdoc.Element("nugetpp").Element("files").Elements("file").Any(x => x.Value == FullPath(contentNode, false)))
            {
                contentNode.Checked = true;

                if (contentNode.Parent != null)
                    contentNode.Parent.Expand();
            }

            foreach (var n in contentNode.Nodes)
            {
                var node = n as TreeNode;
                CheckNodes(xdoc, node);
            }
        }

        private static string FullPath(TreeNode node, bool includeParent)
        {
            if (node.Parent != null)
            {
                return FullPath(node.Parent, includeParent) + "\\" + node.Text;
            }

            if (includeParent)
            {
                return "\\" + node.Text;
            }

            return string.Empty;
        }

        private static void ProcessFile(FileInfo fileInfo, string filePath, string targetPath, string ns)
        {
            targetPath = Path.Combine(targetPath, "content");

            var directory = new FileInfo(Path.Combine(targetPath, filePath.Substring(1) + ".pp")).Directory.FullName;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var copy = fileInfo.CopyTo(Path.Combine(targetPath, filePath.Substring(1) + ".pp"), true);

            //Process code files
            switch (fileInfo.Extension)
            {
                case ".cs":
                case ".vb":
                case ".aspx":
                case ".cshtml":
                case ".vbhtml":

                    var orignal = File.ReadAllText(copy.FullName);
                    var modified = orignal.Replace(ns, "$rootnamespace$");

                    File.WriteAllText(copy.FullName, modified);

                    break;

                default:
                    break;
            }
        }
    }
}