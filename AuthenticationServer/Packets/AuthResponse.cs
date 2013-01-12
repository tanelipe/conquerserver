using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer
{
    public unsafe struct AuthResponse
    {
        public ushort Size;
        public ushort Type;
        public uint AccountID;
        public uint LoginToken;
        public fixed sbyte Address[16];
        public ushort Port;
    }
}
