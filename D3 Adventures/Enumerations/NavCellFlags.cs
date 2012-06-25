using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3_Adventures.Enumerations
{
    [Flags]
    public enum NavCellFlags
    {
        AllowFlier = 2,
        AllowGhost = 0x800,
        AllowProjectile = 0x400,
        AllowSpider = 4,
        AllowWalk = 1,
        LevelAreaBit0 = 8,
        LevelAreaBit1 = 0x10,
        NoNavMeshIntersected = 0x20,
        None = 0,
        NoSpawn = 0x40,
        RoundedCorner0 = 0x1000,
        RoundedCorner1 = 0x2000,
        RoundedCorner2 = 0x4000,
        RoundedCorner3 = 0x8000,
        Special0 = 0x80,
        Special1 = 0x100,
        SymbolNotFound = 0x200
    }
}
