using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;

namespace Utilities.WinControl
{
    /// thanks to http://www.pinvoke.net/default.aspx/user32.clipcursor
    public struct RECT
    {
        #region Variables.
        /// <summary>
        /// Left position of the rectangle.
        /// </summary>
        public int Left;
        /// <summary>
        /// Top position of the rectangle.
        /// </summary>
        public int Top;
        /// <summary>
        /// Right position of the rectangle.
        /// </summary>
        public int Right;
        /// <summary>
        /// Bottom position of the rectangle.
        /// </summary>
        public int Bottom;
        #endregion

        public int Width
        {
            get
            {
                return Math.Abs(Right - Left);
            }
        }

        public int Height
        {
            get
            {
                return Math.Abs(Top - Bottom);
            }
        }

        #region Operators.
        /// <summary>
        /// Operator to convert a RECT to Drawing.Rectangle.
        /// </summary>
        /// <param name="rect">Rectangle to convert.</param>
        /// <returns>A Drawing.Rectangle</returns>
        public static implicit operator Rectangle(RECT rect)
        {
            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        /// <summary>
        /// Operator to convert Drawing.Rectangle to a RECT.
        /// </summary>
        /// <param name="rect">Rectangle to convert.</param>
        /// <returns>RECT rectangle.</returns>
        public static implicit operator RECT(Rectangle rect)
        {
            return new RECT(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }
        #endregion

        #region Constructor.
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="left">Horizontal position.</param>
        /// <param name="top">Vertical position.</param>
        /// <param name="right">Right most side.</param>
        /// <param name="bottom">Bottom most side.</param>
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        #endregion
    }

    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public System.Drawing.Point ptMinPosition;
        public System.Drawing.Point ptMaxPosition;
        public System.Drawing.Rectangle rcNormalPosition;
    }

    // http://pinvoke.net/default.aspx/Structures.WINDOWINFO
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWINFO
    {
        public uint cbSize;
        public RECT rcWindow;
        public RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;

        public WINDOWINFO(Boolean? filler)
            : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
        {
            cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
        }

    }

    public class WC
    {
        public const int SM_CXFRAME = 32;
        public const int SM_CYFRAME = 33;
        public const int SM_CYCAPTION = 4;
        public const int SM_SWAPBUTTON = 23;
        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;
        public const int SM_CMONITORS = 80;
        public const int SM_CXVIRTUALSCREEN = 78;
        public const int SM_CYVIRTUALSCREEN = 79;

        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        public const int MOUSEEVENTF_LEFTUP = 0x0004;
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        public const int MOUSEEVENTF_RIGHTUP = 0x0010;

        #region Custom Methods
        public static Point CenterWindow(RECT rect1, RECT rect2)
        {
            int x = rect1.Left + rect1.Width / 2 - rect2.Width / 2;
            int y = rect1.Top + rect1.Height / 2 - rect2.Height / 2;
            return new Point(x, y);
        }
        #endregion

        #region Handle and ID

        public static IntPtr ID2Handle(int ID)
        {
            try
            {
                return Process.GetProcessById(ID).MainWindowHandle;
            }
            catch { return IntPtr.Zero; }
        }

