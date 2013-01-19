using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public class Entity : IEntity<EntitySpawn>
    {
        public GameClient Owner;

        public byte Avatar;
        public ushort HairStyle;
        public uint Gold;
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
