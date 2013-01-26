namespace GameServer
{
    public struct NpcSpawn
    {
        public ushort Size; // 0
        public ushort Type; // 2
        public uint UID; // 4
        public ushort X; // 8
        public ushort Y; // 10
        public ushort SubType; // 12
        public ushort Interaction; // 14
        public ushort Direction;
    }
}
