using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using NetworkLibrary;
namespace GameServer
{

    public unsafe class GameClient
    {
        private WinsockClient Connection;
        private GameCryptography Crypto;
        public Entity Entity;
        public Screen Screen;
        public uint UID;


        public LoginStatus Status;
        private PacketQueue Queue;

        public List<ConquerItem> Inventory;

        public GameClient(WinsockClient Connection)
        {
            this.Connection = Connection;
   
            Crypto = new GameCryptography();
            Crypto.Initialize();
            Entity = new Entity(this);

            Queue = new PacketQueue();
            Screen = new Screen(this);
            Status = LoginStatus.Logging;

            Inventory = new List<ConquerItem>();
        }

        public PacketQueue Packets
        {
            get { return Queue; }
            set { Queue = value; }
        }
       
   
        public void Send(void* Packet, ushort Size)
        {
            byte[] tmp = new byte[Size];
            fixed (byte* pPacket = tmp)
            {
                Memory.Copy(Packet, pPacket, Size);
            }
            Send(tmp);
        }
        public void SendScreen(void* Packet, ushort Size, bool IncludeSelf = false)
        {
            if (IncludeSelf)
            {
                Send(Packet, Size);
            }

            Entity[] Entities = Screen.Players;
            foreach (Entity entity in Entities)
            {
                entity.Owner.Send(Packet, Size);
            }
        }
        public void Send(byte[] Packet)
        {
            lock (this)
            {
                Crypto.Encrypt(Packet);
                Connection.Send(Packet);
            }
        }
        public void Decrypt(byte[] Packet)
        {
            Crypto.Decrypt(Packet);
        }
        public void GenerateKeys(uint Key1, uint Key2)
        {
            Crypto.GenerateKeys(Key1, Key2);
        }
        public void Disconnect()
        {
            Connection.Disconnect();
        }

        public void Teleport(ushort MapID, ushort X, ushort Y)
        {
            RemoveEntity();

            GeneralData* Packet = PacketHelper.ConstructGeneralData();
            Packet->UID = Entity.UID;
            Packet->DataID = GeneralDataID.ChangeMap;

            Entity.Location.MapID = MapID;
            Entity.Location.X = X;
            Entity.Location.Y = Y;

            Packet->ValueA = X;
            Packet->ValueB = Y;
            Packet->ValueD_High = MapID;

            Send(Packet, Packet->Size);
            Memory.Free(Packet);
        }

        public void RemoveEntity()
        {
            GeneralData* Packet = PacketHelper.ConstructGeneralData();
            Packet->UID = Entity.UID;
            Packet->DataID = GeneralDataID.RemoveEntity;

            SendScreen(Packet, Packet->Size);
            Memory.Free(Packet);
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
    }
}
