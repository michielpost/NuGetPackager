namespace NuGetContentPackager
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.exportButton = new System.Windows.Forms.Button();
			this.namespaceTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.saveFileDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			// 
			// treeView1
			// 
			this.treeView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.treeView1.CheckBoxes = true;
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(12, 97);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(601, 497);
			this.treeView1.TabIndex = 0;
			this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
			this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.Filter = ".csproj |*.csproj|.vbproj|*.vbproj|.nupp|*.nupp";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(12, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(105, 47);
			this.button1.TabIndex = 1;
			this.button1.Text = "Open project file";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.openButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(141, 12);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(87, 47);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save project as XML";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// exportButton
			// 
			this.exportButton.Location = new System.Drawing.Point(250, 12);
			this.exportButton.Name = "exportButton";
			this.exportButton.Size = new System.Drawing.Size(147, 47);
			this.exportButton.TabIndex = 3;
			this.exportButton.Text = "Process and copy to NuGet directory";
			this.exportButton.UseVisualStyleBackColor = true;
			this.exportButton.Click += new System.EventHandler(this.exportButton_Click);
			// 
			// namespaceTextBox
			// 
			this.namespaceTextBox.Location = new System.Drawing.Point(109, 71);
			this.namespaceTextBox.Name = "namespaceTextBox";
			this.namespaceTextBox.Size = new System.Drawing.Size(237, 20);
			this.namespaceTextBox.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 74);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(91, 13);
			this.label1.TabIndex = 5;
			this.label1.Text = "Root namespace:";
			// 
			// saveFileDialog2
			// 
			this.saveFileDialog2.Filter = "NuGet File |*.nupkg; *.nuspec";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(625, 606);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.namespaceTextBox);
			this.Controls.Add(this.exportButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.treeView1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "NuGet Source Code Packager - Create PP files for NuGet";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button exportButton;
        private System.Windows.Forms.TextBox namespaceTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog saveFileDialog2;
    }
}

