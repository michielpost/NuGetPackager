using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using Packager.Services;

namespace NuGetContentPackager
{
    public partial class Form1 : Form
    {
        TreeNode _contentNode = new TreeNode("Content");
        DirectoryInfo _dirInfo;
        private const string ContentFileNameTemplate = "{0}.nupp";
        string _contentFileName = string.Empty;

        public Form1(string[] args)
        {
            InitializeComponent();


            if (args != null && args.Count() > 0)
            {
                OpenFile(new Uri(args[0]).AbsolutePath);
            }
                       
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
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

        private void openButton_Click(object sender, EventArgs e)
        {

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;

                OpenFile(selectedFileName);

            }
        }

        private void OpenFile(string selectedFileName)
        {
            var fileInfo = new FileInfo(selectedFileName);

            if (fileInfo.Exists)
            {
                string fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);


                _dirInfo = fileInfo.Directory;
                saveFileDialog2.InitialDirectory = _dirInfo.FullName;
                namespaceTextBox.Text = fileName;
                _contentFileName = string.Format(ContentFileNameTemplate, fileName);
            }

            _contentNode = new TreeNode();
            namespaceTextBox.Text = PackageService.LoadFile(selectedFileName, ContentFileNameTemplate, _contentNode);
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(_contentNode);
            _contentNode.Expand();
        }

     
        private void saveButton_Click(object sender, EventArgs e)
        {
            List<string> paths = new List<string>();

            PackageService.FillPaths(paths, _contentNode);

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

            if (_dirInfo != null)
            {

                xdoc.Save(Path.Combine(_dirInfo.FullName, _contentFileName));
            }

        }

       

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog2.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = saveFileDialog2.FileName;

                FileInfo saveInfo = new FileInfo(fileName);
                saveFileDialog2.InitialDirectory = saveInfo.DirectoryName;

                PackageService.ExportFiles(fileName, namespaceTextBox.Text, _contentNode, _dirInfo.FullName);
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
