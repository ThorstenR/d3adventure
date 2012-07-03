using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using D3_Adventures;
using D3_Adventures.Structures;
using D3_Adventures.Enumerations;
using D3_Adventures.Memory_Handling;
using Utilities;

namespace A_Simple_Display
{
    public partial class Form1 : Form
    {
        private Actor[] actors, items, monsters;
        private MemoryManager mem = D3_Adventures.Globals.mem;
        private TextWriter consoleLog;

        public Form1()
        {
            InitializeComponent();
            consoleLog = new TextBoxStreamWriter(richTextBoxConsole);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Globals.mem.Attach();
            display();
            Console.SetOut(consoleLog);

            toolStripComboBoxAttributes.Items.AddRange(ActorAttributes._names);
        }

        private void display()
        {
            TreeNode tn;

            actors = Data.IterateActors();
            items = Data.GetItems();
            monsters = Data.GetMonsters();

            treeViewActors.Nodes.Clear();
            treeViewItems.Nodes.Clear();
            treeViewMonsters.Nodes.Clear();

            foreach (Actor a in actors)
            {
                tn = new TreeNode("TEST");
                Dictionary<string, string> fields = a.Fields();
                TreeNode[] nodes = new TreeNode[fields.Count];
                int i = 0;

                foreach (KeyValuePair<string, string> f in fields)
                {
                    nodes[i] = new TreeNode(f.Key + " = " + f.Value);
                    i++;
                }
                treeViewActors.Nodes.Add(new TreeNode(a.name, nodes));
            }

            foreach (Actor a in items)
            {
                tn = new TreeNode(a.name, new TreeNode[]
                {
                    new TreeNode("ID ACD: " + a.id_acd.ToString("X")), 
                    new TreeNode("ID Actor: " + a.id_actor.ToString("X")),
                    new TreeNode("ID SNO: " + a.id_sno.ToString("X")),
                    new TreeNode("World: "+a.guid_world.ToString("X")),
                    new TreeNode("Dist From Me: " + a.distanceFromMe),
                    new TreeNode("POS: "+a.Pos.ToString()),
                    new TreeNode("POS1: "+a.Pos1.ToString()),
                    new TreeNode("POS2: "+a.Pos2.ToString()),
                    new TreeNode("POS3: "+a.Pos3.ToString()),
                    new TreeNode("POS4: "+a.Pos4.ToString()),
                    new TreeNode("POS6: "+a.Pos6.ToString()),
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
                    new TreeNode("World: "+a.guid_world.ToString("X")),
                    new TreeNode("Dist From Me: " + a.distanceFromMe),
                    new TreeNode("POS: "+a.Pos.ToString()),
                    new TreeNode("POS1: "+a.Pos1.ToString()),
                    new TreeNode("POS2: "+a.Pos2.ToString()),
                    new TreeNode("POS3: "+a.Pos3.ToString()),
                    new TreeNode("POS4: "+a.Pos4.ToString()),
                    new TreeNode("POS6: "+a.Pos6.ToString()),
                    new TreeNode("Data1: " + a.unknown_data1.ToString("X")),
                    new TreeNode("Data2: " + a.unknown_data2.ToString("X")),
                    new TreeNode("Data3: " + a.unknown_data3.ToString("X")),
                    //new TreeNode("Life Percentage?: " + a.unknown_healthPercent)
                });

                treeViewMonsters.Nodes.Add(tn);
            }

            ActorCommonData[] acds = Data.IterateACD();
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
                    new TreeNode("Z: " + acd.PosWorld.z),
                    new TreeNode("ItemLocation: "+ acd.itemLoc.ToString("X"))
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


            //hacking in scenese for now
            /*
            var scenes = SceneManager.getScenes();

            tn = new TreeNode("!!SCENE HACK TEMP!!");
             treeViewActors.Nodes.Add(tn);

            foreach (var s in scenes)
            {

                var d = s.SceneInfo.NavZone;

                tn = new TreeNode(s.SceneId.ToString("X"), new TreeNode[]
                {
                    new TreeNode("Scene SNO: " + s.SceneSNO), 
                    new TreeNode("Scene X: " + s.SquareCountX), 
                    new TreeNode("Scene Y: " + s.SquareCountY), 
                    new TreeNode("LevelArea SNO: " + s.LevelAreaSNO), 
                    new TreeNode("WordID: " + s.DynamicWorldId.ToString("X")), 
                    new TreeNode("Position: " + s.Position), 
                    new TreeNode("MarkerSetBounds: " + s.MarkerSetBounds.ToString()), 
                });

                treeViewActors.Nodes.Add(tn);
            }
            */

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
            Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_Gizmo);
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
                        Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_Gizmo);
                    break;
                case 1:
                    actor = getObjectByName(getRootNode(treeViewItems.SelectedNode).Text);
                    Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_Gizmo);
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
                        Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_NPC);
                    break;
                case 1:
                    MessageBox.Show("Too Many Bad Dead Bodies");
                    break;
                case 2:
                    actor = getObjectByName(getRootNode(treeViewMonsters.SelectedNode).Text);
                    Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Axe_Operate_NPC);
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
                    MessageBox.Show(actor.Exists().ToString());
                    break;
                case 1:
                    actor = getObjectByName(getRootNode(treeViewItems.SelectedNode).Text);
                    MessageBox.Show(actor.Exists().ToString());
                    break;
                case 2:
                    actor = getObjectByName(getRootNode(treeViewMonsters.SelectedNode).Text);
                    MessageBox.Show(actor.Exists().ToString());
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
                    Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Wizard_ArcaneOrb);
                    break;
                case 1:
                    actor = getObjectByName(getRootNode(treeViewItems.SelectedNode).Text);
                    Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Wizard_ArcaneOrb);
                    break;
                case 2:
                    actor = getObjectByName(getRootNode(treeViewMonsters.SelectedNode).Text);
                    Actions.PowerUseGUID(actor.id_acd, SNO.SNOPowerId.Wizard_ArcaneOrb);
                    break;
            }
        }

        private void treeViewActors_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            float testA = Data.GetAttributeIRC<float>(ActorAttributes.Intelligence_Total); // irc (??)
            stopwatch.Stop();
            consoleLog.WriteLine("IRC: "+stopwatch.Elapsed);
            stopwatch.Reset();

            stopwatch.Start();
            float testB = Data.GetAttributeAU3<float>(0x77BC0000, ActorAttributes.Intelligence_Total); // AU3 Owned's
            stopwatch.Stop();
            consoleLog.WriteLine("AU3: " + stopwatch.Elapsed);
            stopwatch.Reset();

            stopwatch.Start();
            float testC = Data.GetAttributeShadwd<float>(Globals.Me.FAG, ActorAttributes.Intelligence_Total.offset); // shadwd
            stopwatch.Stop();
            consoleLog.WriteLine("Sh: " + stopwatch.Elapsed);
            stopwatch.Reset();


            stopwatch.Start();
            float testD = Data.GetAttributeIRC<float>(ActorAttributes.Hitpoints_Cur); // irc (??)
            stopwatch.Stop();
            consoleLog.WriteLine("IRC: " + stopwatch.Elapsed);
            stopwatch.Reset();

            stopwatch.Start();
            float testE = Data.GetAttributeAU3<float>(0x77BC0000, ActorAttributes.Hitpoints_Cur); // AU3 Owned's
            stopwatch.Stop();
            consoleLog.WriteLine("AU3: " + stopwatch.Elapsed);
            stopwatch.Reset();

            stopwatch.Start();
            float testF = Data.GetAttributeShadwd<float>(Globals.Me.FAG, ActorAttributes.Hitpoints_Cur.offset); // shadwd
            stopwatch.Stop();
            consoleLog.WriteLine("SH: " + stopwatch.Elapsed);
            stopwatch.Reset();

            int test2;
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
            if (!_output.IsDisposed)
                _output.AppendText(value.ToString()); // When character data is written, append it to the text box.
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }

}
