using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using D3_Adventures;
using D3_Adventures.Memory_Handling;
using Utilities.ProcessTools;

namespace D3Bloader
{
    public static class Program
    {
        static Game.Bot _bot;
        public static string exeName = "Diablo III";
        public static MemoryManager mem = D3_Adventures.Globals.mem;

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
            IntPtr pHandle = PT.GetProcessHandle(exeName);
            if (pHandle == IntPtr.Zero)
            {
                Log.write("Failed to find game process, Is Diablo 3 running?");
                Thread.Sleep(8000);
                return;
            }

            Globals.mem.Attach();


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

        public enum MouseEventFlags
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        static public int LeftClick(int x, int y, bool random_sleep = true, int lowest_sleep = 100, int highes_sleep = 200, bool chest_click = false)
        {
            int sleep = 0;
            Random random = new Random();
            if (random_sleep)
            {
                sleep = random.Next(lowest_sleep, highes_sleep);
                System.Threading.Thread.Sleep(sleep);
            }

            Point tmp = Cursor.Position;

            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);
            Cursor.Position = ConvertToScreenPixel(new Point(x, y));
            if (chest_click)
                System.Threading.Thread.Sleep(50);
            mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
            mouse_event((int)(MouseEventFlags.LEFTUP), 0, 0, 0, 0);


            Cursor.Position = tmp;

            return sleep;
        }

        public static void setD3Foreground()
        {
            SetForegroundWindow(getD3WinHandle());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static IntPtr getD3WinHandle()
        {
            Process[] processes = Process.GetProcessesByName("Diablo III");
            if (processes.Count() > 1)
                throw new Exception("Too many Processes named Diablo 3!");

            if (processes.Count() == 0)
                throw new Exception("Diablo 3 not found!");
            return processes[0].MainWindowHandle;
        }

        [DllImport("user32.dll")]
        private static extern int GetWindowRect(IntPtr hwnd, out Rectangle rect);
        static private Point ConvertToScreenPixel(Point point)
        {
            Rectangle rect;

            GetWindowRect(getD3WinHandle(), out rect);

            Point ret = new Point();

            ret.X = rect.Location.X + point.X;
            ret.Y = rect.Location.Y + point.Y;

            return ret;
        }
    }
}
