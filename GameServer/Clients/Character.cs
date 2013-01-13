using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public unsafe class Character
    {
        private GameClient Owner;
        public uint ID;
        public uint Model;
        public ushort HairStyle;
        public uint Gold;
        public uint Experience;
        public ushort Strength;
        public ushort Dexterity;
        public ushort Vitality;
        public ushort Spirit;
        public ushort StatPoints;
        public ushort HitPoints;
        public ushort ManaPoints;
        public ushort PKPoints;
        public byte Level;
        public byte Class;
        public byte Reborn;
        public string Name;
        public string Spouse;
        
        
        
        public Character(GameClient Owner)
        {
            this.Owner = Owner;

            Spouse = "NONE";
        }


    }


}
