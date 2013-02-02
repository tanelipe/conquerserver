using System;
using System.Text;

namespace GameServer
{
    public delegate int ConquerCallback(Entity Sender, Entity Receiver);
    public delegate int ConquerCallback<T>(Entity Sender, Entity Receiver, T Param);

    public unsafe class ConquerCallbackKernel
    {
        public static ConquerCallback GetScreenReply = new ConquerCallback(ScreenReply);


        private static int ScreenReply(Entity Sender, Entity Receiver)
        {
            GameClient ReceiverClient = Receiver.Owner;
            ReceiverClient.Screen.Add(Sender);
            return 0;
        }
    }
    public unsafe class Kernel
    {
        private static DMapLoader DmapLoader;

        public static void LoadMaps()
        {
            int Ticks = Environment.TickCount;
            DmapLoader = new DMapLoader();
            DmapLoader.LoadGameMap();
            DmapLoader.LoadMaps();
            Console.WriteLine("Loaded maps in {0} ticks!", Environment.TickCount - Ticks);
        }
        public static bool IsWalkable(ushort MapID, ushort X, ushort Y)
        {
            return DmapLoader.GetAccessible(MapID, X, Y);
        }

        public static int ScreenView = 16;

        public static void GetScreen(GameClient Client, ConquerCallback Callback)
        {
            Client.Screen.Cleanup();

            try
            {
                EntityManager.AcquireLock();

                int Distance = -1;

                GameClient[] Clients = EntityManager.Clients;
                foreach (GameClient Other in Clients)
                {
                    if (Other == null) continue;
                    if (Other.UID == Client.UID) continue;
                    if (Other.Entity.Location.MapID != Client.Entity.Location.MapID) continue;

                    Distance = ConquerMath.CalculateDistance(Other.Entity.Location, Client.Entity.Location);
                    if (Distance <= ScreenView)
                    {
                        if (Client.Screen.Add(Other.Entity))
                        {
                            if (Callback != null)
                            {
                                Callback(Client.Entity, Other.Entity);
                            }
                        }
                    }
                }

                NonPlayerCharacter[] NPCs = EntityManager.GetNonPlayingCharacters(Client.Entity.Location.MapID);
                if (NPCs != null)
                {
                    foreach (NonPlayerCharacter npc in NPCs)
                    {
                        if (npc.Location.MapID != Client.Entity.Location.MapID) continue;
                        Distance = ConquerMath.CalculateDistance(npc.Location, Client.Entity.Location);
                        if (Distance <= ScreenView)
                        {
                            if (Client.Screen.Add(npc))
                            {
                                NpcSpawn Spawn = npc.GetSpawnPacket();
                                Client.Send(&Spawn, Spawn.Size);
                            }
                        }
                    }
                }
            }
            finally
            {
                EntityManager.ReleaseLock();
            }          
        }


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
