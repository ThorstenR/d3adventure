using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3_Adventures.Internals;

namespace D3_Adventures.Structures
{
    struct NLabel
    {
        public IntPtr pntCharArr;
        public int int_0;
        public int int_1;
        public int int_2;
        public int length;
        public int password;
    }
    public class Label
    {
        private uint BaseAdress { get; set; }
        private NLabel InternalStructure { get; set; }
        public bool IsPassword { get { return InternalStructure.password == 1; } }
        public int Length { get { return InternalStructure.length; } }
        public String Text
        {
            get
            {
                byte[] buffer;
                try
                {
                    if (Globals.mem.ReadMemory(InternalStructure.pntCharArr, Length + 1, out buffer))
                    {
                        return buffer.ToUTF8String();
                    }
                    else
                        return string.Empty;
                }
                catch { return string.Empty; }
                    //throw new Exception("Error while reading content of string");
            }
        }

        internal Label(uint baseadre)
        {
            BaseAdress = baseadre;
            object obj = Globals.mem.ReadMemory(BaseAdress, typeof(NLabel));
            if (obj != null)
                InternalStructure = (NLabel)obj;
            
        }
    }
}
