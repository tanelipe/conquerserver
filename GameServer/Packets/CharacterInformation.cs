using System.Runtime.InteropServices;

namespace GameServer
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe struct CharacterInformation
    {
        [FieldOffset(0)]
        public ushort Size;
        [FieldOffset(2)]
        public ushort Type;
        [FieldOffset(4)]
        public uint ID;
        [FieldOffset(8)]
        public uint Model;
        [FieldOffset(12)]
        public ushort HairStyle;
        [FieldOffset(16)]
        public uint Gold;
        [FieldOffset(20)]
        public uint Experience;
        [FieldOffset(40)]
        public ushort Strength;
        [FieldOffset(42)]
        public ushort Dexterity;
        [FieldOffset(44)]
        public ushort Vitality;
        [FieldOffset(46)]
        public ushort Spirit;
        [FieldOffset(48)]
        public ushort StatPoints;
        [FieldOffset(50)]
        public ushort HitPoints;
        [FieldOffset(52)]
        public ushort ManaPoints;
        [FieldOffset(54)]
        public ushort PKPoints;
        [FieldOffset(56)]
        public byte Level;
        [FieldOffset(57)]
        public byte Class;
        [FieldOffset(59)]
        public byte Reborn;
        [FieldOffset(60)]
        public bool DisplayName;
        [FieldOffset(61)]
        public byte NameCount;
        [FieldOffset(62)]
        public fixed byte Names[34];
    }
}
