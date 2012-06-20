using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3_Adventures.Scripting
{
    interface IScript
    {
        //string EngineName;
        void Execute(string code);
    }
}
