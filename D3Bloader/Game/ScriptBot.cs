using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using D3Bloader.Scripting;

namespace D3Bloader.Game
{
    // ScriptArena Class
    /// Exposes the arena methodology to scripting
    ///////////////////////////////////////////////////////
    public partial class ScriptBot : Bot
    {	// Member variables
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
            //Load the associated scripts
            _scripts = Scripts.instanceScripts(this, _scriptType);

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

            return true;
        }
    }
}