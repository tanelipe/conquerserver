
namespace GameServer
{
    public struct LearnSpell
    {
        public ushort Size;
        public ushort Type;
        public uint Experience;
        public ushort ID;
        public ushort Level;

        public static LearnSpell Create()
        {
            LearnSpell Packet = new LearnSpell();
            Packet.Size = 0x0C;
            Packet.Type = 0x44F;
            return Packet;
        }
    }
}
