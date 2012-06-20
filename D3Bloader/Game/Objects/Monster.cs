using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using D3_Adventures;
using D3_Adventures.Structures;
using D3_Adventures.Enumerations;

namespace D3Bloader.Game.Objects
{
    public partial class Monster
    {   ///////////////////////////////////////////////////
        // Member Variables
        ///////////////////////////////////////////////////
        public uint ID;
        public int _tickLastUpdate;
        public State _state;
        private Bot _bot;

        public class State
        {
            public bool bCondemned;
            public float posX;
            public float posY;
            public float posZ;
            public double distance;
        }

        #region Member Functions
        ///////////////////////////////////////////////////
        // Member Functions
        ///////////////////////////////////////////////////
        public Monster(Bot bot)
        {
            _bot = bot;
            //Init a new state
            _state = new State();
        }
        #endregion
    }
}
