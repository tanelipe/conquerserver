using GameServer.Database;
using System.Drawing;
namespace GameServer
{

    public unsafe class ItemUsageProcessor : IPacketProcessor
    {
        public ItemUsageProcessor(DatabaseManager Database)
            : base(Database)
        {

        }

        public override void Execute(GameClient Client, byte* pPacket)
        {
            ItemUsage* Packet = (ItemUsage*)pPacket;
            switch (Packet->UsageID)
            {
                case ItemUsageIDs.Ping: Client.Send(Packet, Packet->Size); break;
                case ItemUsageIDs.Equip: HandleEquip(Client, Packet); break;
                case ItemUsageIDs.UnequipItem: HandleUnequip(Client, Packet); break;
            }
        }
        private unsafe void HandleUnequip(GameClient Client, ItemUsage* Packet)
        {
            ItemPosition Position = (ItemPosition)Packet->Location;
            ConquerItem Item;

            if (Client.TryGetEquipment(Position, out Item))
            {
                if (Client.AddInventory(Item))
                {
                    Client.Unequip(Item, Position);
                }
                else
                {
                    Client.Message("Inventory is full!", ChatType.Top, Color.Red);
                }
            }
        }
        private unsafe void HandleEquip(GameClient Client, ItemUsage* Packet)
        {
            ConquerItem Item;
            if (Client.TryGetInventory(Packet->ID, out Item))
            {
                Item.Position = (ItemPosition)Packet->Location;
                Client.AddEquipment(Item, Item.Position);
                Client.RemoveInventory(Item);
            }
        }
    }

}
