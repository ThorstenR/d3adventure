using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace Utilities.WinControl
{
    #region EnumWindows
    /// <summary>
    /// EnumWindows wrapper for .NET
    /// </summary>
    public class EnumWindows
    {
        #region Delegates
        private delegate int EnumWindowsProc(IntPtr hwnd, int lParam);
        #endregion

        #region UnManagedMethods
        private class UnManagedMethods
        {
            [DllImport("user32")]
            public extern static int EnumWindows(
                EnumWindowsProc lpEnumFunc,
                int lParam);
            [DllImport("user32")]
            public extern static int EnumChildWindows(
                IntPtr hWndParent,
                EnumWindowsProc lpEnumFunc,
                int lParam);
        }
        #endregion

        #region Member Variables
        private EnumWindowsCollection items = null;
        #endregion

        /// <summary>
        /// Returns the collection of windows returned by
        /// GetWindows
        /// </summary>
        public EnumWindowsCollection Items
        {
            get
            {
                return this.items;
            }
        }

        /// <summary>
        /// Gets all top level windows on the system.
        /// </summary>
        public void GetWindows()
        {
            this.items = new EnumWindowsCollection();
            UnManagedMethods.EnumWindows(
                new EnumWindowsProc(this.WindowEnum),
                0);
        }
        /// <summary>
        /// Gets all child windows of the specified window
        /// </summary>
        /// <param name="hWndParent">Window Handle to get children for</param>
        public void GetWindows(
            IntPtr hWndParent)
        {
            this.items = new EnumWindowsCollection();
            UnManagedMethods.EnumChildWindows(
                hWndParent,
                new EnumWindowsProc(this.WindowEnum),
                0);
        }

        #region EnumWindows callback
        /// <summary>
        /// The enum Windows callback.
        /// </summary>
        /// <param name="hWnd">Window Handle</param>
        /// <param name="lParam">Application defined value</param>
        /// <returns>1 to continue enumeration, 0 to stop</returns>
        private int WindowEnum(
            IntPtr hWnd,
            int lParam)
        {
            if (this.OnWindowEnum(hWnd))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
        #endregion

        /// <summary>
        /// Called whenever a new window is about to be added
        /// by the Window enumeration called from GetWindows.
        /// If overriding this function, return true to continue
        /// enumeration or false to stop.  If you do not call
        /// the base implementation the Items collection will
        /// be empty.
        /// </summary>
        /// <param name="hWnd">Window handle to add</param>
        /// <returns>True to continue enumeration, False to stop</returns>
        protected virtual bool OnWindowEnum(
            IntPtr hWnd)
        {
            items.Add(hWnd);
            return true;
        }

        #region Constructor, Dispose
        public EnumWindows()
        {
            // nothing to do
        }
        #endregion
    }
    #endregion EnumWindows

    #region EnumWindowsCollection
    /// <summary>
    /// Holds a collection of Windows returned by GetWindows.
    /// </summary>
    public class EnumWindowsCollection : ReadOnlyCollectionBase
    {
        /// <summary>
        /// Add a new Window to the collection.  Intended for
        /// internal use by EnumWindows only.
        /// </summary>
        /// <param name="hWnd">Window handle to add</param>
        public void Add(IntPtr hWnd)
        {
            EnumWindowsItem item = new EnumWindowsItem(hWnd);
            this.InnerList.Add(item);
        }

        /// <summary>
        /// Gets the Window at the specified index
        /// </summary>
        public EnumWindowsItem this[int index]
        {
            get
            {
                return (EnumWindowsItem)this.InnerList[index];
            }
        }

        /// <summary>
        /// Constructs a new EnumWindowsCollection object.
        /// </summary>
        public EnumWindowsCollection()
        {
            // nothing to do
        }
    }
    #endregion

    #region EnumWindowsItem
    /// <summary>
    /// Provides details about a Window returned by the 
    /// enumeration
    /// </summary>
    public class EnumWindowsItem
    {
        #region Structures
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        private struct FLASHWINFO
        {
            public int cbSize;
            public IntPtr hwnd;
            public int dwFlags;
            public int uCount;
            public int dwTimeout;
        }
        #endregion

        #region UnManagedMethods
        private class UnManagedMethods
        {
            [DllImport("user32")]
            public extern static int IsWindowVisible(
                IntPtr hWnd);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public extern static int GetWindowText(
                IntPtr hWnd,
                StringBuilder lpString,
                int cch);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public extern static int GetWindowTextLength(
                IntPtr hWnd);
            [DllImport("user32")]
            public extern static int BringWindowToTop(IntPtr hWnd);
            [DllImport("user32")]
            public extern static int SetForegroundWindow(IntPtr hWnd);
            [DllImport("user32")]
            public extern static int IsIconic(IntPtr hWnd);
            [DllImport("user32")]
            public extern static int IsZoomed(IntPtr hwnd);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public extern static int GetClassName(
                IntPtr hWnd,
                StringBuilder lpClassName,
                int nMaxCount);
            [DllImport("user32")]
            public extern static int FlashWindow(
                IntPtr hWnd,
                ref FLASHWINFO pwfi);
            [DllImport("user32")]
            public extern static int GetWindowRect(
                IntPtr hWnd,
                ref RECT lpRect);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public extern static int SendMessage(
                IntPtr hWnd,
                int wMsg,
                IntPtr wParam,
                IntPtr lParam);
            [DllImport("user32", CharSet = CharSet.Auto)]
            public extern static uint GetWindowLong(
                IntPtr hwnd,
                int nIndex);
            public const int WM_COMMAND = 0x111;
            public const int WM_SYSCOMMAND = 0x112;

            public const int SC_RESTORE = 0xF120;
            public const int SC_CLOSE = 0xF060;
            public const int SC_MAXIMIZE = 0xF030;
            public const int SC_MINIMIZE = 0xF020;

            public const int GWL_STYLE = (-16);
            public const int GWL_EXSTYLE = (-20);

            /// <summary>
            /// Stop flashing. The system restores the window to its original state.
            /// </summary>
            public const int FLASHW_STOP = 0;
            /// <summary>
            /// Flash the window caption. 
            /// </summary>
            public const int FLASHW_CAPTION = 0x00000001;
            /// <summary>
            /// Flash the taskbar button.
            /// </summary>
            public const int FLASHW_TRAY = 0x00000002;
            /// <summary>
            /// Flash both the window caption and taskbar button.
            /// </summary>
            public const int FLASHW_ALL = (FLASHW_CAPTION | FLASHW_TRAY);
            /// <summary>
            /// Flash continuously, until the FLASHW_STOP flag is set.
            /// </summary>
            public const int FLASHW_TIMER = 0x00000004;
            /// <summary>
            /// Flash continuously until the window comes to the foreground. 
            /// </summary>
            public const int FLASHW_TIMERNOFG = 0x0000000C;
        }
        #endregion

        /// <summary>
        /// The window handle.
        /// </summary>
        private IntPtr hWnd = IntPtr.Zero;

        /// <summary>
        /// To allow items to be compared, the hash code
        /// is set to the Window handle, so two EnumWindowsItem
        /// objects for the same Window will be equal.
        /// </summary>
        /// <returns>The Window Handle for this window</returns>
        public override System.Int32 GetHashCode()
        {
            return (System.Int32)this.hWnd;
        }

        /// <summary>
        /// Gets the window's handle
        /// </summary>
        public IntPtr Handle
        {
            get
            {
                return this.hWnd;
            }
        }

        /// <summary>
        /// Gets the window's title (caption)
        /// </summary>
        public string Text
        {
            get
            {
                StringBuilder title = new StringBuilder(260, 260);
                UnManagedMethods.GetWindowText(this.hWnd, title, title.Capacity);
                return title.ToString();
            }
        }

        /// <summary>
        /// Gets the window's class name.
        /// </summary>
        public string ClassName
        {
            get
            {
                StringBuilder className = new StringBuilder(260, 260);
                UnManagedMethods.GetClassName(this.hWnd, className, className.Capacity);
                return className.ToString();
            }
        }

        /// <summary>
        /// Gets/Sets whether the window is iconic (mimimised) or not.
        /// </summary>
        public bool Iconic
        {
            get
            {
                return ((UnManagedMethods.IsIconic(this.hWnd) == 0) ? false : true);
            }
            set
            {
                UnManagedMethods.SendMessage(
                    this.hWnd,
                    UnManagedMethods.WM_SYSCOMMAND,
                    (IntPtr)UnManagedMethods.SC_MINIMIZE,
                    IntPtr.Zero);
            }
        }

        /// <summary>
        /// Gets/Sets whether the window is maximised or not.
        /// </summary>
        public bool Maximised
        {
            get
            {
                return ((UnManagedMethods.IsZoomed(this.hWnd) == 0) ? false : true);
            }
            set
            {
                UnManagedMethods.SendMessage(
                    this.hWnd,
                    UnManagedMethods.WM_SYSCOMMAND,
                    (IntPtr)UnManagedMethods.SC_MAXIMIZE,
                    IntPtr.Zero);
            }
        }

        /// <summary>
        /// Gets whether the window is visible.
        /// </summary>
        public bool Visible
        {
            get
            {
                return ((UnManagedMethods.IsWindowVisible(this.hWnd) == 0) ? false : true);
            }
        }

        /// <summary>
        /// Gets the bounding rectangle of the window
        /// </summary>
        public System.Drawing.Rectangle Rect
        {
            get
            {
                RECT rc = new RECT();
                UnManagedMethods.GetWindowRect(
                    this.hWnd,
                    ref rc);
                System.Drawing.Rectangle rcRet = new System.Drawing.Rectangle(
                    rc.Left, rc.Top,
                    rc.Right - rc.Left, rc.Bottom - rc.Top);
                return rcRet;
            }
        }

        /// <summary>
        /// Gets the location of the window relative to the screen.
        /// </summary>
        public System.Drawing.Point Location
        {
            get
            {
                System.Drawing.Rectangle rc = Rect;
                System.Drawing.Point pt = new System.Drawing.Point(
                    rc.Left,
                    rc.Top);
                return pt;
            }
        }

        /// <summary>
        /// Gets the size of the window.
        /// </summary>
        public System.Drawing.Size Size
        {
            get
            {
                System.Drawing.Rectangle rc = Rect;
                System.Drawing.Size sz = new System.Drawing.Size(
                    rc.Right - rc.Left,
                    rc.Bottom - rc.Top);
                return sz;
            }
        }

        /// <summary>
        /// Restores and Brings the window to the front, 
        /// assuming it is a visible application window.
        /// </summary>
        public void Restore()
        {
            if (Iconic)
            {
                UnManagedMethods.SendMessage(
                    this.hWnd,
                    UnManagedMethods.WM_SYSCOMMAND,
                    (IntPtr)UnManagedMethods.SC_RESTORE,
                    IntPtr.Zero);
            }
            UnManagedMethods.BringWindowToTop(this.hWnd);
            UnManagedMethods.SetForegroundWindow(this.hWnd);
        }

        public WindowStyleFlags WindowStyle
        {
            get
            {
                return (WindowStyleFlags)UnManagedMethods.GetWindowLong(
                    this.hWnd, UnManagedMethods.GWL_STYLE);
            }
        }

        public ExtendedWindowStyleFlags ExtendedWindowStyle
        {
            get
            {
                return (ExtendedWindowStyleFlags)UnManagedMethods.GetWindowLong(
                    this.hWnd, UnManagedMethods.GWL_EXSTYLE);
            }
        }

        /// <summary>
        ///  Constructs a new instance of this class for
        ///  the specified Window Handle.
        /// </summary>
        /// <param name="hWnd">The Window Handle</param>
        public EnumWindowsItem(IntPtr hWnd)
        {
            this.hWnd = hWnd;
        }
    }
    #endregion
}