
namespace GameServer
{
    public struct LearnProfiency
    {
        public ushort Size;
        public ushort Type;
        public uint ID;
        public uint Level;
        public uint Experience;

        public static LearnProfiency Create()
        {
            LearnProfiency Packet = new LearnProfiency();
            Packet.Size = 0x10;
            Packet.Type = 0x401;
            return Packet;
        }
    }
}

/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace RedemptionCO.Network.Packets.Structures
{
    [StructLayout(LayoutKind.Sequential)]
    unsafe struct LearnProfPacket
    {
        public const ushort cSize = 0x10;

        public ushort Size, Type;
        public uint ID, Level, Experience;

        public LearnProfPacket(object nil)
        {
            Size = 0x10;
            Type = 0x401;
            Experience = 0;
            ID = 0;
            Level = 0;
        }

        public static LearnProfPacket* Alloc(byte* Memory)
        {
            LearnProfPacket* LSpell = (LearnProfPacket*)Memory;
            LSpell->Size = 0x10;
            LSpell->Type = 0x401;
            return LSpell;
        }
    }
}

*/
