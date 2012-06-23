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
///////////////////////////////////////////////////
// This isn't much to go off of, but I'm very limited time-wise atm so all of this including the loader is a very basic
// implementation and sample of whats to come.
///////////////////////////////////////////////////

namespace D3Bloader.Script.SimpleBot
{
    class Script_SimpleBot : Scripts.IScript
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
            Log.write(TLog.Normal, "SimpleBot by HellSpawn");
            return true;
        }

        /// <summary>
        /// Allows the script to maintain itself
        /// </summary>
        public bool poll()
        {
            int now = Environment.TickCount;
            _monsters = Data.getMonsters();

            Log.write(String.Format("X: {0} Y: {1}", Data.GetMe().Pos1.x, Data.GetMe().Pos1.y));

            //Simple "attack all" script. Just run around and destroy mobs!
            foreach (Actor actor in _monsters)
            {
                //Sanity check...
                if (actor.unknown_data2 != 29944)
                    continue;

                //Don't attack ourself!
                if (actor.id_acd == Data.toonID)
                    continue;

                //Needs work!
                if (!actor.isAlive())
                    continue;

                //Range of 35
                if (actor.distanceFromMe > 35)
                    continue;

                //Weird bug...
                if (actor.distanceFromMe == 0)
                    continue;

                //isAlive doesn't work after re-iterating so bleh..
                if (actor.id_acd.ToString("X") == "FFFFFFFF")
                    continue;

                //Debug
                //Log.write(String.Format("Attacking: {0} Distance: {1}", actor.id_acd.ToString("X"), actor.distanceFromMe));

                //Attack!
                Actions.interactGUID(actor.id_acd, SNO.SNOPowerId.DemonHunter_BolaShot);

            }

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
    }
}
