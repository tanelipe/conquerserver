
using System.Runtime.InteropServices;
namespace GameServer
{
    public struct EntityItems
    {
        public uint Helmet;
        public uint Armor;
        public uint RightHand;
        public uint LeftHand;
    }
    

    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct EntitySpawn
    {
        [FieldOffset(0)]
        public ushort Size;
        [FieldOffset(2)]
        public ushort Type;
        [FieldOffset(4)]
        public uint UID;
        [FieldOffset(8)]
        public uint Mesh;
        [FieldOffset(12)]
        public uint Status;
        [FieldOffset(14)]
        public ushort GuildID;
        [FieldOffset(16)]
        public ushort GuildRank;
        [FieldOffset(20)]
        public EntityItems Items;
        [FieldOffset(44)]
        public ushort X;
        [FieldOffset(46)]
        public ushort Y;
        [FieldOffset(48)]
        public ushort HairStyle;
        [FieldOffset(50)]
        public ConquerAngle Angle;
        [FieldOffset(51)]
        public ConquerAction Action;
        [FieldOffset(52)]
        public bool ShowNames;
        [FieldOffset(53)]
        public fixed sbyte Name[17];
    }
}
