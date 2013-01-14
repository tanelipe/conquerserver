using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Structures;
namespace GameServer
{
    public enum EntityFlag
    {
        Player,
        Monster,
        NPC
    }
    public struct StatusPoints
    {
        public ushort Strength;
        public ushort Dexterity;
        public ushort Vitality;
        public ushort Spirit;
        public ushort Free;
    }
    public class Entity
    {
        public GameClient Owner;

        public uint UID;
        public Location Location;
        public EntityFlag Flag;

        public byte Avatar;
        public ushort Mesh;

        public uint Model { get { return (uint)(Avatar * 10000 + Mesh); } }
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

        public Entity(GameClient Client)
        {
            Owner = Client;

            Location = new Location();
            Location.MapID = 1002;
            Location.X = 400;
            Location.Y = 400;

            StatusPoints = new StatusPoints();
            Spouse = "NONE";
        }
    }
}
