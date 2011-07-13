using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;

namespace NuGetContentPackager
{
    public partial class Form1 : Form
    {
        TreeNode _contentNode = new TreeNode("Content");
        DirectoryInfo _dirInfo;
        string _contentFileNameTemplate = "{0}.nupp";
        string _contentFileName = string.Empty;

        XDocument _xdoc;


        public Form1(string[] args)
        {
            InitializeComponent();

            if (args.Count() > 0)
            {
                LoadFile(args[0]);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void PopulateContentTreeView(TreeNode parent, DirectoryInfo directory)
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

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
           
                foreach (var node in e.Node.Nodes)
                {
                    TreeNode tnode = node as TreeNode;

                    if (tnode != null)
                        tnode.Checked = e.Node.Checked;
                }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                LoadFile(selectedFileName);

            }
        }

        private void LoadFile(string selectedFileName)
        {
            FileInfo fileInfo = new FileInfo(selectedFileName);
            _dirInfo = fileInfo.Directory;
            saveFileDialog2.InitialDirectory = _dirInfo.FullName;


            string fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);

            namespaceTextBox.Text = fileName;
            _contentFileName = string.Format(_contentFileNameTemplate, fileName);

            string filePath = Path.Combine(_dirInfo.FullName, _contentFileName);
            if (new FileInfo(filePath).Exists)
            {
                _xdoc = XDocument.Load(filePath);

                namespaceTextBox.Text = _xdoc.Element("nugetpp").Element("namespace").Value;
            }

            treeView1.Nodes.Add(_contentNode);

            PopulateContentTreeView(_contentNode, _dirInfo);
            _contentNode.Expand();

            CheckNodes(_xdoc, _contentNode);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> paths = new List<string>();

            FillPaths(paths, _contentNode);

            XDocument xdoc = new XDocument();
            XElement nugetpp = new XElement("nugetpp");

            XElement files = new XElement("files");
            foreach (string path in paths)
            {
                files.Add(new XElement("file", path));
            }

            xdoc.Add(nugetpp);

            nugetpp.Add(files);
            nugetpp.Add(new XElement("namespace", namespaceTextBox.Text));

            xdoc.Save(Path.Combine(_dirInfo.FullName, _contentFileName));


        }

        private void CheckNodes(XDocument xdoc, TreeNode contentNode)
        {
            if (xdoc != null && xdoc.Element("nugetpp").Element("files").Elements("file").Where(x => x.Value == this.FullPath(contentNode, false)).Any())
            {
                contentNode.Checked = true;

                if(contentNode.Parent != null)
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

        private void FillPaths(List<string> paths, TreeNode contentNode)
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

        private string FullPath(TreeNode node, bool includeParent)
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FileInfo saveInfo = new FileInfo(saveFileDialog2.FileName);
                saveFileDialog2.InitialDirectory = saveInfo.DirectoryName;
                string targetDir = saveInfo.DirectoryName;

                List<string> paths = new List<string>();

                FillPaths(paths, _contentNode);

                foreach (string filePath in paths)
                {
                    FileInfo fileInfo = new FileInfo(Path.Combine(_dirInfo.FullName, filePath.Substring(1)));

                    if (fileInfo.Exists)
                    {
                        ProcessFile(fileInfo, filePath, targetDir);
                    }
                }
            }
        }

        private void ProcessFile(FileInfo fileInfo, string filePath, string targerPath)
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

                string modified = orignal.Replace(namespaceTextBox.Text, "$rootnamespace$");

                File.WriteAllText(copy.FullName, modified);
            }




        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Parent != null)
            {
                e.Node.Parent.Expand();
            }
        }

       
    }
}