        public static int GetIDTitle(string Title)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle == Title) return p.Id;
            }
            return 0;
        }

        public static int GetIDName(string Name)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName == Name) return p.Id;
            }
            return 0;
        }

        public static int Hwnd2ID(IntPtr hwnd)
        {
            uint ui;
            GetWindowThreadProcessId(hwnd, out ui);
            return (int)ui;
        }

        #endregion

        #region Window Gets

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder buf, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        // When you don't want the ProcessId, use this overload and pass IntPtr.Zero for the second parameter
        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        public static EnumWindowsItem GetChildWindow(string title, string className)
        {
            if (title == "" && className == "")
            {
                return null;
            }

            EnumWindows windows = new EnumWindows();
            windows.GetWindows();

            foreach (EnumWindowsItem child in windows.Items)
            {
                if (className == "")
                {
                    if (child.Text == title)
                    {
                        return child;
                    }
                }
                if (child.Text == title && child.ClassName == className)
                {
                    return child;
                }
                else if (title == "")
                {
                    if (child.ClassName == className)
                    {
                        return child;
                    }
                }
            }

            return null;
        }

        public static Point winGetPosition(IntPtr hwnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hwnd, ref placement);
            return new Point(placement.rcNormalPosition.X, placement.rcNormalPosition.Y);
        }

        public static Size winGetSize(IntPtr hwnd)
        {
            RECT r;
            GetWindowRect(hwnd, out r);
            return new Size(r.Right - r.Left, r.Bottom - r.Top);
        }

        public static Size winGetClientSize(IntPtr hwnd)
        {
            RECT r;
            GetClientRect(hwnd, out r);
            return new Size(r.Right - r.Left, r.Bottom - r.Top);
        }

        public static string winGetText(int ID)
        {
            // Allocate correct string length first
            int length = GetWindowTextLength(ID2Handle(ID));
            StringBuilder sb = new StringBuilder(length + 1);
            GetWindowText(ID2Handle(ID), sb, sb.Capacity);
            return sb.ToString();
        }

        public static bool winGetIsTopmost(IntPtr Handle)
        {
            return (GetWindowLong(Handle, GWL_EXSTYLE) & WS_EX_TOPMOST) != 0;
        }

        // http://pinvoke.net/default.aspx/Enums.SystemMetric
        #region System Metrics
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(SystemMetric smIndex);

        /// <summary>
        /// Flags used with the Windows API (User32.dll):GetSystemMetrics(SystemMetric smIndex)
        ///   
        /// This Enum and declaration signature was written by Gabriel T. Sharp
        /// ai_productions@verizon.net or osirisgothra@hotmail.com
        /// Obtained on pinvoke.net, please contribute your code to support the wiki!
        /// </summary>
        public enum SystemMetric : int
        {
            /// <summary>
            ///  Width of the screen of the primary display monitor, in pixels. This is the same values obtained by calling GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, HORZRES).
            /// </summary>
            SM_CXSCREEN = 0,
            /// <summary>
            /// Height of the screen of the primary display monitor, in pixels. This is the same values obtained by calling GetDeviceCaps as follows: GetDeviceCaps( hdcPrimaryMonitor, VERTRES).
            /// </summary>
            SM_CYSCREEN = 1,
            /// <summary>
            /// Width of a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CYVSCROLL = 2,
            /// <summary>
            /// Height of a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXVSCROLL = 3,
            /// <summary>
            /// Height of a caption area, in pixels.
            /// </summary>
            SM_CYCAPTION = 4,
            /// <summary>
            /// Width of a window border, in pixels. This is equivalent to the SM_CXEDGE value for windows with the 3-D look. 
            /// </summary>
            SM_CXBORDER = 5,
            /// <summary>
            /// Height of a window border, in pixels. This is equivalent to the SM_CYEDGE value for windows with the 3-D look. 
            /// </summary>
            SM_CYBORDER = 6,
            /// <summary>
            /// Thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels. SM_CXFIXEDFRAME is the height of the horizontal border and SM_CYFIXEDFRAME is the width of the vertical border. 
            /// </summary>
            SM_CXDLGFRAME = 7,
            /// <summary>
            /// Thickness of the frame around the perimeter of a window that has a caption but is not sizable, in pixels. SM_CXFIXEDFRAME is the height of the horizontal border and SM_CYFIXEDFRAME is the width of the vertical border. 
            /// </summary>
            SM_CYDLGFRAME = 8,
            /// <summary>
            /// Height of the thumb box in a vertical scroll bar, in pixels
            /// </summary>
            SM_CYVTHUMB = 9,
            /// <summary>
            /// Width of the thumb box in a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHTHUMB = 10,
            /// <summary>
            /// Default width of an icon, in pixels. The LoadIcon function can load only icons with the dimensions specified by SM_CXICON and SM_CYICON
            /// </summary>
            SM_CXICON = 11,
            /// <summary>
            /// Default height of an icon, in pixels. The LoadIcon function can load only icons with the dimensions SM_CXICON and SM_CYICON.
            /// </summary>
            SM_CYICON = 12,
            /// <summary>
            /// Width of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CXCURSOR = 13,
            /// <summary>
            /// Height of a cursor, in pixels. The system cannot create cursors of other sizes.
            /// </summary>
            SM_CYCURSOR = 14,
            /// <summary>
            /// Height of a single-line menu bar, in pixels.
            /// </summary>
            SM_CYMENU = 15,
            /// <summary>
            /// Width of the client area for a full-screen window on the primary display monitor, in pixels. To get the coordinates of the portion of the screen not obscured by the system taskbar or by application desktop toolbars, call the SystemParametersInfo function with the SPI_GETWORKAREA value.
            /// </summary>
            SM_CXFULLSCREEN = 16,
            /// <summary>
            /// Height of the client area for a full-screen window on the primary display monitor, in pixels. To get the coordinates of the portion of the screen not obscured by the system taskbar or by application desktop toolbars, call the SystemParametersInfo function with the SPI_GETWORKAREA value.
            /// </summary>
            SM_CYFULLSCREEN = 17,
            /// <summary>
            /// For double byte character set versions of the system, this is the height of the Kanji window at the bottom of the screen, in pixels
            /// </summary>
            SM_CYKANJIWINDOW = 18,
            /// <summary>
            /// Nonzero if a mouse with a wheel is installed; zero otherwise
            /// </summary>
            SM_MOUSEWHEELPRESENT = 75,
            /// <summary>
            /// Height of the arrow bitmap on a vertical scroll bar, in pixels.
            /// </summary>
            SM_CYHSCROLL = 20,
            /// <summary>
            /// Width of the arrow bitmap on a horizontal scroll bar, in pixels.
            /// </summary>
            SM_CXHSCROLL = 21,
            /// <summary>
            /// Nonzero if the debug version of User.exe is installed; zero otherwise.
            /// </summary>
            SM_DEBUG = 22,
            /// <summary>
            /// Nonzero if the left and right mouse buttons are reversed; zero otherwise.
            /// </summary>
            SM_SWAPBUTTON = 23,
            /// <summary>
            /// Reserved for future use
            /// </summary>
            SM_RESERVED1 = 24,
            /// <summary>
            /// Reserved for future use
            /// </summary>
            SM_RESERVED2 = 25,
            /// <summary>
            /// Reserved for future use
            /// </summary>
            SM_RESERVED3 = 26,
            /// <summary>
            /// Reserved for future use
            /// </summary>
            SM_RESERVED4 = 27,
            /// <summary>
            /// Minimum width of a window, in pixels.
            /// </summary>
            SM_CXMIN = 28,
            /// <summary>
            /// Minimum height of a window, in pixels.
            /// </summary>
            SM_CYMIN = 29,
            /// <summary>
            /// Width of a button in a window's caption or title bar, in pixels.
            /// </summary>
            SM_CXSIZE = 30,
            /// <summary>
            /// Height of a button in a window's caption or title bar, in pixels.
            /// </summary>
            SM_CYSIZE = 31,
            /// <summary>
            /// Thickness of the sizing border around the perimeter of a window that can be resized, in pixels. SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border. 
            /// </summary>
            SM_CXFRAME = 32,
            /// <summary>
            /// Thickness of the sizing border around the perimeter of a window that can be resized, in pixels. SM_CXSIZEFRAME is the width of the horizontal border, and SM_CYSIZEFRAME is the height of the vertical border. 
            /// </summary>
            SM_CYFRAME = 33,
            /// <summary>
            /// Minimum tracking width of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMINTRACK = 34,
            /// <summary>
            /// Minimum tracking height of a window, in pixels. The user cannot drag the window frame to a size smaller than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message
            /// </summary>
            SM_CYMINTRACK = 35,
            /// <summary>
            /// Width of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider the two clicks a double-click
            /// </summary>
            SM_CXDOUBLECLK = 36,
            /// <summary>
            /// Height of the rectangle around the location of a first click in a double-click sequence, in pixels. The second click must occur within the rectangle defined by SM_CXDOUBLECLK and SM_CYDOUBLECLK for the system to consider the two clicks a double-click. (The two clicks must also occur within a specified time.) 
            /// </summary>
            SM_CYDOUBLECLK = 37,
            /// <summary>
            /// Width of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CXICON
            /// </summary>
            SM_CXICONSPACING = 38,
            /// <summary>
            /// Height of a grid cell for items in large icon view, in pixels. Each item fits into a rectangle of size SM_CXICONSPACING by SM_CYICONSPACING when arranged. This value is always greater than or equal to SM_CYICON.
            /// </summary>
            SM_CYICONSPACING = 39,
            /// <summary>
            /// Nonzero if drop-down menus are right-aligned with the corresponding menu-bar item; zero if the menus are left-aligned.
            /// </summary>
            SM_MENUDROPALIGNMENT = 40,
            /// <summary>
            /// Nonzero if the Microsoft Windows for Pen computing extensions are installed; zero otherwise.
            /// </summary>
            SM_PENWINDOWS = 41,
            /// <summary>
            /// Nonzero if User32.dll supports DBCS; zero otherwise. (WinMe/95/98): Unicode
            /// </summary>
            SM_DBCSENABLED = 42,
            /// <summary>
            /// Number of buttons on mouse, or zero if no mouse is installed.
            /// </summary>
            SM_CMOUSEBUTTONS = 43,
            /// <summary>
            /// Identical Values Changed After Windows NT 4.0  
            /// </summary>
            SM_CXFIXEDFRAME = SM_CXDLGFRAME,
            /// <summary>
            /// Identical Values Changed After Windows NT 4.0
            /// </summary>
            SM_CYFIXEDFRAME = SM_CYDLGFRAME,
            /// <summary>
            /// Identical Values Changed After Windows NT 4.0
            /// </summary>
            SM_CXSIZEFRAME = SM_CXFRAME,
            /// <summary>
            /// Identical Values Changed After Windows NT 4.0
            /// </summary>
            SM_CYSIZEFRAME = SM_CYFRAME,
            /// <summary>
            /// Nonzero if security is present; zero otherwise.
            /// </summary>
            SM_SECURE = 44,
            /// <summary>
            /// Width of a 3-D border, in pixels. This is the 3-D counterpart of SM_CXBORDER
            /// </summary>
            SM_CXEDGE = 45,
            /// <summary>
            /// Height of a 3-D border, in pixels. This is the 3-D counterpart of SM_CYBORDER
            /// </summary>
            SM_CYEDGE = 46,
            /// <summary>
            /// Width of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged. This value is always greater than or equal to SM_CXMINIMIZED.
            /// </summary>
            SM_CXMINSPACING = 47,
            /// <summary>
            /// Height of a grid cell for a minimized window, in pixels. Each minimized window fits into a rectangle this size when arranged. This value is always greater than or equal to SM_CYMINIMIZED.
            /// </summary>
            SM_CYMINSPACING = 48,
            /// <summary>
            /// Recommended width of a small icon, in pixels. Small icons typically appear in window captions and in small icon view
            /// </summary>
            SM_CXSMICON = 49,
            /// <summary>
            /// Recommended height of a small icon, in pixels. Small icons typically appear in window captions and in small icon view.
            /// </summary>
            SM_CYSMICON = 50,
            /// <summary>
            /// Height of a small caption, in pixels
            /// </summary>
            SM_CYSMCAPTION = 51,
            /// <summary>
            /// Width of small caption buttons, in pixels.
            /// </summary>
            SM_CXSMSIZE = 52,
            /// <summary>
            /// Height of small caption buttons, in pixels.
            /// </summary>
            SM_CYSMSIZE = 53,
            /// <summary>
            /// Width of menu bar buttons, such as the child window close button used in the multiple document interface, in pixels.
            /// </summary>
            SM_CXMENUSIZE = 54,
            /// <summary>
            /// Height of menu bar buttons, such as the child window close button used in the multiple document interface, in pixels.
            /// </summary>
            SM_CYMENUSIZE = 55,
            /// <summary>
            /// Flags specifying how the system arranged minimized windows
            /// </summary>
            SM_ARRANGE = 56,
            /// <summary>
            /// Width of a minimized window, in pixels.
            /// </summary>
            SM_CXMINIMIZED = 57,
            /// <summary>
            /// Height of a minimized window, in pixels.
            /// </summary>
            SM_CYMINIMIZED = 58,
            /// <summary>
            /// Default maximum width of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CXMAXTRACK = 59,
            /// <summary>
            /// Default maximum height of a window that has a caption and sizing borders, in pixels. This metric refers to the entire desktop. The user cannot drag the window frame to a size larger than these dimensions. A window can override this value by processing the WM_GETMINMAXINFO message.
            /// </summary>
            SM_CYMAXTRACK = 60,
            /// <summary>
            /// Default width, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CXMAXIMIZED = 61,
            /// <summary>
            /// Default height, in pixels, of a maximized top-level window on the primary display monitor.
            /// </summary>
            SM_CYMAXIMIZED = 62,
            /// <summary>
            /// Least significant bit is set if a network is present; otherwise, it is cleared. The other bits are reserved for future use
            /// </summary>
            SM_NETWORK = 63,
            /// <summary>
            /// Value that specifies how the system was started: 0-normal, 1-failsafe, 2-failsafe /w net
            /// </summary>
            SM_CLEANBOOT = 67,
            /// <summary>
            /// Width of a rectangle centered on a drag point to allow for limited movement of the mouse pointer before a drag operation begins, in pixels. 
            /// </summary>
            SM_CXDRAG = 68,
            /// <summary>
            /// Height of a rectangle centered on a drag point to allow for limited movement of the mouse pointer before a drag operation begins. This value is in pixels. It allows the user to click and release the mouse button easily without unintentionally starting a drag operation.
            /// </summary>
            SM_CYDRAG = 69,
            /// <summary>
            /// Nonzero if the user requires an application to present information visually in situations where it would otherwise present the information only in audible form; zero otherwise. 
            /// </summary>
            SM_SHOWSOUNDS = 70,
            /// <summary>
            /// Width of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CXMENUCHECK = 71,
            /// <summary>
            /// Height of the default menu check-mark bitmap, in pixels.
            /// </summary>
            SM_CYMENUCHECK = 72,
            /// <summary>
            /// Nonzero if the computer has a low-end (slow) processor; zero otherwise
            /// </summary>
            SM_SLOWMACHINE = 73,
            /// <summary>
            /// Nonzero if the system is enabled for Hebrew and Arabic languages, zero if not.
            /// </summary>
            SM_MIDEASTENABLED = 74,
            /// <summary>
            /// Nonzero if a mouse is installed; zero otherwise. This value is rarely zero, because of support for virtual mice and because some systems detect the presence of the port instead of the presence of a mouse.
            /// </summary>
            SM_MOUSEPRESENT = 19,
            /// <summary>
            /// Windows 2000 (v5.0+) Coordinate of the top of the virtual screen
            /// </summary>
            SM_XVIRTUALSCREEN = 76,
            /// <summary>
            /// Windows 2000 (v5.0+) Coordinate of the left of the virtual screen
            /// </summary>
            SM_YVIRTUALSCREEN = 77,
            /// <summary>
            /// Windows 2000 (v5.0+) Width of the virtual screen
            /// </summary>
            SM_CXVIRTUALSCREEN = 78,
            /// <summary>
            /// Windows 2000 (v5.0+) Height of the virtual screen
            /// </summary>
            SM_CYVIRTUALSCREEN = 79,
            /// <summary>
            /// Number of display monitors on the desktop
            /// </summary>
            SM_CMONITORS = 80,
            /// <summary>
            /// Windows XP (v5.1+) Nonzero if all the display monitors have the same color format, zero otherwise. Note that two displays can have the same bit depth, but different color formats. For example, the red, green, and blue pixels can be encoded with different numbers of bits, or those bits can be located in different places in a pixel's color value. 
            /// </summary>
            SM_SAMEDISPLAYFORMAT = 81,
            /// <summary>
            /// Windows XP (v5.1+) Nonzero if Input Method Manager/Input Method Editor features are enabled; zero otherwise
            /// </summary>
            SM_IMMENABLED = 82,
            /// <summary>
            /// Windows XP (v5.1+) Width of the left and right edges of the focus rectangle drawn by DrawFocusRect. This value is in pixels. 
            /// </summary>
            SM_CXFOCUSBORDER = 83,
            /// <summary>
            /// Windows XP (v5.1+) Height of the top and bottom edges of the focus rectangle drawn by DrawFocusRect. This value is in pixels. 
            /// </summary>
            SM_CYFOCUSBORDER = 84,
            /// <summary>
            /// Nonzero if the current operating system is the Windows XP Tablet PC edition, zero if not.
            /// </summary>
            SM_TABLETPC = 86,
            /// <summary>
            /// Nonzero if the current operating system is the Windows XP, Media Center Edition, zero if not.
            /// </summary>
            SM_MEDIACENTER = 87,
            /// <summary>
            /// Metrics Other
            /// </summary>
            SM_CMETRICS_OTHER = 76,
            /// <summary>
            /// Metrics Windows 2000
            /// </summary>
            SM_CMETRICS_2000 = 83,
            /// <summary>
            /// Metrics Windows NT
            /// </summary>
            SM_CMETRICS_NT = 88,
            /// <summary>
            /// Windows XP (v5.1+) This system metric is used in a Terminal Services environment. If the calling process is associated with a Terminal Services client session, the return value is nonzero. If the calling process is associated with the Terminal Server console session, the return value is zero. The console session is not necessarily the physical console - see WTSGetActiveConsoleSessionId for more information. 
            /// </summary>
            SM_REMOTESESSION = 0x1000,
            /// <summary>
            /// Windows XP (v5.1+) Nonzero if the current session is shutting down; zero otherwise
            /// </summary>
            SM_SHUTTINGDOWN = 0x2000,
            /// <summary>
            /// Windows XP (v5.1+) This system metric is used in a Terminal Services environment. Its value is nonzero if the current session is remotely controlled; zero otherwise
            /// </summary>
            SM_REMOTECONTROL = 0x2001,
        }
        #endregion

        #endregion

        #region Window Sets

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int posX,
            int posY, int width, int height, uint uFlags);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int cx, int cy, bool repaint);

        [DllImport("User32.dll")]
        public static extern Int32 SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

        [Flags()]
        public enum SetWindowPosFlags : uint
        {
            /// <summary>If the calling thread and the thread that owns the window are attached to different input queues, 
            /// the system posts the request to the thread that owns the window. This prevents the calling thread from 
            /// blocking its execution while other threads process the request.</summary>
            /// <remarks>SWP_ASYNCWINDOWPOS</remarks>
            SynchronousWindowPosition = 0x4000,
            /// <summary>Prevents generation of the WM_SYNCPAINT message.</summary>
            /// <remarks>SWP_DEFERERASE</remarks>
            DeferErase = 0x2000,
            /// <summary>Draws a frame (defined in the window's class description) around the window.</summary>
            /// <remarks>SWP_DRAWFRAME</remarks>
            DrawFrame = 0x0020,
            /// <summary>Applies new frame styles set using the SetWindowLong function. Sends a WM_NCCALCSIZE message to 
            /// the window, even if the window's size is not being changed. If this flag is not specified, WM_NCCALCSIZE 
            /// is sent only when the window's size is being changed.</summary>
            /// <remarks>SWP_FRAMECHANGED</remarks>
            FrameChanged = 0x0020,
            /// <summary>Hides the window.</summary>
            /// <remarks>SWP_HIDEWINDOW</remarks>
            HideWindow = 0x0080,
            /// <summary>Does not activate the window. If this flag is not set, the window is activated and moved to the 
            /// top of either the topmost or non-topmost group (depending on the setting of the hWndInsertAfter 
            /// parameter).</summary>
            /// <remarks>SWP_NOACTIVATE</remarks>
            DoNotActivate = 0x0010,
            /// <summary>Discards the entire contents of the client area. If this flag is not specified, the valid 
            /// contents of the client area are saved and copied back into the client area after the window is sized or 
            /// repositioned.</summary>
            /// <remarks>SWP_NOCOPYBITS</remarks>
            DoNotCopyBits = 0x0100,
            /// <summary>Retains the current position (ignores X and Y parameters).</summary>
            /// <remarks>SWP_NOMOVE</remarks>
            IgnoreMove = 0x0002,
            /// <summary>Does not change the owner window's position in the Z order.</summary>
            /// <remarks>SWP_NOOWNERZORDER</remarks>
            DoNotChangeOwnerZOrder = 0x0200,
            /// <summary>Does not redraw changes. If this flag is set, no repainting of any kind occurs. This applies to 
            /// the client area, the nonclient area (including the title bar and scroll bars), and any part of the parent 
            /// window uncovered as a result of the window being moved. When this flag is set, the application must 
            /// explicitly invalidate or redraw any parts of the window and parent window that need redrawing.</summary>
            /// <remarks>SWP_NOREDRAW</remarks>
            DoNotRedraw = 0x0008,
            /// <summary>Same as the SWP_NOOWNERZORDER flag.</summary>
            /// <remarks>SWP_NOREPOSITION</remarks>
            DoNotReposition = 0x0200,
            /// <summary>Prevents the window from receiving the WM_WINDOWPOSCHANGING message.</summary>
            /// <remarks>SWP_NOSENDCHANGING</remarks>
            DoNotSendChangingEvent = 0x0400,
            /// <summary>Retains the current size (ignores the cx and cy parameters).</summary>
            /// <remarks>SWP_NOSIZE</remarks>
            IgnoreResize = 0x0001,
            /// <summary>Retains the current Z order (ignores the hWndInsertAfter parameter).</summary>
            /// <remarks>SWP_NOZORDER</remarks>
            IgnoreZOrder = 0x0004,
            /// <summary>Displays the window.</summary>
            /// <remarks>SWP_SHOWWINDOW</remarks>
            ShowWindow = 0x0040,
        }

        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);

        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 SWP_NOZORDER = 0x0004;
        const UInt32 SWP_NOREDRAW = 0x0008;
        const UInt32 SWP_NOACTIVATE = 0x0010;
        const UInt32 SWP_FRAMECHANGED = 0x0020;  /* The frame changed: send WM_NCCALCSIZE */
        const UInt32 SWP_SHOWWINDOW = 0x0040;
        const UInt32 SWP_HIDEWINDOW = 0x0080;
        const UInt32 SWP_NOCOPYBITS = 0x0100;
        const UInt32 SWP_NOOWNERZORDER = 0x0200;  /* Don't do owner Z ordering */
        const UInt32 SWP_NOSENDCHANGING = 0x0400;  /* Don't send WM_WINDOWPOSCHANGING */

        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;

        const int GWL_ID = (-12);
        const int GWL_STYLE = (-16);
        const int GWL_EXSTYLE = (-20);

        // Window Styles 
        const UInt32 WS_OVERLAPPED = 0;
        const UInt32 WS_POPUP = 0x80000000;
        const UInt32 WS_CHILD = 0x40000000;
        const UInt32 WS_MINIMIZE = 0x20000000;
        const UInt32 WS_VISIBLE = 0x10000000;
        const UInt32 WS_DISABLED = 0x8000000;
        const UInt32 WS_CLIPSIBLINGS = 0x4000000;
        const UInt32 WS_CLIPCHILDREN = 0x2000000;
        const UInt32 WS_MAXIMIZE = 0x1000000;
        const UInt32 WS_CAPTION = 0xC00000;      // WS_BORDER or WS_DLGFRAME  
        const UInt32 WS_BORDER = 0x800000;
        const UInt32 WS_DLGFRAME = 0x400000;
        const UInt32 WS_VSCROLL = 0x200000;
        const UInt32 WS_HSCROLL = 0x100000;
        const UInt32 WS_SYSMENU = 0x80000;
        const UInt32 WS_THICKFRAME = 0x40000;
        const UInt32 WS_GROUP = 0x20000;
        const UInt32 WS_TABSTOP = 0x10000;
        const UInt32 WS_MINIMIZEBOX = 0x20000;
        const UInt32 WS_MAXIMIZEBOX = 0x10000;
        const UInt32 WS_TILED = WS_OVERLAPPED;
        const UInt32 WS_ICONIC = WS_MINIMIZE;
        const UInt32 WS_SIZEBOX = WS_THICKFRAME;

        // Extended Window Styles 
        const UInt32 WS_EX_DLGMODALFRAME = 0x0001;
        const UInt32 WS_EX_NOPARENTNOTIFY = 0x0004;
        const UInt32 WS_EX_TOPMOST = 0x0008;
        const UInt32 WS_EX_ACCEPTFILES = 0x0010;
        const UInt32 WS_EX_TRANSPARENT = 0x0020;
        const UInt32 WS_EX_MDICHILD = 0x0040;
        const UInt32 WS_EX_TOOLWINDOW = 0x0080;
        const UInt32 WS_EX_WINDOWEDGE = 0x0100;
        const UInt32 WS_EX_CLIENTEDGE = 0x0200;
        const UInt32 WS_EX_CONTEXTHELP = 0x0400;
        const UInt32 WS_EX_RIGHT = 0x1000;
        const UInt32 WS_EX_LEFT = 0x0000;
        const UInt32 WS_EX_RTLREADING = 0x2000;
        const UInt32 WS_EX_LTRREADING = 0x0000;
        const UInt32 WS_EX_LEFTSCROLLBAR = 0x4000;
        const UInt32 WS_EX_RIGHTSCROLLBAR = 0x0000;
        const UInt32 WS_EX_CONTROLPARENT = 0x10000;
        const UInt32 WS_EX_STATICEDGE = 0x20000;
        const UInt32 WS_EX_APPWINDOW = 0x40000;
        const UInt32 WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        const UInt32 WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);
        const UInt32 WS_EX_LAYERED = 0x00080000;
        const UInt32 WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        const UInt32 WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
        const UInt32 WS_EX_COMPOSITED = 0x02000000;
        const UInt32 WS_EX_NOACTIVATE = 0x08000000;

        public static void winSetNoSize(int ID)
        {
            SetWindowPos(ID2Handle(ID), HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE);
        }

        public static void winSetNoMove(int ID)
        {
            SetWindowPos(ID2Handle(ID), HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE);
        }

        public static void winSetPosition(int ID, int x, int y)
        {
            SetWindowPos(ID2Handle(ID), HWND_NOTOPMOST, x, y, 0, 0, SWP_NOSIZE);
        }

        public static void winSetPosition(IntPtr winHwnd, int x, int y)
        {
            if (winGetIsTopmost(winHwnd))
                SetWindowPos(winHwnd, HWND_TOPMOST , x, y, 0, 0, SWP_NOSIZE);
            else
                SetWindowPos(winHwnd, HWND_NOTOPMOST, x, y, 0, 0, SWP_NOSIZE);
        }

        public static void winSetSize(int ID, int width, int height)
        {
            SetWindowPos(ID2Handle(ID), HWND_NOTOPMOST, 0, 0, width, height, SWP_NOMOVE);
        }

        public static void winSetTopMost(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        public static void winSetToFront(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
        }

        public static void winSetShow(IntPtr hwnd)
        {
            SetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_SHOWWINDOW);
        }

        public static void winSetHide(int ID)
        {
            SetWindowPos(ID2Handle(ID), HWND_NOTOPMOST, 0, 0, 0, 0, SWP_HIDEWINDOW);
        }

        public static void winSetSize(IntPtr hWnd, int width, int height)
        {
            MoveWindow(hWnd, 0, 0, width, height, false);
        }

        [DllImport("User32.Dll")]
        static extern void SetWindowText(IntPtr handle, String s);

        public static void winSetText(int ID, string S)
        {
            SetWindowText(ID2Handle(ID), S);
        }

        /// <summary>
        /// Sets windows mode to Layer and All mouse interaction  is ignored
        ///  and is recieved by lower windows
        /// </summary>
        /// <param name="hwnd">Window Handle Name</param>
        public static void SetWindowLayeredMode(IntPtr hwnd)
        {
            // make window a windows overlay layer ( no clicks or mouse will be captured, and will fall to lower windows)
            SetWindowLong(hwnd, -20, GetWindowLong(hwnd, -20) | 0x00000020);
        }

        #endregion

        [DllImport("user32.dll")]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, WindowShowStyle nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("kernel32.dll")]
        public static extern bool QueryPerformanceFrequency(out long lpFrequency);

        [DllImport("user32.dll")]
        public static extern bool ClientToScreen(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(
               IntPtr parentHwnd,
               IntPtr childAfterHwnd,
               IntPtr className,
               string windowText);

        [Flags]
        public enum VK : ushort
        {
            //
            // Virtual Keys, Standard Set
            //
            VK_LBUTTON = 0x01,
            VK_RBUTTON = 0x02,
            VK_CANCEL = 0x03,
            VK_MBUTTON = 0x04,    // NOT contiguous with L & RBUTTON

            VK_XBUTTON1 = 0x05,    // NOT contiguous with L & RBUTTON
            VK_XBUTTON2 = 0x06,    // NOT contiguous with L & RBUTTON

            // 0x07 : unassigned

            VK_BACK = 0x08,
            VK_TAB = 0x09,

            // 0x0A - 0x0B : reserved

            VK_CLEAR = 0x0C,
            VK_RETURN = 0x0D,

            VK_SHIFT = 0x10,
            VK_CONTROL = 0x11,
            VK_MENU = 0x12,
            VK_PAUSE = 0x13,
            VK_CAPITAL = 0x14,

            VK_KANA = 0x15,
            VK_HANGEUL = 0x15,  // old name - should be here for compatibility
            VK_HANGUL = 0x15,
            VK_JUNJA = 0x17,
            VK_FINAL = 0x18,
            VK_HANJA = 0x19,
            VK_KANJI = 0x19,

            VK_ESCAPE = 0x1B,

            VK_CONVERT = 0x1C,
            VK_NONCONVERT = 0x1D,
            VK_ACCEPT = 0x1E,
            VK_MODECHANGE = 0x1F,

            VK_SPACE = 0x20,
            VK_PRIOR = 0x21,
            VK_NEXT = 0x22,
            VK_END = 0x23,
            VK_HOME = 0x24,
            VK_LEFT = 0x25,
            VK_UP = 0x26,
            VK_RIGHT = 0x27,
            VK_DOWN = 0x28,
            VK_SELECT = 0x29,
            VK_PRINT = 0x2A,
            VK_EXECUTE = 0x2B,
            VK_SNAPSHOT = 0x2C,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_HELP = 0x2F,

            //
            // VK_0 - VK_9 are the same as ASCII '0' - '9' (0x30 - 0x39)
            // 0x40 : unassigned
            // VK_A - VK_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A)
            //

            VK_LWIN = 0x5B,
            VK_RWIN = 0x5C,
            VK_APPS = 0x5D,

            //
            // 0x5E : reserved
            //

            VK_SLEEP = 0x5F,

            VK_NUMPAD0 = 0x60,
            VK_NUMPAD1 = 0x61,
            VK_NUMPAD2 = 0x62,
            VK_NUMPAD3 = 0x63,
            VK_NUMPAD4 = 0x64,
            VK_NUMPAD5 = 0x65,
            VK_NUMPAD6 = 0x66,
            VK_NUMPAD7 = 0x67,
            VK_NUMPAD8 = 0x68,
            VK_NUMPAD9 = 0x69,
            VK_MULTIPLY = 0x6A,
            VK_ADD = 0x6B,
            VK_SEPARATOR = 0x6C,
            VK_SUBTRACT = 0x6D,
            VK_DECIMAL = 0x6E,
            VK_DIVIDE = 0x6F,
            VK_F1 = 0x70,
            VK_F2 = 0x71,
            VK_F3 = 0x72,
            VK_F4 = 0x73,
            VK_F5 = 0x74,
            VK_F6 = 0x75,
            VK_F7 = 0x76,
            VK_F8 = 0x77,
            VK_F9 = 0x78,
            VK_F10 = 0x79,
            VK_F11 = 0x7A,
            VK_F12 = 0x7B,
            VK_F13 = 0x7C,
            VK_F14 = 0x7D,
            VK_F15 = 0x7E,
            VK_F16 = 0x7F,
            VK_F17 = 0x80,
            VK_F18 = 0x81,
            VK_F19 = 0x82,
            VK_F20 = 0x83,
            VK_F21 = 0x84,
            VK_F22 = 0x85,
            VK_F23 = 0x86,
            VK_F24 = 0x87,

            //
            // 0x88 - 0x8F : unassigned
            //

            VK_NUMLOCK = 0x90,
            VK_SCROLL = 0x91,

            //
            // VK_L* & VK_R* - left and right Alt, Ctrl and Shift virtual keys.
            // Used only as parameters to GetAsyncKeyState() and GetKeyState().
            // No other API or message will distinguish left and right keys in this way.
            //
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3,
            VK_LMENU = 0xA4,
            VK_RMENU = 0xA5,

            VK_BROWSER_BACK = 0xA6,
            VK_BROWSER_FORWARD = 0xA7,
            VK_BROWSER_REFRESH = 0xA8,
            VK_BROWSER_STOP = 0xA9,
            VK_BROWSER_SEARCH = 0xAA,
            VK_BROWSER_FAVORITES = 0xAB,
            VK_BROWSER_HOME = 0xAC,

            VK_VOLUME_MUTE = 0xAD,
            VK_VOLUME_DOWN = 0xAE,
            VK_VOLUME_UP = 0xAF,
            VK_MEDIA_NEXT_TRACK = 0xB0,
            VK_MEDIA_PREV_TRACK = 0xB1,
            VK_MEDIA_STOP = 0xB2,
            VK_MEDIA_PLAY_PAUSE = 0xB3,
            VK_LAUNCH_MAIL = 0xB4,
            VK_LAUNCH_MEDIA_SELECT = 0xB5,
            VK_LAUNCH_APP1 = 0xB6,
            VK_LAUNCH_APP2 = 0xB7,

            //
            // 0xB8 - 0xB9 : reserved
            //

            VK_OEM_1 = 0xBA,   // ';:' for US
            VK_OEM_PLUS = 0xBB,   // '+' any country
            VK_OEM_COMMA = 0xBC,   // ',' any country
            VK_OEM_MINUS = 0xBD,   // '-' any country
            VK_OEM_PERIOD = 0xBE,   // '.' any country
            VK_OEM_2 = 0xBF,   // '/?' for US
            VK_OEM_3 = 0xC0,   // '`~' for US

            //
            // 0xC1 - 0xD7 : reserved
            //

            //
            // 0xD8 - 0xDA : unassigned
            //

            VK_OEM_4 = 0xDB,  //  '[{' for US
            VK_OEM_5 = 0xDC,  //  '\|' for US
            VK_OEM_6 = 0xDD,  //  ']}' for US
            VK_OEM_7 = 0xDE,  //  ''"' for US
            VK_OEM_8 = 0xDF

            //
            // 0xE0 : reserved
            //
        }

        public enum WindowShowStyle
        {
            /// <summary>Hides the window and activates another window.</summary>
            /// <remarks>See SW_HIDE</remarks>
            Hide = 0,
            /// <summary>Activates and displays a window. If the window is minimized
            /// or maximized, the system restores it to its original size and
            /// position. An application should specify this flag when displaying
            /// the window for the first time.</summary>
            /// <remarks>See SW_SHOWNORMAL</remarks>
            ShowNormal = 1,
            /// <summary>Activates the window and displays it as a minimized window.</summary>
            /// <remarks>See SW_SHOWMINIMIZED</remarks>
            ShowMinimized = 2,
            /// <summary>Activates the window and displays it as a maximized window.</summary>
            /// <remarks>See SW_SHOWMAXIMIZED</remarks>
            ShowMaximized = 3,
            /// <summary>Maximizes the specified window.</summary>
            /// <remarks>See SW_MAXIMIZE</remarks>
            Maximize = 3,
            /// <summary>Displays a window in its most recent size and position.
            /// This value is similar to "ShowNormal", except the window is not
            /// actived.</summary>
            /// <remarks>See SW_SHOWNOACTIVATE</remarks>
            ShowNormalNoActivate = 4,
            /// <summary>Activates the window and displays it in its current size
            /// and position.</summary>
            /// <remarks>See SW_SHOW</remarks>
            Show = 5,
            /// <summary>Minimizes the specified window and activates the next
            /// top-level window in the Z order.</summary>
            /// <remarks>See SW_MINIMIZE</remarks>
            Minimize = 6,
            /// <summary>Displays the window as a minimized window. This value is
            /// similar to "ShowMinimized", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWMINNOACTIVE</remarks>
            ShowMinNoActivate = 7,
            /// <summary>Displays the window in its current size and position. This
            /// value is similar to "Show", except the window is not activated.</summary>
            /// <remarks>See SW_SHOWNA</remarks>
            ShowNoActivate = 8,
            /// <summary>Activates and displays the window. If the window is
            /// minimized or maximized, the system restores it to its original size
            /// and position. An application should specify this flag when restoring
            /// a minimized window.</summary>
            /// <remarks>See SW_RESTORE</remarks>
            Restore = 9,
            /// <summary>Sets the show state based on the SW_ value specified in the
            /// STARTUPINFO structure passed to the CreateProcess function by the
            /// program that started the application.</summary>
            /// <remarks>See SW_SHOWDEFAULT</remarks>
            ShowDefault = 10,
            /// <summary>Windows 2000/XP: Minimizes a window, even if the thread
            /// that owns the window is hung. This flag should only be used when
            /// minimizing windows from a different thread.</summary>
            /// <remarks>See SW_FORCEMINIMIZE</remarks>
            ForceMinimized = 11
        }

        /// <summary>
        ///    Performs a bit-block transfer of the color data corresponding to a
        ///    rectangle of pixels from the specified source device context into
        ///    a destination device context.
        /// </summary>
        /// <param name="hdc">Handle to the destination device context.</param>
        /// <param name="nXDest">The leftmost x-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nYDest">The topmost y-coordinate of the destination rectangle (in pixels).</param>
        /// <param name="nWidth">The width of the source and destination rectangles (in pixels).</param>
        /// <param name="nHeight">The height of the source and the destination rectangles (in pixels).</param>
        /// <param name="hdcSrc">Handle to the source device context.</param>
        /// <param name="nXSrc">The leftmost x-coordinate of the source rectangle (in pixels).</param>
        /// <param name="nYSrc">The topmost y-coordinate of the source rectangle (in pixels).</param>
        /// <param name="dwRop">A raster-operation code.</param>
        /// <returns>
        ///    <c>true</c> if the operation succeeded, <c>false</c> otherwise.
        /// </returns>
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

        /// <summary>
        ///     Specifies a raster-operation code. These codes define how the color data for the
        ///     source rectangle is to be combined with the color data for the destination
        ///     rectangle to achieve the final color.
        /// </summary>
        public enum TernaryRasterOperations : uint
        {
            /// <summary>dest = source</summary>
            SRCCOPY = 0x00CC0020,
            /// <summary>dest = source OR dest</summary>
            SRCPAINT = 0x00EE0086,
            /// <summary>dest = source AND dest</summary>
            SRCAND = 0x008800C6,
            /// <summary>dest = source XOR dest</summary>
            SRCINVERT = 0x00660046,
            /// <summary>dest = source AND (NOT dest)</summary>
            SRCERASE = 0x00440328,
            /// <summary>dest = (NOT source)</summary>
            NOTSRCCOPY = 0x00330008,
            /// <summary>dest = (NOT src) AND (NOT dest)</summary>
            NOTSRCERASE = 0x001100A6,
            /// <summary>dest = (source AND pattern)</summary>
            MERGECOPY = 0x00C000CA,
            /// <summary>dest = (NOT source) OR dest</summary>
            MERGEPAINT = 0x00BB0226,
            /// <summary>dest = pattern</summary>
            PATCOPY = 0x00F00021,
            /// <summary>dest = DPSnoo</summary>
            PATPAINT = 0x00FB0A09,
            /// <summary>dest = pattern XOR dest</summary>
            PATINVERT = 0x005A0049,
            /// <summary>dest = (NOT dest)</summary>
            DSTINVERT = 0x00550009,
            /// <summary>dest = BLACK</summary>
            BLACKNESS = 0x00000042,
            /// <summary>dest = WHITE</summary>
            WHITENESS = 0x00FF0062,
            /// <summary>
            /// Capture window as seen on screen.  This includes layered windows 
            /// such as WPF windows with AllowsTransparency="true"
            /// </summary>
            CAPTUREBLT = 0x40000000
        }

        /* OBSOLETE FOR NOW
        #region Mouse Junk?
        //overloads for various actions
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, StringBuilder lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        private static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, String lParam);

        public static IntPtr MakeLParam(int LoWord, int HiWord)
        {
            return (IntPtr)((HiWord << 16) | (LoWord & 0xffff));
        }

        protected class WMmessages
        {
            public const int WM_NULL = 0x000;
            public const int WM_CREATE = 0x001;
            public const int WM_DESTROY = 0x002;
            public const int WM_MOVE = 0x003;
            public const int WM_SIZE = 0x005;
            public const int WM_ACTIVATE = 0x006;
            public const int WM_SETFOCUS = 0x007;
            public const int WM_KILLFOCUS = 0x008;
            public const int WM_ENABLE = 0x00A;
            public const int WM_SETREDRAW = 0x00B;
            public const int WM_SETTEXT = 0x00C;
            public const int WM_GETTEXT = 0x00D;
            public const int WM_GETTEXTLENGTH = 0x00E;
            public const int WM_PAINT = 0x00F;
            public const int WM_CLOSE = 0x010;
            public const int WM_QUERYENDSESSION = 0x011;
            public const int WM_QUIT = 0x012;
            public const int WM_QUERYOPEN = 0x013;
            public const int WM_ERASEBKGND = 0x014;
            public const int WM_SYSCOLORCHANGE = 0x015;
            public const int WM_ENDSESSION = 0x016;
            public const int WM_SHOWWINDOW = 0x018;
            public const int WM_WININICHANGE = 0x01A;
            public const int WM_DEVMODECHANGE = 0x01B;
            public const int WM_ACTIVATEAPP = 0x01C;
            public const int WM_FONTCHANGE = 0x01D;
            public const int WM_TIMECHANGE = 0x01E;
            public const int WM_CANCELMODE = 0x01F;
            public const int WM_SETCURSOR = 0x020;
            public const int WM_MOUSEACTIVATE = 0x021;
            public const int WM_CHILDACTIVATE = 0x022;
            public const int WM_QUEUESYNC = 0x023;
            public const int WM_GETMINMAXINFO = 0x024;
            public const int WM_PAINTICON = 0x026;
            public const int WM_ICONERASEBKGND = 0x027;
            public const int WM_NEXTDLGCTL = 0x028;
            public const int WM_SPOOLERSTATUS = 0x02A;
            public const int WM_DRAWITEM = 0x02B;
            public const int WM_MEASUREITEM = 0x02C;
            public const int WM_DELETEITEM = 0x02D;
            public const int WM_VKEYTOITEM = 0x02E;
            public const int WM_CHARTOITEM = 0x02F;
            public const int WM_SETFONT = 0x030;
            public const int WM_GETFONT = 0x031;
            public const int WM_SETHOTKEY = 0x032;
            public const int WM_GETHOTKEY = 0x033;
            public const int WM_QUERYDRAGICON = 0x037;
            public const int WM_COMPAREITEM = 0x039;
            public const int WM_COMPACTING = 0x041;
            public const int WM_COMMNOTIFY = 0x044; // no longer suported 
            public const int WM_WINDOWPOSCHANGING = 0x046;
            public const int WM_WINDOWPOSCHANGED = 0x047;
            public const int WM_POWER = 0x048;
            public const int WM_COPYDATA = 0x04A;
            public const int WM_CANCELJOURNAL = 0x04B;
            public const int WM_USER = 0x400;
            public const int WM_NOTIFY = 0x04E;
            public const int WM_INPUTLANGCHANGEREQUEST = 0x050;
            public const int WM_INPUTLANGCHANGE = 0x051;
            public const int WM_TCARD = 0x052;
            public const int WM_HELP = 0x053;
            public const int WM_USERCHANGED = 0x054;
            public const int WM_NOTIFYFORMAT = 0x055;
            public const int WM_CONTEXTMENU = 0x07B;
            public const int WM_STYLECHANGING = 0x07C;
            public const int WM_STYLECHANGED = 0x07D;
            public const int WM_DISPLAYCHANGE = 0x07E;
            public const int WM_GETICON = 0x07F;
            public const int WM_SETICON = 0x080;
            public const int WM_NCCREATE = 0x081;
            public const int WM_NCDESTROY = 0x082;
            public const int WM_NCCALCSIZE = 0x083;
            public const int WM_NCHITTEST = 0x084;
            public const int WM_NCPAINT = 0x085;
            public const int WM_NCACTIVATE = 0x086;
            public const int WM_GETDLGCODE = 0x087;
            public const int WM_SYNCPAINT = 0x088;
            public const int WM_NCMOUSEMOVE = 0x0A0;
            public const int WM_NCLBUTTONDOWN = 0x0A1;
            public const int WM_NCLBUTTONUP = 0x0A2;
            public const int WM_NCLBUTTONDBLCLK = 0x0A3;
            public const int WM_NCRBUTTONDOWN = 0x0A4;
            public const int WM_NCRBUTTONUP = 0x0A5;
            public const int WM_NCRBUTTONDBLCLK = 0x0A6;
            public const int WM_NCMBUTTONDOWN = 0x0A7;
            public const int WM_NCMBUTTONUP = 0x0A8;
            public const int WM_NCMBUTTONDBLCLK = 0x0A9;
            public const int WM_NCXBUTTONDOWN = 0x0AB;
            public const int WM_NCXBUTTONUP = 0x0AC;
            public const int WM_NCXBUTTONDBLCLK = 0x0AD;
            public const int WM_INPUT = 0x0FF;
            public const int WM_KEYFIRST = 0x100;
            public const int WM_KEYDOWN = 0x100;
            public const int WM_KEYUP = 0x101;
            public const int WM_CHAR = 0x102;
            public const int WM_DEADCHAR = 0x103;
            public const int WM_SYSKEYDOWN = 0x104;
            public const int WM_SYSKEYUP = 0x105;
            public const int WM_SYSCHAR = 0x106;
            public const int WM_SYSDEADCHAR = 0x107;
            public const int WM_UNICHAR = 0x109;
            public const int WM_KEYLAST = 0x109;
            public const int WM_IME_STARTCOMPOSITION = 0x10D;
            public const int WM_IME_ENDCOMPOSITION = 0x10E;
            public const int WM_IME_COMPOSITION = 0x10F;
            public const int WM_IME_KEYLAST = 0x10F;
            public const int WM_INITDIALOG = 0x110;
            public const int WM_COMMAND = 0x111;
            public const int WM_SYSCOMMAND = 0x112;
            public const int WM_TIMER = 0x113;
            public const int WM_HSCROLL = 0x114;
            public const int WM_VSCROLL = 0x115;
            public const int WM_INITMENU = 0x116;
            public const int WM_INITMENUPOPUP = 0x117;
            public const int WM_MENUSELECT = 0x11F;
            public const int WM_MENUCHAR = 0x120;
            public const int WM_ENTERIDLE = 0x121;
            public const int WM_MENURBUTTONUP = 0x122;
            public const int WM_MENUDRAG = 0x123;
            public const int WM_MENUGETOBJECT = 0x124;
            public const int WM_UNINITMENUPOPUP = 0x125;
            public const int WM_MENUCOMMAND = 0x126;
            public const int WM_CHANGEUISTATE = 0x127;
            public const int WM_UPDATEUISTATE = 0x128;
            public const int WM_QUERYUISTATE = 0x129;
            public const int WM_CTLCOLORMSGBOX = 0x132;
            public const int WM_CTLCOLOREDIT = 0x133;
            public const int WM_CTLCOLORLISTBOX = 0x134;
            public const int WM_CTLCOLORBTN = 0x135;
            public const int WM_CTLCOLORDLG = 0x136;
            public const int WM_CTLCOLORSCROLLBAR = 0x137;
            public const int WM_CTLCOLORSTATIC = 0x138;
            public const int MN_GETHMENU = 0x1E1;
            public const int WM_MOUSEFIRST = 0x200;
            public const int WM_MOUSEMOVE = 0x200;
            public const int WM_LBUTTONDOWN = 0x201;
            public const int WM_LBUTTONUP = 0x202;
            public const int WM_LBUTTONDBLCLK = 0x203;
            public const int WM_RBUTTONDOWN = 0x204;
            public const int WM_RBUTTONUP = 0x205;
            public const int WM_RBUTTONDBLCLK = 0x206;
            public const int WM_MBUTTONDOWN = 0x207;
            public const int WM_MBUTTONUP = 0x208;
            public const int WM_MBUTTONDBLCLK = 0x209;
            public const int WM_MOUSEWHEEL = 0x20A;
            public const int WM_XBUTTONDOWN = 0x20B;
            public const int WM_XBUTTONUP = 0x20C;
            public const int WM_XBUTTONDBLCLK = 0x20D;
            public const int WM_MOUSELAST = 0x20A;
            public const int WM_PARENTNOTIFY = 0x210;
            public const int WM_ENTERMENULOOP = 0x211;
            public const int WM_EXITMENULOOP = 0x212;
            public const int WM_NEXTMENU = 0x213;
            public const int WM_SIZING = 0x214;
            public const int WM_CAPTURECHANGED = 0x215;
            public const int WM_MOVING = 0x216;
            public const int WM_POWERBROADCAST = 0x218;
            public const int WM_DEVICECHANGE = 0x219;
            public const int WM_MDICREATE = 0x220;
            public const int WM_MDIDESTROY = 0x221;
            public const int WM_MDIACTIVATE = 0x222;
            public const int WM_MDIRESTORE = 0x223;
            public const int WM_MDINEXT = 0x224;
            public const int WM_MDIMAXIMIZE = 0x225;
            public const int WM_MDITILE = 0x226;
            public const int WM_MDICASCADE = 0x227;
            public const int WM_MDIICONARRANGE = 0x228;
            public const int WM_MDIGETACTIVE = 0x229;
            public const int WM_MDISETMENU = 0x230;
            public const int WM_ENTERSIZEMOVE = 0x231;
            public const int WM_EXITSIZEMOVE = 0x232;
            public const int WM_DROPFILES = 0x233;
            public const int WM_MDIREFRESHMENU = 0x234;
            public const int WM_IME_SETCONTEXT = 0x281;
            public const int WM_IME_NOTIFY = 0x282;
            public const int WM_IME_CONTROL = 0x283;
            public const int WM_IME_COMPOSITIONFULL = 0x284;
            public const int WM_IME_SELECT = 0x285;
            public const int WM_IME_CHAR = 0x286;
            public const int WM_IME_REQUEST = 0x288;
            public const int WM_IME_KEYDOWN = 0x290;
            public const int WM_IME_KEYUP = 0x291;
            public const int WM_MOUSEHOVER = 0x2A1;
            public const int WM_MOUSELEAVE = 0x2A3;
            public const int WM_NCMOUSEHOVER = 0x2A0;
            public const int WM_NCMOUSELEAVE = 0x2A2;
            public const int WM_WTSSESSION_CHANGE = 0x2B1;
            public const int WM_TABLET_FIRST = 0x2c0;
            public const int WM_TABLET_LAST = 0x2df;
            public const int WM_CUT = 0x300;
            public const int WM_COPY = 0x301;
            public const int WM_PASTE = 0x302;
            public const int WM_CLEAR = 0x303;
            public const int WM_UNDO = 0x304;
            public const int WM_RENDERFORMAT = 0x305;
            public const int WM_RENDERALLFORMATS = 0x306;
            public const int WM_DESTROYCLIPBOARD = 0x307;
            public const int WM_DRAWCLIPBOARD = 0x308;
            public const int WM_PAINTCLIPBOARD = 0x309;
            public const int WM_VSCROLLCLIPBOARD = 0x30A;
            public const int WM_SIZECLIPBOARD = 0x30B;
            public const int WM_ASKCBFORMATNAME = 0x30C;
            public const int WM_CHANGECBCHAIN = 0x30D;
            public const int WM_HSCROLLCLIPBOARD = 0x30E;
            public const int WM_QUERYNEWPALETTE = 0x30F;
            public const int WM_PALETTEISCHANGING = 0x310;
            public const int WM_PALETTECHANGED = 0x311;
            public const int WM_HOTKEY = 0x312;
            public const int WM_PRINT = 0x317;
            public const int WM_PRINTCLIENT = 0x318;
            public const int WM_APPCOMMAND = 0x319;
            public const int WM_THEMECHANGED = 0x31A;
            public const int WM_HANDHELDFIRST = 0x358;
            public const int WM_HANDHELDLAST = 0x35F;
            public const int WM_AFXFIRST = 0x360;
            public const int WM_AFXLAST = 0x37F;
            public const int WM_PENWINFIRST = 0x380;
            public const int WM_PENWINLAST = 0x38F;
        }

        #region MK_Controls
        //
        // Key State Masks for Mouse Messages
        //
        protected class MK_Controls
        {
            public const int MK_LBUTTON = 0x0001;
            public const int MK_RBUTTON = 0x0002;
            public const int MK_SHIFT = 0x0004;
            public const int MK_CONTROL = 0x0008;
            public const int MK_MBUTTON = 0x0010;
        }

        #endregion

        public static IntPtr MouseMove(int ID, int x, int y)
        {
            IntPtr hWnd = ID2Handle(ID);
            if (hWnd != IntPtr.Zero)
                return SendMessage(hWnd, WMmessages.WM_MOUSEMOVE, IntPtr.Zero, MakeLParam(x, y));
            return IntPtr.Zero;
        }

        // modified to send to window handle below mouse
        public static void SendMouseClick(int ID, bool Left)
        {
            IntPtr hWnd = ID2Handle(ID);
            if (hWnd != IntPtr.Zero)
            {
                //MouseMove(ID, x, y);
                Point p = WinControl.winGetPosition(ID);
                Point m = Control.MousePosition;
                int x = m.X - p.X - 3;
                int y = m.Y - p.Y - 25;

                if (x > 0 && y > 0)
                {
                    if (Left)
                    {
                        SendMessage(hWnd, WMmessages.WM_LBUTTONDOWN, (IntPtr)MK_Controls.MK_LBUTTON, MakeLParam(x, y));
                        SendMessage(hWnd, WMmessages.WM_LBUTTONUP, (IntPtr)MK_Controls.MK_LBUTTON, MakeLParam(x, y));
                    }
                    else
                    {
                        SendMessage(hWnd, WMmessages.WM_RBUTTONDOWN, (IntPtr)MK_Controls.MK_RBUTTON, MakeLParam(x, y));
                        SendMessage(hWnd, WMmessages.WM_RBUTTONUP, (IntPtr)MK_Controls.MK_RBUTTON, MakeLParam(x, y));

                    }
                }
            }
        }
        #endregion

        // special function for sending the mouse postion
        // into the message system of another window that would normally
        // rely on focus/direct interaction
        public static void MoveMouseOnWindow(int ID)
        {
            Point p = WinControl.winGetPosition(ID);
            Point m = Control.MousePosition;
            int x = m.X - p.X - 3;
            int y = m.Y - p.Y - 25;

            if (x > 0 && y > 0)
            {
                WinControl.MouseMove(ID, x, y);
            }
        }
    */

        public static bool IsFullScreen(IntPtr hWnd)
        {
            RECT rctWindow;
            RECT rctClient;

            GetWindowRect(hWnd, out rctWindow);
            GetClientRect(hWnd, out rctClient);

            int width = rctClient.Right;
            int height = rctClient.Bottom;

            // Check if the RECT's are the same
            if (rctClient.Bottom == rctWindow.Bottom && rctClient.Right == rctWindow.Right)
            {
                return true;
            }

            // Check for a multi monitor setup
            int monitors = WC.GetSystemMetrics(WC.SystemMetric.SM_CMONITORS);

            // Get the virtual resoltuion
            if (monitors > 1)
            {
                if (rctClient.Bottom == rctWindow.Bottom && rctClient.Right == (rctWindow.Right - rctWindow.Left))
                {
                    return true;
                }
            }

            return false;
        }

        public static object List { get; set; }
    }

    /// <summary>
    /// Window Style Flags
    /// </summary>
    [Flags]
    public enum WindowStyleFlags : uint
    {
        WS_OVERLAPPED = 0x00000000,
        WS_POPUP = 0x80000000,
        WS_CHILD = 0x40000000,
        WS_MINIMIZE = 0x20000000,
        WS_VISIBLE = 0x10000000,
        WS_DISABLED = 0x08000000,
        WS_CLIPSIBLINGS = 0x04000000,
        WS_CLIPCHILDREN = 0x02000000,
        WS_MAXIMIZE = 0x01000000,
        WS_BORDER = 0x00800000,
        WS_DLGFRAME = 0x00400000,
        WS_VSCROLL = 0x00200000,
        WS_HSCROLL = 0x00100000,
        WS_SYSMENU = 0x00080000,
        WS_THICKFRAME = 0x00040000,
        WS_GROUP = 0x00020000,
        WS_TABSTOP = 0x00010000,
        WS_MINIMIZEBOX = 0x00020000,
        WS_MAXIMIZEBOX = 0x00010000,
    }

    /// <summary>
    /// Extended Windows Style flags
    /// </summary>
    [Flags]
    public enum ExtendedWindowStyleFlags : int
    {
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_TRANSPARENT = 0x00000020,

        WS_EX_MDICHILD = 0x00000040,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_CONTEXTHELP = 0x00000400,

        WS_EX_RIGHT = 0x00001000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,

        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_APPWINDOW = 0x00040000,

        WS_EX_LAYERED = 0x00080000,

        WS_EX_NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
        WS_EX_LAYOUTRTL = 0x00400000, // Right to left mirroring

        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_NOACTIVATE = 0x08000000
    }
}

