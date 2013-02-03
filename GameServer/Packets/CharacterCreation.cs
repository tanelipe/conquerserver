
namespace GameServer
{
    public unsafe struct CharacterCreation
    {
        public ushort Size;
        public ushort Type;
        public fixed sbyte Account[16];
        public fixed sbyte Name[16];
        public fixed sbyte Password[16];
        public ushort Model;
        public ushort Class;
        public uint ID;
    }
}
