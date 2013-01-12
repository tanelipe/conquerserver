using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationServer
{
    public unsafe struct AuthRequest
    {
        public ushort Size;
        public ushort Type;
        public fixed sbyte Username[16];
        public fixed sbyte Password[16];
        public fixed sbyte Server[16];
    }
}