namespace Utilities.WinControl.Mouse
{
    public static class M
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi;
            [FieldOffset(0)]
            public KEYBDINPUT ki;
            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }

        public enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        public enum IDC_STANDARD_CURSORS
        {
            IDC_ARROW = 32512,
            IDC_IBEAM = 32513,
            IDC_WAIT = 32514,
            IDC_CROSS = 32515,
            IDC_UPARROW = 32516,
            IDC_SIZE = 32640,
            IDC_ICON = 32641,
            IDC_SIZENWSE = 32642,
            IDC_SIZENESW = 32643,
            IDC_SIZEWE = 32644,
            IDC_SIZENS = 32645,
            IDC_SIZEALL = 32646,
            IDC_NO = 32648,
            IDC_HAND = 32649,
            IDC_APPSTARTING = 32650,
            IDC_HELP = 32651
        }

        [DllImport("user32.dll")]
        public static extern bool GetClipCursor(out RECT lpRect);

        [DllImport("user32.dll")]
        public static extern bool ClipCursor(ref RECT lpRect);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern IntPtr CreateCursor(IntPtr hInst, int xHotSpot, int yHotSpot,
           int nWidth, int nHeight, byte[] pvANDPlane, byte[] pvXORPlane);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursor(IntPtr hInstance, IDC_STANDARD_CURSORS lpCursorName);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string lpFileName); // 32x32 .cur file

        [DllImport("user32.dll")]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);
    }
}