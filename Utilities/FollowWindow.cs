using System;
using System.Threading;
using System.Diagnostics;
using Utilities.WinControl;

/// Threaded window following system
/// Created by CTS_AE
/// Able to decide which window follows which
/// Allows Offsets with handle location
/// Updated from AoMapClient

namespace Utilities.FollowWindow
{
    public enum WindowHandlingLocation
    {
        TopLeft2mid,
        TopRight2mid,
        BottomLeft2mid,
        BottomRight2mid
    }

    public class FW
    {
        IntPtr ParentHwnd;
        IntPtr ExternalHwnd;
        int offsetX = 0;
        int offsetY = 0;
        public Thread Follow;
        public bool Following;
        public bool QuickStop;
        public bool followsParent;
        public WindowHandlingLocation winHanLocation = WindowHandlingLocation.TopLeft2mid;

        public FW(IntPtr ParentHwnd, IntPtr ExternalHwnd, bool followsParrent)
        {
            this.ParentHwnd = ParentHwnd;
            this.ExternalHwnd = ExternalHwnd;
            this.followsParent = followsParrent;
        }

        public FW(IntPtr ParentHwnd, IntPtr ExternalHwnd, bool followsParrent, WindowHandlingLocation winHanLocation)
        {
            this.ParentHwnd = ParentHwnd;
            this.ExternalHwnd = ExternalHwnd;
            this.followsParent = followsParrent;
            this.winHanLocation = winHanLocation;
        }

        public void Follower()
        {
            //EnumWindowsItem child = WC.GetChildWindow("Map", "");

            //if (child == null)
            //{
            //    return;
            //}

            while (WC.Hwnd2ID(ParentHwnd) != 0) // old way didn't work, since the handle wasn't to the child window
            {
                try
                {
                    int px = WC.winGetPosition(ParentHwnd).X;
                    int py = WC.winGetPosition(ParentHwnd).Y;
                    int pw = WC.winGetSize(ParentHwnd).Width;
                    int ph = WC.winGetSize(ParentHwnd).Height;

                    int x = WC.winGetPosition(ExternalHwnd).X;
                    int y = WC.winGetPosition(ExternalHwnd).Y;
                    int w = WC.winGetSize(ExternalHwnd).Width;
                    int h = WC.winGetSize(ExternalHwnd).Height;
                    
                    if (followsParent)
                    {    
                        switch (winHanLocation) // hopefully works not tested
                        {    
                            case WindowHandlingLocation.TopLeft2mid:
                                WC.winSetPosition(ExternalHwnd, px + offsetX, py + offsetY);
                                break;
                            case WindowHandlingLocation.TopRight2mid:
                                WC.winSetPosition(ExternalHwnd, px + pw - w - offsetX, py + offsetY);
                                break;
                            case WindowHandlingLocation.BottomLeft2mid:
                                WC.winSetPosition(ExternalHwnd, px + offsetX, py + ph - h - offsetY);
                                break;
                            case WindowHandlingLocation.BottomRight2mid:
                                WC.winSetPosition(ExternalHwnd, px + pw - w - offsetX, py + ph - h - offsetY);
                                break;
                        }
                    }
                    else
                    {
                        switch (winHanLocation)
                        {  
                            case WindowHandlingLocation.TopLeft2mid:
                                
                                WC.winSetPosition(ParentHwnd, x + offsetX, y + offsetY);
                                break;
                            case WindowHandlingLocation.TopRight2mid:
                                WC.winSetPosition(ParentHwnd, x + w - pw - offsetX, y + offsetY);
                                break;
                            case WindowHandlingLocation.BottomLeft2mid:
                                WC.winSetPosition(ParentHwnd, x + offsetX, y + h - ph - offsetY);
                                break;
                            case WindowHandlingLocation.BottomRight2mid:
                                WC.winSetPosition(ParentHwnd, x + w - pw - offsetX, y + h - ph - offsetY);
                                break;
                        }
                    }
                }
                catch { Toggle(false); }
                Thread.Sleep(333);
            }
        }

        public void Start()
        {
            if (Follow == null)
            {
                Follow = new Thread(new ThreadStart(Follower));
                Follow.Name = "Follow Thread1";
                Follow.Start();
            }
            else if (Follow.ThreadState == System.Threading.ThreadState.Stopped)
            {
                Follow = new Thread(new ThreadStart(Follower));
                Follow.Name = "Follow Thread2";
                Follow.Start();
            }
            Following = true;
            QuickStop = false;
        }

        public void Toggle(bool On)
        {
            if (On)
            {
                this.Start();
            }
            else
            {
                Follow.Abort();
                Following = false;
            }
        }

        public void Offset(int x, int y)
        {
            offsetX = x;
            offsetY = y;
        }

        public void Offset(System.Drawing.Point p)
        {
            offsetX = p.X;
            offsetY = p.Y;
        }

    }
}
