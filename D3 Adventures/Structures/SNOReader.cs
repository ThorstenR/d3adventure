
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using D3_Adventures.Enumerations;
using D3_Adventures.Injector;
using System.Runtime.InteropServices;
using D3_Adventures.Enumerations;

namespace D3_Adventures.Structures
{
    public class SNOReader
    {
        private static readonly Dictionary<Tuple<SNO.SNOGroup, int>, string> dictionary_0 = new Dictionary<Tuple<SNO.SNOGroup, int>, string>();
        private readonly Dictionary<SNO.ClientSNOTable, SNOTable> dictionary_1 = new Dictionary<SNO.ClientSNOTable, SNOTable>();

        internal SNOReader()
        {
            foreach (string str in Enum.GetNames(typeof(SNO.ClientSNOTable)))
            {
                SNO.ClientSNOTable table;
                Enum.TryParse<SNO.ClientSNOTable>(str, false, out table);
                IntPtr zero = IntPtr.Zero;
                switch (table)
                {
                    case SNO.ClientSNOTable.Actor:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.Worlds:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.Monster:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.Act:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.Power:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.Scene:
                        zero = (IntPtr)0x15f2190;
                        break;

                    case SNO.ClientSNOTable.MarkerSet:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.Quest:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.SkillKit:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.StringList:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;

                    case SNO.ClientSNOTable.GameBalance:
                        zero = (IntPtr)Offsets.SNOActors;
                        break;
                }
                this.dictionary_1.Add(table, new SNOTable(zero, table));
            }
        }

        public string LookupSNOName(SNO.SNOGroup snoGroup, int snoId)
        {
            string str;
            if (dictionary_0.TryGetValue(new Tuple<SNO.SNOGroup, int>(snoGroup, snoId), out str))
            {
                return str;
            }
            using (MemoryInjector class2 = new MemoryInjector(Globals.mem, 20))
            {
                Globals.mem.Injector.CallFunction(Offsets.gnGetDisplayedNameForSnoId, CallingConvention.Cdecl, new object[] { snoId, (uint)class2.Address, (uint)snoGroup });
                str = Globals.mem.ReadMemoryAsString((uint)Globals.mem.ReadMemory<IntPtr>(class2.Address + 4), 0x200, Encoding.UTF8);
                dictionary_0.Add(new Tuple<SNO.SNOGroup, int>(snoGroup, snoId), str);
                return str;
            }
        }

        public SNOTable this[SNO.ClientSNOTable snoGroup]
        {
            get
            {
                if (!this.dictionary_1.ContainsKey(snoGroup))
                {
                    throw new Exception(string.Format("Couldn't find SNOGroup {0} in SNOTableCache!", snoGroup));
                }
                return this.dictionary_1[snoGroup];
            }
        }
    }
}

