
namespace GameServer
{
    public unsafe struct AuthMessage
    {
        public ushort Size;
        public ushort Type;
        public uint AccountID;
        public uint LoginToken;
        public fixed sbyte Message[16];
    }
}
