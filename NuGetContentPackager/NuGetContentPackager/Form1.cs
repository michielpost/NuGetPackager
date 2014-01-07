using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using Packager.Services;

namespace NuGetContentPackager
{
    /// <summary>
    /// Main form for editing or creating .nupp files.
    /// Also initiates copying of source files.
    /// </summary>
    public partial class Form1 : Form
    {
        TreeNode _contentNode = new TreeNode("Content");
        DirectoryInfo _dirInfo;
        private const string ContentFileNameTemplate = "{0}.nupp";
        string _contentFileName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// When any arguments are specified, the first is regarded to be the project file.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public Form1(string[] args)
        {
            InitializeComponent();

            if (args != null && args.Any())
            {
                OpenFile(new Uri(args[0]).AbsolutePath);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (var tnode in e.Node.Nodes.OfType<TreeNode>())
            {
                tnode.Checked = e.Node.Checked;
            }
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var selectedFileName = openFileDialog1.FileName;

                OpenFile(selectedFileName);

            }
        }

        private void OpenFile(string selectedFileName)
        {
            var fileInfo = new FileInfo(selectedFileName);

            if (fileInfo.Exists)
            {
                var fileName = Path.GetFileNameWithoutExtension(fileInfo.FullName);

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
            var paths = new List<string>();

            PackageService.FillPaths(paths, _contentNode);

            var xdoc = new XDocument();
            var nugetpp = new XElement("nugetpp");

            var files = new XElement("files");
            foreach (var path in paths)
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
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                var fileName = saveFileDialog2.FileName;

                var saveInfo = new FileInfo(fileName);
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
