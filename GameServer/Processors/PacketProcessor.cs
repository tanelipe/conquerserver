using System;
using GameServer.Database;
using GameServer.Scripting;
namespace GameServer.Processors
{ 
    public unsafe class PacketProcessor
    {
        private CommandProcessor CommandProcessor;
        private NpcScripting NpcScriptEngine;
        private DatabaseManager Database;
        

        public PacketProcessor(DatabaseManager Database)
        {
            this.Database = Database;
            CommandProcessor = new CommandProcessor(Database);
            NpcScriptEngine = new NpcScripting();
        }

        public void Process(GameClient Client, byte[] Chunk)
        {
            Client.Packets.Push(Chunk);

            byte[] Packet = Client.Packets.Pop();
            while (Packet != null)
            {
                InternalProcess(Client, Packet);
                Packet = Client.Packets.Pop();
            }
        }

        private void InternalProcess(GameClient Client, byte[] Packet)
        {          
            fixed (byte* pPacket = Packet)
            {
                ushort *Size = (ushort*)(pPacket + 0);
                ushort* Type = (ushort*)(pPacket + 2);

                if (*Size != Packet.Length)
                {
                    SizeMismatch(Client, *Size, Packet);
                    return;
                }

                Kernel.HexDump(Packet, "Client -> Server");

                IPacketProcessor processor = null;
                switch (*Type)
                {
                    case 0x3E9: processor = new CreateCharacterProcessor(Database); break;
                    case 0x3EC: processor = new ChatProcessor(Database, CommandProcessor, NpcScriptEngine); break;
                    case 0x3ED: processor = new MovementProcessor(Database); break;
                    case 0x3F1: processor = new ItemUsageProcessor(Database); break;
                    case 0x3F2: processor = new GeneralDataProcessor(Database); break;
                    case 0x3FE: processor = new AttackProcessor(Database); break;
                    case 0x41C: processor = new LoginTransferProcessor(Database); break;
                    case 0x7EF:
                    case 0x7F0:
                        processor = new NpcProcessor(Database, NpcScriptEngine);
                        break;
                    default:
                        Client.Send(Packet);
                        break;
                }

                if (processor != null)
                {
                    processor.Execute(Client, pPacket);
                }
            }
        }
        private void SizeMismatch(GameClient Client, ushort Size, byte[] Packet)
        {
            int RemainingSize = Packet.Length - Size;
            if (RemainingSize > 0)
            {                
                byte[] Header = new byte[Size];
                Buffer.BlockCopy(Packet, 0, Header, 0, Size);

                byte[] Footer = new byte[RemainingSize];
                Buffer.BlockCopy(Packet, Size, Footer, 0, RemainingSize);

                Process(Client, Header);
                Process(Client, Footer);
            }
        }  
    }
}
