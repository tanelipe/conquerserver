
namespace GameServer
{
    public struct NpcInitialize
    {
        public ushort Size;
        public ushort Type;
        public uint UID;
        public uint Unknown;
        public ushort Mode;
        public ushort TypeDirection;
    }
}

/*
[Client -> Server Size: 0x0010 Type: 0x07EF]
10 00 EF 07 08 00 00 00 00 00 00 00 00 00 00 00  ; ..ï.............
*/