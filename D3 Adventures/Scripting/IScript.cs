using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3_Adventures.Scripting
{
    public enum ScriptType
    {
        code, // string of code
        file // file path
    }

    interface IScript
    {
        //string EngineName;
        void Execute(string code);
    }
}
