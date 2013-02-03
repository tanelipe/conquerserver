using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NetworkLibrary;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer
{
    public unsafe class GameClient
    {
        private WinsockClient Socket;
        private GameCryptography Cryptography;
        private PacketQueue PacketQueue;
        private object SendLock;

        private List<ConquerItem> Inventory;
        private Dictionary<ItemPosition, ConquerItem> Equipment;

        public GameClient(WinsockClient Socket)
        {
            this.Socket = Socket;

            SendLock = new object();

            Cryptography = new GameCryptography();
            Cryptography.Initialize();

            Entity = new Entity(this);
            Screen = new Screen(this);
            PacketQueue = new PacketQueue();
            Inventory = new List<ConquerItem>();

            Equipment = new Dictionary<ItemPosition, ConquerItem>();
            
            Status = LoginStatus.Logging;
        }
        
        public Entity Entity { get; set; }
        public Screen Screen { get; set; }
        public uint UID { get; set; }
        public uint ActiveNPC { get; set; }
        public LoginStatus Status { get; set; }

        public PacketQueue Packets
        {
            get { return PacketQueue; }
            set { PacketQueue = value; }
        }


        public void Send(void* pPacket, ushort Size)
        {
            byte[] tmp = new byte[Size];
            fixed (byte* Packet = tmp)
            {
                Memory.Copy(pPacket, Packet, Size);
                Send(tmp);
            }
            tmp = null;
        }
        public void Send(byte[] Packet)
        {
            lock (SendLock)
            {
                Cryptography.Encrypt(Packet);
                Socket.Send(Packet);
            }
        }
        public void SendScreen(void* Packet, ushort Size, bool Self = false)
        {
            if (Self)
            {
                Send(Packet, Size);
            }

            Entity[] Entities = Screen.Players;
            Parallel.ForEach(Entities, (entity) =>
            {
                entity.Owner.Send(Packet, Size);
            });            
        }

        public void Decrypt(byte[] Packet)
        {
            Cryptography.Decrypt(Packet);
        }
        public void GenerateKeys(uint Key1, uint Key2)
        {
            Cryptography.GenerateKeys(Key1, Key2);
        }
        public void Disconnect()
        {
            Socket.Disconnect();
        }


        public void RemoveFromScreen()
        {
            GeneralData* Packet = PacketHelper.AllocGeneral();

            Packet->UID = Entity.UID;
            Packet->DataID = GeneralDataID.RemoveEntity;
            SendScreen(Packet, Packet->Size);

            Entity[] Entities = Screen.Players;
            Parallel.ForEach(Entities, (entity) =>
            {
                entity.Owner.Screen.Remove(Entity);
            }); 

            Memory.Free(Packet);
        }

        public void Teleport(ushort MapID, ushort X, ushort Y)
        {

            if (Kernel.IsWalkable(MapID, X, Y))
            {
                GeneralData* Packet = PacketHelper.AllocGeneral();

                if (Entity.Location.MapID == MapID && ConquerMath.CalculateDistance(X, Y, Entity.Location.X, Entity.Location.Y, true) < Kernel.ScreenView)
                {
                    // Teleporting within Kernel.ScreenView distance. Don't remove entity from other players. Send "Jump" packet instead
                    Packet->DataID = GeneralDataID.Jump;
                    Packet->UID = Entity.UID;
                    Packet->ValueA = Entity.Location.X;
                    Packet->ValueB = Entity.Location.Y;
                    Packet->ValueD_High = X;
                    Packet->ValueD_Low = Y;

                    Entity.Location.MapID = MapID;
                    Entity.Location.X = X;
                    Entity.Location.Y = Y;
                    SendScreen(Packet, Packet->Size, true);
                }
                else
                {
                    Packet->UID = Entity.UID;
                    Packet->DataID = GeneralDataID.RemoveEntity;
                    SendScreen(Packet, Packet->Size);
                }
                Packet = PacketHelper.ReAllocGeneral(Packet);
                Packet->UID = Entity.UID;
                Packet->DataID = GeneralDataID.ChangeMap;

                Packet->ValueA = X;
                Packet->ValueB = Y;
                Packet->ValueD_High = MapID;

                Entity.Location.MapID = MapID;
                Entity.Location.X = X;
                Entity.Location.Y = Y;

                Send(Packet, Packet->Size);
                Memory.Free(Packet);
            }
            else
            {
                Message(string.Format("Can't teleport to MapID: {0} X {1} Y {2}.", MapID, X, Y), ChatType.Center, Color.White);
                return;
            }
        }
        public void Message(string Message, ChatType Type, Color Color, string From = "SYSTEM", string To = "ALLUSERS")
        {
            Chat* Packet = PacketHelper.CreateChat(From, To, Message);
            Packet->ChatType = Type;
            Packet->ID = Entity.UID;
            Packet->Color = (uint)Color.ToArgb();
            Send(Packet, Packet->Size);
            Memory.Free(Packet);
        }
        public bool AddInventory(ConquerItem Item)
        {
            lock (Inventory)
            {
                if (Inventory.Count < 40)
                {
                    Item.Position = ItemPosition.Inventory;
                    Item.Send(this);
                    Inventory.Add(Item);
                    return true;
                }
                return false;
            }
        }
        public bool RemoveInventory(ConquerItem Item)
        {
            bool Removed = false;
            lock (Inventory)
            {
                Removed = Inventory.Remove(Item);
                if (Removed)
                {
                    Item.RemoveInventory(this);
                }
   
            }
            return Removed;
        }
        public bool TryGetInventory(uint UID, out ConquerItem Item)
        {
            Item = default(ConquerItem);
            lock (Inventory)
            {
                Item = Inventory.Find((item) => { return item.UID == UID; });
                if (Item != null)
                    return true;
            }
            return false;
        }
        public bool AddEquipment(ConquerItem Item, ItemPosition Position)
        {
            Item.Position = Position;
            Equipment.ThreadSafeAdd(Position, Item);
            Item.Send(this);
            return true;
        }
        public bool TryGetEquipment(ItemPosition Position, out ConquerItem Item)
        {
            Item = default(ConquerItem);
            if (Equipment.ContainsKey(Position))
            {
                Item = Equipment[Position];
                return true;
            }
            return false;
        }
        public void Unequip(ConquerItem Item, ItemPosition Position)
        {
            Item.Position = Position;
            Item.Unequip(this);
            Equipment.ThreadSafeRemove(Position);
        }
    }
}
