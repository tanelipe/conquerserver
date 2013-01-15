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

        public void Walk(ConquerAngle Angle)
        {
            int dx = 0, dy = 0;
            switch (Angle)
            {
                case ConquerAngle.North: dx = -1; dy = -1; break;
                case ConquerAngle.South: dx = 1; dy = 1; break;
                case ConquerAngle.East: dx = 1; dy = -1; break;
                case ConquerAngle.West: dx = -1; dy = 1; break;
                case ConquerAngle.NorthWest: dx = -1; break;
                case ConquerAngle.SouthWest: dy = 1; break;
                case ConquerAngle.NorthEast: dy = -1; break;
                case ConquerAngle.SouthEast: dx = 1; break;
            }
            Location.X = (ushort)(Location.X + dx);
            Location.Y = (ushort)(Location.Y + dy);
        }
    }
}
