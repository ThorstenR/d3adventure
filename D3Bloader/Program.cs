using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using Utilities.MemoryHandling;
using D3_Adventures;

namespace D3Bloader
{
    public static class Program
    {
        static Game.Bot _bot;
        public static string exeName = "Diablo III";
        public static ReadWriteMemory mem;
        public static bool debugMessages = false;
        public static bool screwWarden = true; // turn to true to use things that use memory writing

        /// <summary>
        /// Makes a note of all unhandled exceptions
        /// </summary>
        public static void onException(object o, UnhandledExceptionEventArgs e)
        {	//Talk about the exception
            using (LogAssume.Assume(_bot._logger))
                Log.write(TLog.Exception, "Unhandled exception:\r\n" + e.ExceptionObject.ToString());
        }

        static void Main(string[] args)
        {
            if (!Log.init())
            {	//Abort..
                Console.WriteLine("Logger initialization failed, exiting..");
                Thread.Sleep(10000);
                return;
            }

            DdMonitor.bNoSync = false;
            DdMonitor.bEnabled = false;
            DdMonitor.DefaultTimeout = -1;

            //Register our catch-all exception handler
            Thread.GetDomain().UnhandledException += onException;

            //Create a logging client for the main loader thread
            LogClient handlerLogger = Log.createClient("LoaderHandler");
            Log.assume(handlerLogger);

            //Is Diablo 3 available?
            IntPtr pHandle = D3_Adventures.Utilities.GetProcessHandle(exeName);
            if (pHandle == IntPtr.Zero)
            {
                Log.write("Failed to find game process, Is Diablo 3 running?");
                Thread.Sleep(8000);
                return;
            }

            //Start our R/W Class
            mem = new ReadWriteMemory(pHandle);

            //Initilize the bot!
            Game.Bot bot = new Game.Bot();
            if (!bot.init())
            {
                Log.write("Failed to initialize D3BLoader, exiting...");
                Thread.Sleep(8000);
                return;
            }

            //Begin!
            bot.begin();

            Log.write("D3BLoader running...");

            while (true)
            {
                Console.Read();
            }
        }
    }
}
