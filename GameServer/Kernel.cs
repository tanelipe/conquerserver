using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    class Kernel
    {
        public static unsafe void HexDump(void* Packet, ushort Size, string Header)
        {
            byte[] Dump = new byte[Size];
            byte* pPacket = (byte*)Packet;
            for (int i = 0; i < Size; i++)
                Dump[i] = pPacket[i];
            HexDump(Dump, Header);

        }
        public static unsafe void HexDump(byte[] Packet, string Header)
        {
            StringBuilder dump = new StringBuilder();

            fixed (byte* pPacket = Packet)
            {
                ushort Size = *(ushort*)pPacket;
                ushort Type = *(ushort*)(pPacket + 2);

                dump.AppendLine(string.Format("[{0} Size: 0x{1:X4} Type: 0x{2:X4}]", Header, Size, Type));
                for (int i = 0; i < Packet.Length; i += 16)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        if (i + j < Packet.Length)
                            dump.AppendFormat("{0:X2} ", Packet[i + j]);
                        else
                            dump.Append("   ");
                    }
                    dump.Append(" ; ");
                    for (int j = 0; j < 16; j++)
                    {
                        if (i + j < Packet.Length)
                        {
                            char Value = (char)Packet[i + j];
                            if (char.IsLetterOrDigit(Value))
                                dump.Append(Value);
                            else
                                dump.Append(".");
                        }
                    }
                    dump.AppendLine();
                }
            }
            Console.WriteLine(dump.ToString());
        }
    }
}
