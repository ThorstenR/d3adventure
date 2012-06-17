using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using D3_Adventures;
using Utilities.GlobalKeyboardHook;

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

            MessageBox.Show(Offsets.myToon.ToString("X"));
            MessageBox.Show(Offsets.ACDCount.ToString());

            GKH gkh = new GKH();
            //gkh.Hook().ToString();
            gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
        }

        static void gkh_KeyDown(object sender, KeyEventArgs e)
        {
            
            //MessageBox.Show(e.KeyCode.ToString());

            Data.Vec3 pos = Data.getCurrentPos();
            /*
            if (e.KeyCode == Keys.W)
                Actions.moveToPos(pos.x, pos.y + 5, pos.z);
            if (e.KeyCode == Keys.S)
                Actions.moveToPos(pos.x, pos.y - 5, pos.z);
            if (e.KeyCode == Keys.A)
                Actions.moveToPos(pos.x - 5, pos.y, pos.z);
            if (e.KeyCode == Keys.D)
                Actions.moveToPos(pos.x + 5, pos.y, pos.z);
             */
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.S || e.KeyCode == Keys.A || e.KeyCode == Keys.D)
                e.SuppressKeyPress = true;

            if (e.KeyCode == Keys.W)
                Actions.moveToPos(pos.x - 5, pos.y - 5, pos.z);
            if (e.KeyCode == Keys.S)
                Actions.moveToPos(pos.x + 5, pos.y + 5, pos.z);
            if (e.KeyCode == Keys.A)
                Actions.moveToPos(pos.x + 5, pos.y - 5, pos.z);
            if (e.KeyCode == Keys.D)
                Actions.moveToPos(pos.x - 5, pos.y + 5, pos.z);

        }

        private void display()
        {
            foreach (Data.gameObject o in objs)
            {
                //if (o.data == 2 && o.data2 == -1) // just items 
                //if (o.data2 == 29944) // monsters
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

        private TreeNode getRootNode(TreeNode node)
        {
            TreeNode n = node;
            while (n.Parent != null)
            {
                n = n.Parent;
            }
            return n;
        }

        private Data.gameObject getObjectByName(string name)
        {
            return objs.Where(o => o.name == name).FirstOrDefault();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(getRootNode(treeView1.SelectedNode).Text);
            Data.gameObject obj = getObjectByName(getRootNode(treeView1.SelectedNode).Text);
            Actions.interactGUID(obj.guid, 0x7545);
        }

    }
}
