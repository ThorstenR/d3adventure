using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using D3_Adventures;
using D3_Adventures.Structures;
using D3_Adventures.Enumerations;
using Utilities.MemoryHandling;
using System.IO;

namespace A_Simple_Display
{
    public partial class Form1 : Form
    {
        private Data.gameObject[] objs;
        private MemoryManager mem = D3_Adventures.Program.mem;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            display();
        }

        private void display()
        {
            TreeNode tn;

            objs = Data.iterateObjectList();

            treeViewObjects.Nodes.Clear();
            treeViewItems.Nodes.Clear();
            treeViewMonsters.Nodes.Clear();

            foreach (Data.gameObject o in objs)
            {
                //if (o.data == 2 && o.data2 == -1) // just items 
                //if (o.data2 == 29944) // monsters
                tn = new TreeNode(o.name, new TreeNode[]
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
                treeViewObjects.Nodes.Add(tn);

                if (o.data == 2 && o.data2 == -1) // just items 
                {
                    tn = new TreeNode(o.name, new TreeNode[]
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
                    treeViewItems.Nodes.Add(tn);
                }

                if (o.data2 == 29944) // monsters
                {
                    tn = new TreeNode(o.name, new TreeNode[]
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
                    treeViewMonsters.Nodes.Add(tn);
                }
            }

            ActorCommonData[] acds = Data.iterateACD();
            treeViewACD.Nodes.Clear();

            foreach (ActorCommonData acd in acds)
            {
                tn = new TreeNode(acd.name, new TreeNode[]
                {
                    // Todo Serialize/display
                });
                treeViewACD.Nodes.Add(tn);
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
            Data.gameObject obj = getObjectByName(getRootNode(treeViewObjects.SelectedNode).Text);
            Actions.interactGUID(obj.guid, SNO.SNOPowerId.Axe_Operate_Gizmo);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            display();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Data.gameObject obj;
            switch (tabControlDisplay.SelectedIndex)
            {
                case 0:
                    obj = getObjectByName(getRootNode(treeViewObjects.SelectedNode).Text);
                    if (obj.data == 2 && obj.data2 == -1)
                        Actions.interactGUID(obj.guid, SNO.SNOPowerId.Axe_Operate_Gizmo);
                    break;
                case 1:
                    obj = getObjectByName(getRootNode(treeViewItems.SelectedNode).Text);
                    Actions.interactGUID(obj.guid, SNO.SNOPowerId.Axe_Operate_Gizmo);
                    break;
                case 2:
                    MessageBox.Show("Too Many Bad Dead Bodies");
                    break;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Data.gameObject obj;
            switch (tabControlDisplay.SelectedIndex)
            {
                case 0:
                    obj = getObjectByName(getRootNode(treeViewObjects.SelectedNode).Text);
                    if (obj.data2 == 29944)
                        Actions.interactGUID(obj.guid, SNO.SNOPowerId.Axe_Operate_NPC);
                    break;
                case 1:
                    MessageBox.Show("Too Many Bad Dead Bodies");
                    break;
                case 2:
                    obj = getObjectByName(getRootNode(treeViewMonsters.SelectedNode).Text);
                    Actions.interactGUID(obj.guid, SNO.SNOPowerId.Axe_Operate_NPC);
                    break;
            }
        }

    }
}
