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

        private byte avatar;
        public byte Avatar
        {
            get { return avatar; }
            set
            {
                avatar = value;
                if (Owner.Status == LoginStatus.Complete)
                {
                    StatusUpdateEntry Entry = new StatusUpdateEntry()
                    {
                        Type = ConquerStatusIDs.Model,
                        Value = Model
                    };

                    StatusUpdate* Update = PacketHelper.UpdatePacket(Entry);
                    Update->UID = UID;
                    Owner.SendScreen(Update, Update->Size, true);
                    Memory.Free(Update);
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
                    StatusUpdate* Update = PacketHelper.UpdatePacket(StatusUpdateEntry.Gold(money));
                    Update->UID = UID;
                    Owner.Send(Update, Update->Size);
                    Memory.Free(Update);
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

            Spouse = "NONE";       
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
