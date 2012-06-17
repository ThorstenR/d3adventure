namespace A_Simple_Display
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.treeViewObjects = new System.Windows.Forms.TreeView();
            this.contextMenuStripTreeNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlDisplay = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.treeViewItems = new System.Windows.Forms.TreeView();
            this.treeViewMonsters = new System.Windows.Forms.TreeView();
            this.contextMenuStripTreeNode.SuspendLayout();
            this.tabControlDisplay.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewObjects
            // 
            this.treeViewObjects.ContextMenuStrip = this.contextMenuStripTreeNode;
            this.treeViewObjects.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewObjects.Location = new System.Drawing.Point(3, 3);
            this.treeViewObjects.Name = "treeViewObjects";
            this.treeViewObjects.Size = new System.Drawing.Size(615, 360);
            this.treeViewObjects.TabIndex = 0;
            // 
            // contextMenuStripTreeNode
            // 
            this.contextMenuStripTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStripTreeNode.Name = "contextMenuStripTreeNode";
            this.contextMenuStripTreeNode.Size = new System.Drawing.Size(120, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(119, 22);
            this.toolStripMenuItem1.Text = "Get Item";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // tabControlDisplay
            // 
            this.tabControlDisplay.Controls.Add(this.tabPage1);
            this.tabControlDisplay.Controls.Add(this.tabPage2);
            this.tabControlDisplay.Controls.Add(this.tabPage3);
            this.tabControlDisplay.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControlDisplay.Location = new System.Drawing.Point(0, 27);
            this.tabControlDisplay.Name = "tabControlDisplay";
            this.tabControlDisplay.SelectedIndex = 0;
            this.tabControlDisplay.Size = new System.Drawing.Size(629, 392);
            this.tabControlDisplay.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.treeViewObjects);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(621, 366);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Objects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.treeViewItems);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(621, 366);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Items";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripSeparator2,
            this.toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(629, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(50, 22);
            this.toolStripButton1.Text = "Refresh";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(107, 22);
            this.toolStripButton2.Text = "Pick Selected Item";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(106, 22);
            this.toolStripButton3.Text = "Interact With NPC";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.treeViewMonsters);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(621, 366);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Monsters";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // treeViewItems
            // 
            this.treeViewItems.ContextMenuStrip = this.contextMenuStripTreeNode;
            this.treeViewItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewItems.Location = new System.Drawing.Point(3, 3);
            this.treeViewItems.Name = "treeViewItems";
            this.treeViewItems.Size = new System.Drawing.Size(615, 360);
            this.treeViewItems.TabIndex = 1;
            // 
            // treeViewMonsters
            // 
            this.treeViewMonsters.ContextMenuStrip = this.contextMenuStripTreeNode;
            this.treeViewMonsters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewMonsters.Location = new System.Drawing.Point(3, 3);
            this.treeViewMonsters.Name = "treeViewMonsters";
            this.treeViewMonsters.Size = new System.Drawing.Size(615, 360);
            this.treeViewMonsters.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 419);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabControlDisplay);
            this.Name = "Form1";
            this.Text = "A Simple Display";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStripTreeNode.ResumeLayout(false);
            this.tabControlDisplay.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewObjects;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTreeNode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.TabControl tabControlDisplay;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.TreeView treeViewItems;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TreeView treeViewMonsters;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}

