using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using D3Bloader.Scripting;
using D3Bloader.Game.Objects;

using D3_Adventures;
using D3_Adventures.Structures;
using D3_Adventures.Enumerations;

namespace D3Bloader.Game
{
    // ScriptArena Class
    /// Exposes the arena methodology to scripting
    ///////////////////////////////////////////////////////
    public partial class ScriptBot : Bot
    {
        ///////////////////////////////////////////////////
        // Constant Data
        ///////////////////////////////////////////////////
        private const string BT_EVENT_KEY = "BT.Build";
        private const string ATTACK_EVENT_KEY = "Toon.AttackEnemy";

        // Member variables
        ///////////////////////////////////////////////////
        private List<Scripts.IScript> _scripts;		//The scripts we're currently supporting
        private string _scriptType;					//The type of scripts we're instancing

        ///////////////////////////////////////////////////
        // Member Functions
        ///////////////////////////////////////////////////
        /// <summary>
        /// Generic constructor
        /// </summary>
        public ScriptBot(Bot bot, string scriptType)
            : base()
        {
            _scriptType = scriptType;
        }

        /// <summary>
        /// Initializes scripts
        /// </summary>
        public override bool init()
        {
            EventObjects.eventInit(this, true);

            //Load the associated scripts
            _scripts = Scripts.instanceScripts(this, _scriptType);


            //If the bot implementes a BT, load it and use it.
            _BehaviorTrees = new List<TreeSharp.Composite>();
            if (exists(BT_EVENT_KEY))
            {
                //Grab all handlers (could be more then one BT).
                foreach (var mthHandler in events[BT_EVENT_KEY].methods)
                {
                    TreeSharp.Composite x = mthHandler.handler(mthHandler.that, null) as TreeSharp.Composite;
                    x.Start(null);
                    _BehaviorTrees.Add(x);
                }
            }


            return true;

        }

        /// <summary>
        /// Allows the bot to keep it's state up-to-date
        /// </summary>
        public override bool poll()
        {
            base.poll();

            //Poll all scripts!
            foreach (Scripts.IScript script in _scripts)
                script.poll();

            //run all the BTs.
            foreach (var bt in _BehaviorTrees)
            {
                try
                {
                    if (bt.Tick(null) != TreeSharp.RunStatus.Running)
                        bt.Start(null);
                }
                catch
                {
                    bt.Stop(null);
                    bt.Start(null);
                    throw;
                }
            }


            return true;
        }

        /// <summary>
        /// Simple Item Pickup
        /// </summary>
        public override void pickItem(Data.gameObject itm)
        {
        }

        public override void Attack(Actor who)
        {
            Log.write("ATTACKING {0}", who.name);
            isAttacking = true;
            if (exists(ATTACK_EVENT_KEY))
            {
                //Grab all handlers (could be more then one BT).
                foreach (var mthHandler in events[ATTACK_EVENT_KEY].methods)
                {
                    mthHandler.handler(mthHandler.that, new object[] { who });
                }
            }
        }
    }
}