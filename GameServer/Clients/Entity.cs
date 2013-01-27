using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public unsafe class Entity : IEntity<EntitySpawn>
    {
        public GameClient Owner;
        private Dictionary<ConquerStatusIDs, StatusUpdateEntry> PendingUpdates;


        private byte avatar;
        public byte Avatar
        {
            get { return avatar; }
            set
            {
                avatar = value;
                if (Owner.Status == LoginStatus.Complete)
                {
                    AddStatusUpdate(StatusUpdateEntry.Create(ConquerStatusIDs.Model, Model));
                }
            }
        }
        public ushort HairStyle;


        private uint money;
        public uint Money
        {
            get { return money; }
            set
            {
                money = value;

                if (Owner.Status == LoginStatus.Complete)
                {
                    AddStatusUpdate(StatusUpdateEntry.Create(ConquerStatusIDs.Money, value));
                }
            }
        }
        public uint Experience;

        public StatusPoints StatusPoints;
        public ushort HitPoints;
        public ushort ManaPoints;
        public ushort PKPoints;
        public byte Level;
        public byte Class;
        public byte Reborn;
        public string Name;
        public string Spouse;

        public Dictionary<ItemPosition, ConquerItem> Equipment;

        public uint Model { get { return (uint)(Avatar * 10000 + Mesh); } }

        public Entity(GameClient Owner)
        {
            this.Owner = Owner;

            Equipment = new Dictionary<ItemPosition, ConquerItem>();
            StatusPoints = new StatusPoints();
            PendingUpdates = new Dictionary<ConquerStatusIDs, StatusUpdateEntry>();
            Spouse = "NONE";       
        }

        public void AddEquipment(ConquerItem Item)
        {
            Equipment.ThreadSafeAdd(Item.Position, Item);
            Item.Send(Owner);
        }

        public void BeginStatusUpdates()
        {
            PendingUpdates.Clear();
        }
        public void AddStatusUpdate(StatusUpdateEntry Entry)
        {
            PendingUpdates.ThreadSafeAdd(Entry.Type, Entry);
        }
        public void EndStatusUpdates()
        {
            lock (PendingUpdates)
            {
                StatusUpdateEntry[] Entries = PendingUpdates.Values.ToArray();
                StatusUpdate* Update = PacketHelper.UpdatePacket(Entries);
                Update->UID = UID;
                Owner.Send(Update, Update->Size);
                Memory.Free(Update);

                PendingUpdates.Clear();
            }
        }  
        public override EntityFlag GetFlag()
        {
            return EntityFlag.Player;
        }
        public override EntitySpawn GetSpawnPacket()
        {
            return PacketHelper.EntitySpawn(this);
        }
    }
}
