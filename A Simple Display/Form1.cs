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
        private Actor[] actors, items, monsters;
        private MemoryManager mem = D3_Adventures.Program.mem;
        private TextWriter consoleLog;

        public Form1()
        {
            InitializeComponent();
            consoleLog = new TextBoxStreamWriter(richTextBoxConsole);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            display();
            Console.SetOut(consoleLog);
        }

        private void display()
        {
            TreeNode tn;

            actors = Data.IterateActors();
            items = Data.getItems();
            monsters = Data.getMonsters();

            treeViewActors.Nodes.Clear();
            treeViewItems.Nodes.Clear();
            treeViewMonsters.Nodes.Clear();

            foreach (Actor a in actors)
            {
                //if (o.data == 2 && o.data2 == -1) // just items 
                //if (o.data2 == 29944) // monsters
                tn = new TreeNode(a.name, new TreeNode[]
                {
                    new TreeNode("ID ACD: " + a.id_acd.ToString("X")), 
                    new TreeNode("ID Actor: " + a.id_actor.ToString("X")),
                    new TreeNode("ID SNO: " + a.id_sno.ToString("X")),
                    new TreeNode("World: "+a.guid_world.ToString("X")),
                    new TreeNode("Dist From Me: " + a.distanceFromMe),
                    new TreeNode("X: " + a.Pos.x),
                    new TreeNode("Y: " + a.Pos.y),
                    new TreeNode("Z: " + a.Pos.z),
                    new TreeNode("Data1: " + a.unknown_data1.ToString("X")),
                    new TreeNode("Data2: " + a.unknown_data2.ToString("X")),
                    new TreeNode("Data3: " + a.unknown_data3.ToString("X")),
                    new TreeNode("MemLocation: " + a.mem_location.ToString("X")),
                    new TreeNode("Life Percentage?: " + mem.ReadMemoryAsFloat(a.mem_location + 0x408)),
                    //new TreeNode("Life Percentage?: " + a.unknown_healthPercent)
                });
                treeViewActors.Nodes.Add(tn);
            }

            foreach (Actor a in items)
            {
                tn = new TreeNode(a.name, new TreeNode[]
                {
                    new TreeNode("ID ACD: " + a.id_acd.ToString("X")), 
                    new TreeNode("ID Actor: " + a.id_actor.ToString("X")),
                    new TreeNode("ID SNO: " + a.id_sno.ToString("X")),
                    new TreeNode("Dist From Me: " + a.distanceFromMe),
                    new TreeNode("X: " + a.Pos.x),
                    new TreeNode("Y: " + a.Pos.y),
                    new TreeNode("Z: " + a.Pos.z),
                    new TreeNode("Data1: " + a.unknown_data1.ToString("X")),
                    new TreeNode("Data2: " + a.unknown_data2.ToString("X")),
                    new TreeNode("Data3: " + a.unknown_data3.ToString("X")),
                    //new TreeNode("Life Percentage?: " + a.unknown_healthPercent)
                });
                treeViewItems.Nodes.Add(tn);
            }

            foreach (Actor a in monsters)
            {
                tn = new TreeNode(a.name, new TreeNode[]
                {
                    new TreeNode("ID ACD: " + a.id_acd.ToString("X")), 
                    new TreeNode("ID Actor: " + a.id_actor.ToString("X")),
                    new TreeNode("ID SNO: " + a.id_sno.ToString("X")),
                    new TreeNode("Dist From Me: " + a.distanceFromMe),
                    new TreeNode("X: " + a.Pos.x),
                    new TreeNode("Y: " + a.Pos.y),
                    new TreeNode("Z: " + a.Pos.z),
                    new TreeNode("Data1: " + a.unknown_data1.ToString("X")),
                    new TreeNode("Data2: " + a.unknown_data2.ToString("X")),
                    new TreeNode("Data3: " + a.unknown_data3.ToString("X")),
                    //new TreeNode("Life Percentage?: " + a.unknown_healthPercent)
                });
                treeViewMonsters.Nodes.Add(tn);
            }

            ActorCommonData[] acds = Data.iterateACD();
            treeViewACD.Nodes.Clear();

            foreach (ActorCommonData acd in acds)
            {
                tn = new TreeNode(acd.name, new TreeNode[]
                {
                    // Todo Serialize/display
                    new TreeNode("Owner: " + acd.id_owner.ToString("X")),
                    new TreeNode("SNO: " + acd.id_snow.ToString("X")),
                    new TreeNode("Attribute: " + acd.id_attrib.ToString("X")),
                    new TreeNode("X: " + acd.PosWorld.x),
                    new TreeNode("Y: " + acd.PosWorld.y),
                    new TreeNode("Z: " + acd.PosWorld.z)
                });
                treeViewACD.Nodes.Add(tn);

                if (acd.id_owner == Data.toonID)
                {
                    tn = new TreeNode(acd.name, new TreeNode[]
                    {
                        // Todo Serialize/display
                        new TreeNode("Owner: " + acd.id_owner.ToString("X")),
                        new TreeNode("SNO: " + acd.id_snow.ToString("X")),
                        new TreeNode("Attribute: " + acd.id_attrib.ToString("X")),
                        new TreeNode("X: " + acd.PosWorld.x),
                        new TreeNode("Y: " + acd.PosWorld.y),
                        new TreeNode("Z: " + acd.PosWorld.z)
                    });
                    treeViewMyItems.Nodes.Add(tn);
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

        private Actor getObjectByName(string name)
        {
            return actors.Where(o => o.name == name).FirstOrDefault();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(getRootNode(treeView1.SelectedNode).Text);
            Actor actor = getObjectByName(getRootNode(treeViewActors.SelectedNode).Text);
            Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_Gizmo);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            display();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Actor actor;
            switch (tabControlDisplay.SelectedIndex)
            {
                case 0:
                    actor = getObjectByName(getRootNode(treeViewActors.SelectedNode).Text);
                    if (actor.unknown_data1 == 2 && actor.unknown_data2 == -1)
                        Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_Gizmo);
                    break;
                case 1:
                    actor = getObjectByName(getRootNode(treeViewItems.SelectedNode).Text);
                    Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_Gizmo);
                    break;
                case 2:
                    MessageBox.Show("Too Many Bad Dead Bodies");
                    break;
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Actor actor;
            switch (tabControlDisplay.SelectedIndex)
            {
                case 0:
                    actor = getObjectByName(getRootNode(treeViewActors.SelectedNode).Text);
                    if (actor.unknown_data2 == 29944)
                        Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_NPC);
                    break;
                case 1:
                    MessageBox.Show("Too Many Bad Dead Bodies");
                    break;
                case 2:
                    actor = getObjectByName(getRootNode(treeViewMonsters.SelectedNode).Text);
                    Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_NPC);
                    break;
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Actor actor;
            switch (tabControlDisplay.SelectedIndex)
            {
                case 0:
                    actor = getObjectByName(getRootNode(treeViewActors.SelectedNode).Text);
                    MessageBox.Show(actor.isAlive().ToString());
                    break;
                case 1:
                    actor = getObjectByName(getRootNode(treeViewItems.SelectedNode).Text);
                    MessageBox.Show(actor.isAlive().ToString());
                    break;
                case 2:
                    actor = getObjectByName(getRootNode(treeViewMonsters.SelectedNode).Text);
                    MessageBox.Show(actor.isAlive().ToString());
                    break;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Actor actor;
            switch (tabControlDisplay.SelectedIndex)
            {
                case 0:
                    actor = getObjectByName(getRootNode(treeViewActors.SelectedNode).Text);
                    Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Wizard_ArcaneOrb);
                    break;
                case 1:
                    actor = getObjectByName(getRootNode(treeViewItems.SelectedNode).Text);
                    Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Wizard_ArcaneOrb);
                    break;
                case 2:
                    actor = getObjectByName(getRootNode(treeViewMonsters.SelectedNode).Text);
                    Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.Wizard_ArcaneOrb);
                    break;
            }
        }

    }

    // Thanks to: http://saezndaree.wordpress.com/2009/03/29/how-to-redirect-the-consoles-output-to-a-textbox-in-c/
    public class TextBoxStreamWriter : TextWriter
    {
        RichTextBox _output = null;

        public TextBoxStreamWriter(RichTextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            _output.AppendText(value.ToString()); // When character data is written, append it to the text box.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

}
