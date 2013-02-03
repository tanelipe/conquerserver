
namespace GameServer
{
    public struct AttackTarget
    {
        public uint ID;
        public ulong Damage;
    }

    public unsafe struct AttackTargetPacket
    {
        public ushort Size;
        public ushort Type;
        public uint UID;
        public ushort X;
        public ushort Y;
        public ushort SpellID;
        public ushort SpellLevel;
        public uint Amount;
        public fixed byte Targets[1];
    }
}
