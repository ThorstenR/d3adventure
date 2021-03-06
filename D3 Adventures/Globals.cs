﻿/*
 *  Original Credits goto UnknOwned
 *      http://www.ownedcore.com/forums/diablo-3/diablo-3-bots-programs/diablo-3-memory-editing/356250-autoit-diablo-3-click-move-interaction-actor-indexing.html
 *  Conversion by UserNamex32
 *      http://code.google.com/p/d3adventure/
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;

using D3_Adventures.Memory_Handling;
using D3_Adventures.Structures;
using System.IO;
using Utilities.ProcessTools;

// need to do proper documentation 
//  http://msdn.microsoft.com/en-us/library/b2s063f7.aspx

namespace D3_Adventures
{
    public static class Globals
    {
        // Fields
        public static bool debugMessages = false;
        public static string exeName = "Diablo III";
        public static IntPtr winHandle = PT.GetProcessHandle("Diablo III");

        public static MemoryManager mem = new MemoryManager(winHandle);
        public static SNOReader SNO = new SNOReader();
        public static bool screwWarden = false;
        public static Actor Me
        {
            get
            {
                return Data.GetMe();
            }
        }
    }


}
