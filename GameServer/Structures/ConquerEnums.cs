using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public enum ConquerAngle : sbyte
    {
        Unknown = -1,
        SouthWest = 0,
        West = 1,
        NorthWest = 2,
        North = 3,
        NorthEast = 4,
        East = 5,
        SouthEast = 6,
        South = 7
    }

    public enum ConquerAction : byte
    {
        None = 0x00,
        Cool = 0xE6,
        Kneel = 0xD2,
        Sad = 0xAA,
        Happy = 0x96,
        Angry = 0xA0,
        Lie = 0x0E,
        Dance = 0x01,
        Wave = 0xBE,
        Bow = 0xC8,
        Sit = 0xFA,
        Stand = 0x64,
    }
}
