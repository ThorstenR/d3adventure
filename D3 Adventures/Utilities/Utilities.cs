using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using System.DirectoryServices;
using System.Management;
using System.Security.Principal;

namespace D3_Adventures
{
    public class Utilities
    {
        public static int GetProcessID(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);
            if (p.Length == 0)
                return -1;
            else
                return p[0].Id;
        }

        public static IntPtr GetProcessHandle(string processName)
        {
            Process[] p = Process.GetProcessesByName(processName);
            if (p.Length == 0)
                return IntPtr.Zero;
            else
                return p[0].MainWindowHandle;
        }

        public static bool isAdmin(string processName)
        {

            string ProcessOwner = "";
            string ProcessDomain = "";

            System.Management.ObjectQuery x = new System.Management.ObjectQuery("Select * From Win32_Process where Name='" + processName + ".exe" + "'");
            System.Management.ManagementObjectSearcher mos = new System.Management.ManagementObjectSearcher(x);
            foreach (System.Management.ManagementObject mo in mos.Get())
            {
                string[] s = new string[2];
                mo.InvokeMethod("GetOwner", (object[])s);
                ProcessOwner = s[0].ToString();
                ProcessDomain = s[1].ToString();
                break;
            }

            string userPath = ProcessDomain + "/" + ProcessOwner;

            using (DirectoryEntry groupEntry = new DirectoryEntry("WinNT://./Administrators,group"))
            {
                foreach (object member in (IEnumerable)groupEntry.Invoke("Members"))
                {
                    using (DirectoryEntry memberEntry = new DirectoryEntry(member))
                    {
                        if (Globals.debugMessages) Console.WriteLine(memberEntry.Path);
                        if (memberEntry.Path.Contains(userPath))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;

        }



    }
}
