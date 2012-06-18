using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

using D3Bloader.Scripting;
using D3Bloader.Game.Objects;

using Utilities.MemoryHandling;
using Utilities;

using D3_Adventures;
using D3_Adventures.Structures;
using D3_Adventures.Enumerations;

namespace D3Bloader.Game
{
    public class Bot : IEventObject
    {   ///////////////////////////////////////////////////
        // Member Variables
        ///////////////////////////////////////////////////
        public new LogClient _logger;
        public ConfigSetting _config;
        private Thread _botThread;
        public int _tickLastMove;
        public Data.gameObject[] objs;

        #region Member Functions
        ///////////////////////////////////////////////////
        // Member Functions
        ///////////////////////////////////////////////////

        #region EventObject
        /// <summary>
        /// The event logger, if exists, for this class
        /// </summary>
        public EventHandlers events
        {
            get;
            set;
        }

        #region ThreadedObject
        /// <summary>
        /// The event logger, if exists, for this class
        /// </summary>
        public LogClient _eventLogger
        {
            get;
            set;
        }

        /// <summary>
        /// The sync object for this class
        /// </summary>
        public object _sync
        {
            get;
            set;
        }
        #endregion

        /// <summary>
        /// Initializes events for the event object
        /// </summary>
        public void eventInit(bool bParseEvents)
        {
            EventObjects.eventInit(this, bParseEvents);
        }

        /// <summary>
        /// Triggers an event
        /// </summary>
        public void trigger(string name, params object[] args)
        {
            EventObjects.trigger(this, name, true, args);
        }

        /// <summary>
        /// Calls a singlecast event, returning a value
        /// </summary>
        public object call(string name, params object[] args)
        {
            return EventObjects.callsync(this, name, true, args);
        }

        /// <summary>
        /// Calls a singlecast event, returning a value
        /// </summary>
        public object callsync(string name, bool bSync, params object[] args)
        {
            return EventObjects.callsync(this, name, bSync, args);
        }

        /// <summary>
        /// Determines if a event type exists
        /// </summary>
        public bool exists(string name)
        {	//Does the event exist?
            HandlerList list;
            return events.TryGetValue(name, out list);
        }

        /// <summary>
        /// Flushes the handlerlist - removing all handlers
        /// </summary>
        public void flushEvents()
        {	//Kill all the handlers!
            using (DdMonitor.Lock(_sync))
                events.Clear();
        }
        #endregion

        /// <summary>
        /// Generic Constructor
        /// </summary>
        public Bot()
        {
        }

        /// <summary>
        /// Performs loader-related initialization
        /// </summary>
        public virtual bool init()
        {
            //Load Config
            _config = new Xmlconfig("config.xml", false).Settings;

            // Load scripts
            ///////////////////////////////////////////////
            Log.write("Loading scripts..");

            //Obtain the bot and operation types
            ConfigSetting scriptConfig = new Xmlconfig("scripts.xml", false).Settings;
            IList<ConfigSetting> scripts = scriptConfig["scripts"].GetNamedChildren("type");

            //Load the bot types
            List<Scripting.InvokerType> scriptingBotTypes = new List<Scripting.InvokerType>();

            foreach (ConfigSetting cs in scripts)
            {	//Convert the config entry to a bottype structure
                scriptingBotTypes.Add(
                    new Scripting.InvokerType(
                            cs.Value,
                            cs["inheritDefaultScripts"].boolValue,
                            cs["scriptDir"].Value)
                );
            }

            //Load them into the scripting engine
            Scripting.Scripts.loadBotTypes(scriptingBotTypes);

            try
            {	//Loads!
                bool bSuccess = Scripting.Scripts.compileScripts();
                if (!bSuccess)
                {	//Failed. Exit
                    Log.write(TLog.Error, "Unable to load scripts.");
                    return false;
                }
            }
            catch (Exception ex)
            {	//Error while compiling
                Log.write(TLog.Exception, "Exception while compiling scripts:\n" + ex.ToString());
                return false;
            }

            string invokerType = _config["General/botType"].Value;
            //Instance our botType
            if (!Scripting.Scripts.invokerTypeExists(invokerType))
            {
                Log.write(TLog.Error, "Unable to find botType '{0}'", invokerType);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Starts our new bot thread
        /// </summary>
        public void begin()
        {
            _logger = Log.createClient(_config["General/botScript"].Value);
            _botThread = new Thread(new ThreadStart(newBot));
            _botThread.IsBackground = true;
            _botThread.Name = "BotThread";
            _botThread.Start();
        }

        /// <summary>
        /// Looks after our BotState
        /// </summary>
        public void newBot()
        {
            //Instance our Scripting Environment
            ScriptBot scriptBot;
            using (LogAssume.Assume(_logger))
            {
                scriptBot = new ScriptBot(this, _config["General/botScript"].Value);
                scriptBot.init();
            }

            //Start polling our script
            while (true)
            {
                using (LogAssume.Assume(_logger))
                   scriptBot.poll();
                //Sleep for a bit..
                Thread.Sleep(5);
            }
        }

        /// <summary>
        /// Allows the bot to keep it's state up-to-date
        /// </summary>
        public virtual bool poll()
        {
            objs = Data.iterateObjectList();
            return true;
        }
        #endregion

        #region Accessors
        ///////////////////////////////////////////////////
        // Accessors
        ///////////////////////////////////////////////////
        private Data.gameObject getObjectByName(string name)
        {
            return objs.Where(o => o.name == name).FirstOrDefault();
        }

        /// <summary>
        /// Returns a sorted list of mobs in a given range.
        /// </summary>
        public List<Monster> getMobsInRange(float x, float y, int range)
        {
            List<Monster> mobs = new List<Monster>();
            List<Data.gameObject> objList = new List<Data.gameObject>();

            //Grab monsters in our given range...
            objList = objs.Where(mob => mob.data2 == 29944 && mob.distanceFromMe <= range).ToList();
            
            // Sort by distance to Toon
            objList.OrderBy(mob => mob.distanceFromMe);

            foreach (Data.gameObject m in objList)
            {
                Monster mob = new Monster();
                mob.mobID = m.guid;
                mobs.Add(mob);
            }

            return mobs;
        }

        /// <summary>
        /// Returns a sorted list of items in a given range.
        /// </summary>
        public List<Item> getItemsInRange(float x, float y, int range)
        {
            List<Item> items = new List<Item>();
            List<Data.gameObject> objList = new List<Data.gameObject>();

            //Grab items in our given range...
            objList = objs.Where(itm => itm.data == 2 && itm.data2 == -1 && itm.distanceFromMe <= range).ToList();

            // Sort by distance to Toon
            objList.OrderBy(itm => itm.distanceFromMe);

            foreach (Data.gameObject i in objList)
            {
                Item itm = new Item();
                itm.itemID = i.guid;
                items.Add(itm);
            }

            return items;
        }

        /// <summary>
        /// Simple Item Pickup
        /// </summary>
        public virtual void pickItem(Data.gameObject itm)
        {
        }


        /// <summary>
        /// Used for moving from point a to b
        /// </summary>
        public void moveTo(float x, float y, float z)
        {
            Log.write(String.Format("Moving to {0},{1}", x, y));
            Actions.moveToPos(x, y, z);
            _tickLastMove = Environment.TickCount;
        }

        #endregion
    }
}
