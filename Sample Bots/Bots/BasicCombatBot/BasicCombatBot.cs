using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3Bloader.Game;
using D3Bloader.Game.Objects;
using D3Bloader.Scripting;
using D3_Adventures;
using D3_Adventures.Enumerations;
using D3_Adventures.Structures;
using TreeSharp;
///////////////////////////////////////////////////
// This isn't much to go off of, but I'm very limited time-wise atm so all of this including the loader is a very basic
// implementation and sample of whats to come.
///////////////////////////////////////////////////

namespace D3Bloader.Script.BasicCombatBot
{
    class Script_BasicCombat : Scripts.IScript
    {	///////////////////////////////////////////////////
        // Member Variables
        ///////////////////////////////////////////////////
        Bot _bot;
        int _tickLastUpdate;
        public Actor[] _monsters;

        ///////////////////////////////////////////////////
        // Member Functions
        ///////////////////////////////////////////////////
        /// <summary>
        /// Performs script initialization
        /// </summary>
        public bool init(IEventObject invoker)
        {
            _bot = invoker as Bot;
            Log.write(TLog.Normal, "BasicCombatBot by HellSpawn & Shadwd");
            return true;
        }

        /// <summary>
        /// Allows the script to maintain itself
        /// </summary>
        public bool poll()
        {
            return true;
        }

        /// <summary>
        /// Triggered when an item is picked.
        /// </summary>
        [Scripts.Event("Toon.ItemPickup")]
        public bool itemPickup(Item itm)
        {
            return true;
        }

        /// <summary>
        /// Triggered when an enemy should be attacked.
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        [Scripts.Event("Toon.AttackEnemy")]
        public bool attackEnemy(Actor who)
        {
            Actions.PowerUseGUID(who.id_acd, SNO.SNOPowerId.Barbarian_Bash);
            return true;
        }


     
        /// <summary>
        /// Triggered when the BT needs to be built.
        /// </summary>
        [Scripts.Event("BT.Build")]
        public Composite BuildTree()
        {
            return new PrioritySelector(

                //we first try and find a target
                new FindNearestEnemeyDecorator()[_bot],

                //then we will attack it.
                new AttackCurrentTargetDecorator()[_bot]

            );
        }

    }
}
