
namespace GameServer
{
    public enum ItemPosition : byte
    {
        Inventory = 0,
        Headgear = 1,
        Necklace = 2,
        Armor = 3,
        Right = 4,
        Left = 5,
        Ring = 6,
        Boots = 8,
    }
    public class ConquerItem
    {
        public uint UID { get; set; }
        public uint ID { get; set; }
        public byte Plus { get; set; }
        public byte SocketOne { get; set; }
        public byte SocketTwo { get; set; }
        public ushort Mode { get; set; }
        public ushort Durability { get; set; }
        public ushort MaxDurability { get; set; }
        public ushort Effect { get; set; }
        public ItemPosition Position { get; set; }


        private const uint UID_Start = 100000000;
        private const uint UID_Reset = 900000000;
        private static uint NextUID = UID_Start;

        public ConquerItem()
        {
            UID = NextUID++;
            if (NextUID >= UID_Reset)
                NextUID = UID_Start;
        }
    }
}