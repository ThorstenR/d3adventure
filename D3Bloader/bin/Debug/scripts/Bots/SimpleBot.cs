using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3Bloader.Game;
using D3Bloader.Scripting;
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

            //Simple "unstucker", limit of 5 minutes of no movement.
            if (_bot._tickLastMove != 0 && ((now - _bot._tickLastMove) / 1000) > 3600)
            { //Leave game/whatever...
            }

            return true;
        }
    }
}
