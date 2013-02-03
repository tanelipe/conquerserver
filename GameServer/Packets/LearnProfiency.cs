
namespace GameServer
{
    public struct LearnProfiency
    {
        public ushort Size;
        public ushort Type;
        public uint ID;
        public uint Level;
        public uint Experience;

        public static LearnProfiency Create()
        {
            LearnProfiency Packet = new LearnProfiency();
            Packet.Size = 0x10;
            Packet.Type = 0x401;
            return Packet;
        }
    }
}