namespace GameServer
{
    public struct ItemUsage
    {
        public ushort Size;
        public ushort Type;
        public uint ID;
        public uint Location;
        public ItemUsageIDs UsageID;
        public uint Timer;
    }
    public enum ItemUsageIDs : uint
    {
        BuyItem = 1,
        SellItem = 2,
        RemoveInventory = 3,
        Equip = 4,
        UpdateItem = 5,
        UnequipItem = 6,
        ViewWarehouse = 9,
        DepositCash = 10,
        WithdrawCash = 11,
        DropMoney = 12,
        DBUpgrade = 19,
        MetUpgrade = 20,
        ShowVendingList = 0x15,
        AddVendingItemGold = 0x16,
        RemoveVendingItem = 0x17,
        BuyVendingItem = 0x18,
        UpdateArrowCount = 25,
        Fireworks = 26,
        Ping = 27,
        Repair = 14
    }
}
