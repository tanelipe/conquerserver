using System.Runtime.InteropServices;
namespace GameServer
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ItemInformation
    {
        [FieldOffset(0)]
        public ushort Size;
        [FieldOffset(2)]
        public ushort Type;
        [FieldOffset(4)]
        public uint UID;
        [FieldOffset(8)]
        public uint ID;
        [FieldOffset(12)]
        public ushort CurrentDurability;
        [FieldOffset(14)]
        public ushort MaxDurability;
        [FieldOffset(16)]
        public ItemMode Mode;
        [FieldOffset(18)]
        public ItemPosition Location;
        [FieldOffset(19)]
        public byte SocketOne;
        [FieldOffset(20)]
        public byte SocketTwo;
        [FieldOffset(23)]
        public byte Plus;

        public static ItemInformation Create()
        {
            ItemInformation Packet = new ItemInformation();
            Packet.Size = 24;
            Packet.Type = 0x3F0;
            Packet.Mode = ItemMode.Default;
            return Packet;
        }
    }
    public enum ItemMode : ushort
    {
        Default = 0x01,
        VendByGold = 0x01,
        Trade = 0x02,
        Update = 0x02,
        VendByCps = 0x03,
        View = 0x04
    }
}
