using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Structures
{
    public struct Location
    {
        public ushort X;
        public ushort Y;
        public ushort MapID;

        public override string ToString()
        {
            return MapID + " " + X + " " + Y;
        }
    }
}
