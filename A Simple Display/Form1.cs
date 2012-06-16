using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using D3_Adventures;

namespace A_Simple_Display
{
    public partial class Form1 : Form
    {
        private Data.gameObject[] objs;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            objs = Data.iterateObjectList();
            display();
        }

        private void display()
        {
            foreach (Data.gameObject o in objs)
            {
                TreeNode tn = new TreeNode(o.name, new TreeNode[]
                {
                    new TreeNode("Guid: " + o.guid), 
                    new TreeNode("Dist From Me: " + o.distanceFromMe),
                    new TreeNode("X: " + o.position.x),
                    new TreeNode("Y: " + o.position.y),
                    new TreeNode("Z: " + o.position.z),
                    new TreeNode("Data1: " + o.data),
                    new TreeNode("Data2: " + o.data2),
                    new TreeNode("Data3: " + o.data3)
                });
                treeView1.Nodes.Add(tn);
            }
        }
    }
}
